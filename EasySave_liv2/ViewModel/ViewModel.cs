﻿using EasySave.Model;
using EasySave.Model.Remote;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading;
using EasySave_RemoteClient.src;
using System.Net.Sockets;
using MessageBox = System.Windows.MessageBox;
using System.Threading.Tasks;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;

namespace EasySave.ViewModel
{
    public class View_Model : INotifyPropertyChanged
    {

        private Backup backup = new Backup();
        private List<string> _programs;
        private List<string> _extensions;
        private List<string> _pextensions;
        private List<Newbackup> _backups;

        public static string ListBoxContent = "no data.";
        internal BinaryFormatter bf = new BinaryFormatter();
        public static string incomingData = "";

        public List<string> listBackup = new List<string>();
        public List<Thread> LThreads = new List<Thread>();

        // Thread signal.  
        public static ManualResetEvent allDone = new ManualResetEvent(false);
         
        //Server socket thread
        private Thread STh;
        //Determines when the socket must end
        private bool hasEnded = false;
        //saves incoming string from client
        private string _request;
    



        public View_Model()
        {
            BackupListToName();
            LoadConfig();
        }


        //Retrieve saves' names to list them
        private void BackupListToName()
        {
            foreach (Newbackup nbu in backup.BackupsList)
                    listBackup.Add(nbu.taskname);        
        }

        //Calls backup creation 
        public void CreateBackupUI(string source, string destination, string taskname, bool differential)
        {

            if (!CheckPathBoxContent(source, destination))
                return;
            int diff = 0;

            DirectoryInfo diS = new DirectoryInfo(source);
            DirectoryInfo diD = new DirectoryInfo(destination);

            if (differential)
                diff = 1;

            backup.CreateBackup(diS, diD, taskname, diff);
        }

        //Find backup object from string
        public void FindBackupByName(string str)
        {
            if (backup.BackupsList.Count > 0)
                foreach(Newbackup bu in backup.BackupsList)
                    if (bu.taskname == str)
                        if (bu.backupType == 1)
                        {
                            new Thread(() => backup.DifferentialCall(new DirectoryInfo(bu.source), new DirectoryInfo(bu.destination), bu.taskname)).Start();
                            return;
                        }
                        else
                        {
                            new Thread(() => backup.Copy(bu.source, bu.destination, bu.taskname)).Start();
                            return;
                        }
                
        }

        //Returns a boolean to MainWindow to prevent from saving or not
        public bool OnSaveProgramPrevention()
        {
            bool isOk = true;
            List<string> apps = this.backup.ProgramPreventClose;

            if (apps != null)
                foreach (string app in apps)
                    if (!backup.OnSaveProgramPrevention_single(app))
                        isOk = false;
                    else
                        isOk = true; 

            return isOk;
        }

        //Writes config on ConfigWindow close and Add button
        internal void SaveConfig(List<string> prog, List<string> ext, List<string> pext)
        {
            this.backup.WriteConfigFile(prog, ext, pext);
        }

        //Check and load configuration for MainWindow use
        internal void LoadConfig()
        {
            this.backup.CheckConfigRequirements();
            Programs = this.backup.ProgramPreventClose;
            Extensions = this.backup.EncryptExt;
            PExtensions = this.backup.PriorityExtensions;
        }

        internal void LoadDataGrid(IList<string> lstr)
        {
            List<Newbackup> temp = new List<Newbackup>();

            if (backup.BackupsList.Count > 0)
                foreach (string str in lstr)
                    foreach (Newbackup bu in backup.BackupsList)
                        if (bu.taskname == str)
                        {
                            temp.Add(bu);
                            break;
                        }
                            
            Backups = temp;
                        
        }

//----------------------------------PATH INPUT CHECK-------------------------------

        private bool CheckPathBoxContent(string source, string destination)
        {
            if (Directory.Exists(source) && Directory.Exists(destination))
                return true;
            else
                return false;
        }

        public string GetSource(string str)
        {
            string res = "";
            foreach (Newbackup bu in backup.BackupsList)
                if (bu.taskname == str)
                {
                    res = bu.source;
                    break;
                }
            return res;     
               
        }

        public string GetDestination(string str)
        {
            string res = "";
            foreach (Newbackup bu in backup.BackupsList)
                if (bu.taskname == str)
                {
                    res = bu.destination;
                    break;
                }
                    
            return res;
        }
    
//-----------------------------SERVER DATA THREAD METHODS----------------------------

