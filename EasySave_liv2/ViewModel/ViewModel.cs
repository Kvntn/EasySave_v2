using EasySave_liv2.Model;
using System.Collections.Generic;
using System.IO;

namespace EasySave_liv2.ViewModel
{
    public class View_Model
    {

        private Backup backup = new Backup();
        public List<string> listBackup = new List<string>();

        //attributes for config window
        public List<string> Programs = new List<string>();
        public List<string> Extensions = new List<string>();

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
                        backup.Differential(new DirectoryInfo(bu.source), new DirectoryInfo(bu.destination), bu.taskname);
                        return;
                    }  
                    else
                    {
                        backup.Copy(bu.source, bu.destination, bu.taskname);
                        return;
                    }
  
        }

        //Returns boolean to MainWindow to prevent save or not
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
        internal void SaveConfig(List<string> prog, List<string> ext)
        {
            this.backup.WriteConfigFile(prog, ext);
        }

        //Check and load configuration for MainWindow use
        internal void LoadConfig()
        {
            this.backup.CheckConfigRequirements();
            this.Programs = this.backup.ProgramPreventClose;
            this.Extensions = this.backup.EncryptExt;
        }
    }
}
