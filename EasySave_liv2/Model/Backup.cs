﻿using System;
using System.Collections.Generic; // used for data types
using System.ComponentModel;
using System.Diagnostics; // used to calculate time
using System.IO; // used to manage files and directories
using System.Linq;
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

        // list for extensions that need encryption
        public List<string> EncryptExt;

        // list of programs that prevent from closing
        public List<string> ProgramPreventClose;

        //detailed backup list
        public int backupCount = 0;
        public List<ConfigFile> LoadedConfig { get; set; }
        public List<Newbackup> BackupsList { get; set; }
        public List<Newbackup> SelectedBackups { get; set; }
        private ConfigFile Config { get; set; }

        //--- ALL PATHS OF RELATED JSON FILES
        private readonly static string documentStorage = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\EasySave\";
        private readonly static string jsonList = documentStorage + @"\CreatedBackups.json";
        private readonly static string logfilepath = documentStorage + @"\ExecutedBackups.json";
        private readonly static string configFile = documentStorage + @"\config.json";


        public Backup()
        {
            BackupsList = LoadList();
            CheckConfigRequirements();
        }



        //Loads config file to allow modifications.
        private ConfigFile ReadConfigJson()
        {
            if (!File.Exists(configFile))
                File.Create(configFile);

            StreamReader sr = new StreamReader(configFile);

            var json = sr.ReadToEnd();
            dynamic conf = JsonConvert.DeserializeObject<ConfigFile>(json);
            sr.Close();

            return conf;
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


//-------------------JSON FILES WRITING LOG/SAVE STATUS--------------------------------------

        // logs for executed backups
        private void WriteLogs(DirectoryInfo source, DirectoryInfo destination, string taskname, double timer, int type, double cryptime)
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
                time = timer,
                encryptionTime = cryptime
            });

            json = JsonConvert.SerializeObject(saveData.ToArray(), Formatting.Indented); //Formatting.Indented is used for a pretty json file
                                                                                         // Write string to file in json format
            File.AppendAllText(logfilepath, json); // call parent target folder to save json
                                                   // appendAllText to add string instead of replace

        }

        //Writes backup's status progressively
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
                progression = (filesNBcount / totalFolderFiles * 100).ToString(),
                bytesRemaining = copiedBytes + " out of " + totalFolderSize
            }); ;
            string json = JsonConvert.SerializeObject(folderData.ToArray(), Formatting.Indented); //Formatting.Indented is used for a pretty json file

            // Write string to file in json format
            File.WriteAllText(destination.Parent.FullName + "/json", json); // call parent target folder to save json
                                                                            // writeAllText to replace the text used for real time
                                                                            //  "/json" to name json file
        }

        //writes ConfigFile on configuration window closing 
        public void WriteConfigFile(List<string> prog, List<string> ext)
        {
            if (!File.Exists(configFile))
            {
                if (!new DirectoryInfo(documentStorage).Exists)
                    new DirectoryInfo(documentStorage).Create();
                File.Create(configFile);
            }

            if (ext == null)
                ext = new List<string>();
            if (prog == null)
                prog = new List<string>();

            ConfigFile configJson = new ConfigFile(prog, ext);

            File.WriteAllText(configFile, string.Empty);

            File.AppendAllText(
                configFile,
                JsonConvert.SerializeObject(configJson, Formatting.Indented)
            );


        }

        //------------------BACKUP RELATED METHODS-----------------------------------

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

        //full backup
        public void Copy(string sourceDirectory, string targetDirectory, string name)
        {
            CheckBuildTargetFolder(targetDirectory);

            Stopwatch sw = new Stopwatch();
            sw.Start();

            DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
            DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);

            // Calculate total folder size
            foreach (FileInfo fi in diSource.GetFiles("*.*", SearchOption.AllDirectories))
            {
                totalFolderSize += fi.Length;
            }

            double cryptime = CopyAll(diSource, diTarget);

            sw.Stop();

            WriteLogs(diSource, diTarget, name, sw.ElapsedMilliseconds, 0, cryptime);
        }

        //used for all types of save
        public double CopyAll(DirectoryInfo source, DirectoryInfo target)
        {

            int totalFolderFiles = source.GetFiles("*", SearchOption.AllDirectories).Length;
            double cryptime = 0;
            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                filesNBcount++;
                copiedBytes += fi.Length; // increment the size of each copied file.

                // Serialize real-time data to json file
                // Write string to file in json format
                RealTimeJson(fi, totalFolderFiles, target);
                cryptime = cryptime + CopyOrEncrypt(fi, target, true);


            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                if (diSourceSubDir.Name != diSourceSubDir.Parent.Name)
                {
                    DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                    CopyAll(diSourceSubDir, nextTargetSubDir);
                }

            }

            return cryptime;
        }

        //differential backup, supports encryption
        public void Differential(DirectoryInfo source, DirectoryInfo destination, string name)
        {
            CheckBuildTargetFolder(destination.FullName);

            Stopwatch sw = new Stopwatch();
            sw.Start();

            FileInfo[] files = source.GetFiles();
            FileInfo[] destFiles = destination.GetFiles();

            bool Emptydestination = true;
            int totalFolderFiles = 0;
            long totalSize = 0;
            double cryptime = 0;

            if (Directory.GetFileSystemEntries(destination.FullName).Length != 0)
            {
                Emptydestination = false;
            }


            // Calculating the total number of files to copy
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
                        cryptime = cryptime + CopyOrEncrypt(fileD, destination, true);
                        filesNBcount++;
                        RealTimeJson(file, totalFolderFiles, destination);
                    }
                    // Copy all new files  
                    else if (!File.Exists(Path.Combine(destination.FullName, file.Name)))
                    {
                        cryptime = cryptime + CopyOrEncrypt(fileD, destination, true);
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
                if (diSourceSubDir.Name != diSourceSubDir.Parent.Name)
                {
                    DirectoryInfo nextTargetSubDir = destination.CreateSubdirectory(diSourceSubDir.Name);
                    Differential(diSourceSubDir, nextTargetSubDir, name);
                }

            }
            sw.Stop();
            WriteLogs(source, destination, name, sw.ElapsedMilliseconds, 0, cryptime);
        }