        public void StartServer()
        {
            ListBoxContent = string.Join("||", listBackup);
            STh = new Thread(new ThreadStart(StartListening));
            STh.Name = "Server Socket Thread";
            STh.Start();
       
        }

        public void SetSocketData()
        {
            ListBoxContent = "";
            ListBoxContent = string.Join("||", listBackup);
        }
    

//-----------------------------SOCKET COMMUNICATION-----------------------------------

        public void StartListening()
        {

            // Establish the local endpoint for the socket.  
            // running the listener on "localhost".  
            IPHostEntry ipHostInfo = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11304);

            // Create a TCP/IP socket.  
            Socket listener = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.  
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(1);

                allDone.Reset();
                // Start an asynchronous socket to listen for connections.
                listener.BeginAccept(
                    new AsyncCallback(AcceptCallback),
                    listener);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }

        public void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                // Signal the main thread to continue.  
                allDone.Set();

                // Get the socket that handles the client request.  
                Socket listener = (Socket)ar.AsyncState;
                Socket handler = listener.EndAccept(ar);

                // Create the state object.  
                StateObject state = new StateObject();
                state.workSocket = handler;

                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
            }
            catch (Exception e )
            {
                MessageBox.Show(e.ToString(), "AcceptCallback Server");
            }
            
        }

        public void ReadCallback(IAsyncResult ar)
        {
            string content = string.Empty;

            try
            {
                // Retrieve the state object and the handler socket  
                // from the asynchronous state object.  
                StateObject state = (StateObject)ar.AsyncState;
                Socket handler = state.workSocket;

                // Read data from the client socket.
                int bytesRead = handler.EndReceive(ar);

                if (bytesRead > 0)
                {
                    // There  might be more data, so store the data received so far.  
                    state.sb.Append(Encoding.ASCII.GetString(
                        state.buffer, 0, bytesRead));

                    // Check for end-of-file tag. If it is not there, read
                    // more data.  
                    content = state.sb.ToString();

                    if (content.IndexOf("XX") > -1)
                    {
                        switch (content)
                        {
                            case "DataXX":
                                Send(handler, String.Join("||", listBackup));
                                break;
                            case "ProgressXX":

                                break;
                            case "closeConnectionXX":
                                break;
                            default:
                                MessageBox.Show("ErrorXX");
                                break;
                        }
                    }
                    else
                    {
                        // Not all data received. Get more.  
                        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReadCallback), state);
                    }
                   
                    //handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    //new AsyncCallback(ReadCallback), state);

                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "ReadCallback Server");
            }
            
        }

        private void Send(Socket handler, String data)
        {
            try
            {
                // Convert the string data to byte data using ASCII encoding.  
                byte[] byteData = Encoding.ASCII.GetBytes(data);

                // Begin sending the data to the remote device.  
                handler.BeginSend(byteData, 0, byteData.Length, 0,
                    new AsyncCallback(SendCallback), handler);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Send Server");
            }
            
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = handler.EndSend(ar);



                //APRES RENVOI DE DATA JE FAIS QUOI LA ??
                //BEGINRECEIVE ???




                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "SendCallback Server");
            }
        }

   
        bool SocketConnected(Socket s)
        {
            bool part1 = s.Poll(1000, SelectMode.SelectRead);
            bool part2 = (s.Available == 0);
            if (part1 && part2)
                return false;
            else
                return true;
        }

        //--------------------------INOTIFY and PROPERTIES ---------------------------------------------- 


        //attributes for config window
        public List<string> Programs
        {
            get { return _programs ?? new List<string>(); }
            set
            {
                _programs = value;
                NotifyPropertyChanged("Programs");
            }
        }
        public List<string> Extensions
        {
            get { return _extensions ?? new List<string>(); }
            set
            {
                _extensions = value;
                NotifyPropertyChanged("Extensions");
            }
        }
        public List<string> PExtensions
        {
            get { return _pextensions ?? new List<string>(); }
            set
            {
                _pextensions = value;
                NotifyPropertyChanged("PEextensions");
            }
        }
        public List<Newbackup> Backups
        {
            get { return _backups; }
            set
            {
                _backups = value;
                NotifyPropertyChanged("Backups");
            }
        }

        public string Request
        {
            get { return _request; }
            set 
            { 
                _request = value; 
            }
        }

        // Property Change Logic  
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

        }
    }
}