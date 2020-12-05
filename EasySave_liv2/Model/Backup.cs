using System;
using System.Collections.Generic; // used for data types
using System.ComponentModel;
using System.Diagnostics; // used to calculate time
using System.IO; // used to manage files and directories
using System.Linq;
using System.Threading; // used for progression tests (not required)
using Newtonsoft.Json; // used for json

namespace EasySave_liv2.Model
{
    class Backup : INotifyPropertyChanged
    {

        //implmemented by interface INotifyPorpertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        // initialize variables for real time Json
        static long copiedBytes = 0;
        public static long totalFolderSize = 0;
        public static int filesNBcount = 0;


/*        // initialize variables for sequential methode
        public static string taskname;
        public static int backupType;
        public static double time;
        public static int jsonLength;*/

        // list for extensions that need encryption
        public List<string> EncryptExt = new List<string>();

        //detailed backup list
        public int backupCount = 0;
        public List<ConfigFile> LoadedConfig { get; set; }
        public List<Newbackup> BackupsList { get; set; }
        public List<Newbackup> SelectedBackups { get; set; }

        private readonly static string documentStorage = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\EasySave\";
        private readonly static string jsonList = documentStorage + @"\CreatedBackups.json";
        private readonly static string logfilepath = documentStorage + @"\ExecutedBackups.json";
        private readonly static string configFile = documentStorage + @"\config.json";

        // check background app to pursue process
        //public static bool pursue;

        public Backup()
        {
            BackupsList = LoadList();
        }

        //Loads json list with the saves you created
        private List<Newbackup> LoadList()
        {

            if (!File.Exists(jsonList))
                File.Create(jsonList);

            StreamReader sr = new StreamReader(jsonList);

            var json = sr.ReadToEnd();
            BackupsList = JsonConvert.DeserializeObject<List<Newbackup>>(json);
            sr.Close();

            if (BackupsList != null)
                backupCount = BackupsList.Count;

            return BackupsList;
        }

        // logs for executed backups
        private void WriteLogs(DirectoryInfo source, DirectoryInfo destination, string taskname, double timer, int type)
        {
            if (!File.Exists(logfilepath))
            {
                if (!new DirectoryInfo(documentStorage).Exists)
                    new DirectoryInfo(documentStorage).Create();
                File.Create(logfilepath);
            }

            StreamReader sr = new StreamReader(logfilepath);
            dynamic json = sr.ReadToEnd();
            List<Logs> saveData = JsonConvert.DeserializeObject<List<Logs>>(json);
            if (saveData == null)
                saveData = new List<Logs>();
            sr.Close();
            File.WriteAllText(logfilepath, string.Empty);

            saveData.Add(new Logs()
            {
                taskname = taskname,
                Horodotage = DateTime.Now,
                source = source.FullName,
                backupType = type,
                destination = destination.FullName,
                filesize = totalFolderSize,
                time = timer
            });

            json = JsonConvert.SerializeObject(saveData.ToArray(), Formatting.Indented); //Formatting.Indented is used for a pretty json file
                                                   // Write string to file in json format
            File.AppendAllText(logfilepath, json); // call parent target folder to save json
                                                   // appendAllText to add string instead of replace

        }
        
        // logs for created backups
        public void CreateBackup(DirectoryInfo source, DirectoryInfo destination, string taskname, int backupType)
        {

            if (!File.Exists(jsonList))
            {
                if (!new DirectoryInfo(documentStorage).Exists)
                    new DirectoryInfo(documentStorage).Create();
                File.Create(jsonList);
            }

            StreamReader sr = new StreamReader(jsonList);
            var json = sr.ReadToEnd();
            List<Newbackup> saveData = JsonConvert.DeserializeObject<List<Newbackup>>(json);
            if (saveData == null)
                saveData = new List<Newbackup>();
            sr.Close();

            File.WriteAllText(jsonList, string.Empty);

            saveData.Add(new Newbackup()
            {
                taskname = taskname,
                source = source.FullName,
                destination = destination.FullName,
                backupType = backupType
            });


            json = JsonConvert.SerializeObject(saveData.ToArray(), Formatting.Indented); //Formatting.Indented is used for a pretty json file                                                  
            File.AppendAllText(jsonList, json); // Write string to file in json format
        }


        private void RealTimeJson(FileInfo fi, int totalFolderFiles, DirectoryInfo destination)
        {

            List<RealTimeJson> folderData = new List<RealTimeJson>();

            // Serialize real-time data to json file

            folderData.Add(new RealTimeJson()
            {
                path = fi.FullName,
                name = fi.Name,
                CurrentFileSize = fi.Length,
                creationTime = fi.CreationTime,
                lastWriteTime = fi.LastWriteTime,
                extension = fi.Extension,
                remaining_files = totalFolderFiles - filesNBcount,
                progression = filesNBcount + " out of " + totalFolderFiles,
                bytesRemaining = copiedBytes + " out of " + totalFolderSize
            });
            string json = JsonConvert.SerializeObject(folderData.ToArray(), Formatting.Indented); //Formatting.Indented is used for a pretty json file

            // Write string to file in json format
            File.WriteAllText(destination.Parent.FullName + "/json", json); // call parent target folder to save json
                                                                            // writeAllText to replace the text used for real time
                                                                            //  "/json" to name json file
        } 