//---------------ENCRYPTION RELATED METHODS------------------------------------

        //Functional on everyfile and within recursion
        private double CopyOrEncrypt(FileInfo file, DirectoryInfo target, bool overwrite)
        {
            Stopwatch cryptime = new Stopwatch();

            if (EncryptExt.Count() != 0)
            {
                foreach (string ext in EncryptExt)
                    if (Path.GetExtension(file.FullName) == ext)
                    {
                        cryptime.Start();
                        EncryptFile(file.FullName, Path.Combine(target.FullName, file.Name));
                        cryptime.Stop();
                        return cryptime.ElapsedMilliseconds;
                    }
                    else
                    {
                        file.CopyTo(Path.Combine(target.FullName, file.Name), overwrite);
                        return 0;
                    }
            }
            else
            {
                file.CopyTo(Path.Combine(target.FullName, file.Name), overwrite);
                return 0;
            }
            return 0;
        }

        //starts CryptoSoft.exe process for each file
        private void EncryptFile(string src, string dst)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                Arguments = "\"" + src + "\" \"" + dst + "\"",
                FileName = @"./res/CryptoSoft/CryptoSoft.exe"
            };

            using (Process p = new Process())
            {
                p.StartInfo = startInfo;
                p.Start();
            }
        }



//-----------------CONFIG RELATED METHODS -----------------------------------


        //used to load config into variables
        public void CheckConfigRequirements()
        {
            this.Config = ReadConfigJson();

            if (Config != null)
            {
                if (Config.Extensions != null)
                {
                    EncryptExt = Config.Extensions;
                    foreach (string str in EncryptExt)
                        if (str == "")
                            EncryptExt.Remove("");  
                }
                   
                else
                    EncryptExt = new List<string>();

                if (Config.Program != null)
                {
                    ProgramPreventClose = Config.Program;
                    foreach (string str in ProgramPreventClose)
                        if (str == "")
                            ProgramPreventClose.Remove("");
                }
                    
                else
                    ProgramPreventClose = new List<string>();
            }
           
                
        }

        //check if a program marked in the config file is open and prevent save
        //returns true if save can continue.
        public bool OnSaveProgramPrevention_single(string app)
        {
            Process[] processlist = Process.GetProcessesByName(app);

            if (processlist.Length != 0 && processlist != null)
                return false;
            else
                return true;

        }

        //Creates path to the save if it has been deleted
        private void CheckBuildTargetFolder(string target)
        {
            string[] splitted = target.Split(Path.DirectorySeparatorChar);
            string build = "";

            for (int i = 0; i < splitted.Length; i++)
            {
                build = Path.Combine(build, splitted[i]);
                if (!Directory.Exists(build))
                    Directory.CreateDirectory(build);
            }
        }
       

//--------------------------------------------------------------------------

        //implemented from interface INotifyPropertyChanged
        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }


    }
}
