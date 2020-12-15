using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Windows;

namespace EasySave_RemoteClient.src
{
    class Backend : INotifyPropertyChanged
    {
        Thread CTh;
        Socket client;

        private ThreadState _socketThreadState;
        public List<string> _strList = new List<string>();
        public static bool receivedData = false;

        // ManualResetEvent instances signal completion.  
        private static ManualResetEvent connectDone = new ManualResetEvent(false);
        private static ManualResetEvent sendDone = new ManualResetEvent(false);
        private static ManualResetEvent receiveDone = new ManualResetEvent(false);

        // The response from the remote device.  
        private string _response;

        private string message = "";
        public string ip = string.Empty;
        private int port = 11304;

        public static bool hasEnded = false;
        public static bool endedReception = false;
        public static string incomingData = "";

        public Backend()
        {
            this.SocketThreadState = ThreadState.Unstarted;
        }

        public void StartSocketThread(string ipx, string messagex)
        {
            ip = ipx;
            message = messagex;

            CTh = new Thread(new ThreadStart(StartClient));
            CTh.Name = "Client Socket";
            CTh.Start();
            SocketThreadState = CTh.ThreadState;
        }

        internal void LoadDataGrid(IList<string> temp)
        {
           //from listbox to datagrid
        }

        public void ActualizedData()
        {
            receivedData = false;
            foreach (string str in incomingData.Split("||"))
                StrList.Add(str);
            receivedData = true;

        }


        //---------------------------SOCKET ASYNC METHODS----------------------------------

        public void StartClient()
        {
            // Connect to a remote device.  
            try
            {
                // Establish the remote endpoint for the socket.  
                IPHostEntry ipHostInfo = Dns.GetHostEntry(ip);
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

                // Create a TCP/IP socket.  
                client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                switch (message)
                {
                    case "Data":
                        message = "DataXX";
                        break;
                    case "Progress":
                        message = "ProgressXX";
                        break;
                    case "closeConnection":
                        message = "closeConnectionXX";
                        break;
                    default:
                        MessageBox.Show("Error");
                        break;
                }

                // Connect to the remote endpoint.  
                client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), client);
                connectDone.WaitOne();

                // Send test data to the remote device.  
                Send(client, message);
                sendDone.WaitOne();

                // Receive the response from the remote device.  
                Receive(client);
                receiveDone.WaitOne();


                // Release the socket.  
                client.Shutdown(SocketShutdown.Both);
                client.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;

                // Complete the connection.  
                client.EndConnect(ar);

                // Signal that the connection has been made.  
                connectDone.Set();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), CTh.Name);
            }
        }

        private void Receive(Socket client)
        {
            try
            {
                // Create the state object.  
                StateObject state = new StateObject();
                state.workSocket = client;

                // Begin receiving the data from the remote device.  
                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Receive Client");
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the state object and the client socket
                // from the asynchronous state object.  
                StateObject state = (StateObject)ar.AsyncState;
                Socket client = state.workSocket;

                // Read data from the remote device.  
                int bytesRead = client.EndReceive(ar);

                if (bytesRead > 0)
                {
                    // There might be more data, so store the data received so far.  
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                    // Get the rest of the data.  
                    client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReceiveCallback), state);
                }
                else
                {
                    // All the data has arrived; put it in response.  
                    if (state.sb.Length > 1)
                    {
                        incomingData = state.sb.ToString();
                    }
                    
                    if (message == "DataXX")
                    {
                        ActualizedData();
                    }
                        
                    // Signal that all bytes have been received.  
                    receiveDone.Set();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "ReceiveCallback Client");
            }
        }

        private void Send(Socket client, string data)
        {
            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.  
            client.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), client);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = client.EndSend(ar);

                // Signal that all bytes have been sent.  
                sendDone.Set();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "SendCallback Client");
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

        public void RefreshData()
        {
            Send(client, "Data");
            sendDone.WaitOne();
        }

        //-----------------------------PROPERTIES-------------------------

        public ThreadState SocketThreadState
        {
            get { return _socketThreadState; }
            set
            {
                _socketThreadState = value;
                NotifyPropertyChanged("ThreadState");
            }
        }

        public List<string> StrList
        {
            get { return _strList; }
            set
            {
                _strList = value;
                NotifyPropertyChanged("StrList");
            }
        }

        public string Response
        {
            get { return _response; }
            set { _response = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