        //full backup
        public void Copy(string sourceDirectory, string targetDirectory)
        {

            DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
            DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);

            // Calculate total folder size
            foreach (FileInfo fi in diSource.GetFiles("*.*", SearchOption.AllDirectories))
            {
                totalFolderSize += fi.Length;
            }

            CopyAll(diSource, diTarget);
        }

        //Functional on everyfile and within recursion
        private void CopyOrEncrypt(FileInfo file, DirectoryInfo target, bool overwrite)
        {
            EncryptExt.Add(".txt");

            if (EncryptExt.Count() != 0)
            {
                foreach (string ext in EncryptExt)
                    if (Path.GetExtension(file.FullName) == ext)
                    {
                        EncryptFile(file.FullName, Path.Combine(target.Name, file.Name));
                        return;

                    }else {

                        file.CopyTo(Path.Combine(target.FullName, file.Name), overwrite);
                        //Console.WriteLine("Copying file    | {0}", file.Name);
                        return;
                    }
            }
            else
            {
                file.CopyTo(Path.Combine(target.FullName, file.Name), overwrite);
            }
        }

        //used for all types of save
        public void CopyAll(DirectoryInfo source, DirectoryInfo target) 
        {

            int totalFolderFiles = source.GetFiles("*", SearchOption.AllDirectories).Length;

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                filesNBcount++;
                copiedBytes += fi.Length; // increment the size of each copied file.

                // Serialize real-time data to json file
                CopyOrEncrypt(fi, target, true);
                // Write string to file in json format
                RealTimeJson(fi, totalFolderFiles, target);  
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }

        //differential backup, supports encryption
        public void Differential(DirectoryInfo source, DirectoryInfo destination)
        {
            // Copy files.  
            bool Emptydestination = true;
            FileInfo[] files = source.GetFiles();
            FileInfo[] destFiles = destination.GetFiles();
            int totalFolderFiles = 0;
            long totalSize = 0;

            if (Directory.GetFileSystemEntries(destination.FullName).Length != 0)
            {
                Emptydestination = false;
            }


            // Claculating the totalt number of files to copy
            foreach (FileInfo file in files)
            {
                foreach (FileInfo fileD in destFiles)
                {
                    // Copy only modified files
                    if (file.Name == fileD.Name && file.LastWriteTime > fileD.LastWriteTime)
                    {
                        totalFolderFiles++;
                        totalSize = totalSize + fileD.Length;
                    }

                    // Copy all new files  
                    else if (!File.Exists(Path.Combine(destination.FullName, file.Name)))
                    {
                        totalFolderFiles++;
                        totalSize = totalSize + fileD.Length;
                    }
                }
            }

            foreach (FileInfo file in files)
            {
                foreach (FileInfo fileD in destFiles)
                {
                    Emptydestination = false;
                    // Copy only modified files
                    if (file.Name == fileD.Name && file.LastWriteTime > fileD.LastWriteTime)
                    {
                        CopyOrEncrypt(fileD, destination, true);
                        filesNBcount++;
                        RealTimeJson(file, totalFolderFiles, destination);
                    }
                    // Copy all new files  
                    else if (!File.Exists(Path.Combine(destination.FullName, file.Name)))
                    {
                        CopyOrEncrypt(fileD, destination, true);
                        filesNBcount++;
                        RealTimeJson(file, totalFolderFiles, destination);
                    }
                }
            }

            if (Emptydestination == true)
            {
                CopyAll(source, destination);
            }

            // Process subdirectories.  
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                filesNBcount++;
                DirectoryInfo nextTargetSubDir =
                destination.CreateSubdirectory(diSourceSubDir.Name);
                Differential(diSourceSubDir, nextTargetSubDir);
            }
        }

        //pour le logiciel métier, c'était pour pas l'oublier que j'ai mis ça
        public bool isWorkspaceClosed()
        {
            return false;
        }

        //starts CryptoSoft.exe process for each file
        private void EncryptFile(string src, string dst)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                Arguments = "\"" + src + " \"" + dst + "\"",
                FileName = @"./info/CryptoSoft.exe"
            };

            using (Process p = new Process())
            {
                p.StartInfo = startInfo;
                p.Start();
            }
        }

        //implemented from interface INotifyPropertyChanged
        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        private List<ConfigFile> ReadConfigFile()
        {
            if (!File.Exists(configFile))
            {
                if (!new DirectoryInfo(documentStorage).Exists)
                    new DirectoryInfo(documentStorage).Create();
                File.Create(configFile);
            }

            StreamReader sr = new StreamReader(configFile);
            var json = sr.ReadToEnd();
            List<ConfigFile> saveData = JsonConvert.DeserializeObject<List<ConfigFile>>(json);
            if (saveData == null)
                saveData = new List<ConfigFile>();
            sr.Close();

            return saveData;
        }
    
    }
}
