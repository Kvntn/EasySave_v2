using EasySave_liv2.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EasySave_liv2.ViewModel
{
    class View_Model
    {

        private Backup backup = new Backup();
        public List<string> listBackup = new List<string>();

        public View_Model()
        {
            BackupListToName();
        }

        private void BackupListToName()
        {
            foreach (Newbackup nbu in backup.BackupsList)
                    listBackup.Add(nbu.taskname); 
                
        }

        public void CreateBackupUI(string source, string destination, string taskname, bool differential)
        {
            int diff = 0;

            DirectoryInfo diS = new DirectoryInfo(source);
            DirectoryInfo diD = new DirectoryInfo(destination);

            if (differential)
                diff = 1;

            backup.CreateBackup(diS, diD, taskname, diff);
        }

        public void FindBackupByName(string str)
        {
            foreach(Newbackup bu in backup.BackupsList)
                if (bu.taskname == str)
                    if (bu.backupType == 1)
                    {
                        backup.Differential(new DirectoryInfo(bu.source), new DirectoryInfo(bu.destination));
                        return;
                    }  
                    else
                    {
                        backup.CopyAll(new DirectoryInfo(bu.source), new DirectoryInfo(bu.destination));
                        return;
                    }
  
        }
    }
}
