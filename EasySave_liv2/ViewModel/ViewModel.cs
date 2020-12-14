using EasySave.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace EasySave.ViewModel
{
    public class View_Model : INotifyPropertyChanged
    {

        private Backup backup = new Backup();
        private List<string> _programs;
        private List<string> _extensions;
        private List<string> _pextensions;
        private List<Newbackup> _backups;

        public List<string> listBackup = new List<string>();
        

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
            get { return _backups ?? new List<Newbackup>(); }
            set
            {
                _backups = value;
                NotifyPropertyChanged("Backups");
            }
        }

        // Property Change Logic  
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

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

        internal void loadDataGrid(IList<string> lstr)
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
    }
}
