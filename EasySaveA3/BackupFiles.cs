using System;
using System.Collections.Generic; // used for data types
using System.Diagnostics; // used to calculate time
using System.IO; // used to manage files and directories
using System.Threading; // used for progression tests (not required)
using Newtonsoft.Json; // used for json

namespace EasySave
{
    class BackupFiles
    {

        // initialize variables for real time Json

        static long copiedBytes = 0;
        public static long totalFolderSize = 0;
        public static int filesNBcount = 0;

        // initialize variables for sequential methode

        public static string taskname;
        public static int backupType;
        public static double time;
        public static int jsonLength;
        public static dynamic BackupsList;

        // check background app to persue process

        public static bool pursue;

        // share total files to copy in sequential backup (for progressbar)

        public static int SeqtotalFiles;
        public static bool seqType;


        public static void Copy(string sourceDirectory, string targetDirectory)
        {

            DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
            DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);

            // Calculate total folder size

            DirectoryInfo dir = new DirectoryInfo(sourceDirectory);

            foreach (FileInfo fi in dir.GetFiles("*.*", SearchOption.AllDirectories))
            {
                totalFolderSize += fi.Length;
            }

            CopyAll(diSource, diTarget);

        }


        public static void WriteLogs(DirectoryInfo source, DirectoryInfo destination, string taskname, double timer)
        {
            // logs for executed backups

            string logfilepath = @"..\..\..\..\ExecutedBackups.json";

            if (!File.Exists(logfilepath))
                File.Create(logfilepath);

            StreamReader sr = new StreamReader(logfilepath);
            dynamic json = sr.ReadToEnd();
            List<Logs> saveData = JsonConvert.DeserializeObject<List<Logs>>(json);
            sr.Close();
            File.WriteAllText(logfilepath, string.Empty);

            saveData.Add(new Logs()
            {
                taskname = taskname,
                Horodotage = DateTime.Now,
                source = source.FullName,
                backupType = backupType,
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

        public static void CreateBackup(DirectoryInfo source, DirectoryInfo destination, string taskname, int backupType)
        {
            string BackupsFile = @"..\..\..\..\CreatedBackups.json";

            if (!File.Exists(BackupsFile))
            {
                File.Create(BackupsFile);
            }

            StreamReader sr = new StreamReader(BackupsFile);
            var json = sr.ReadToEnd();
            List<Newbackup> saveData = JsonConvert.DeserializeObject<List<Newbackup>>(json);
            sr.Close();

            File.WriteAllText(BackupsFile, string.Empty);

            saveData.Add(new Newbackup()
            {
                taskname = taskname,
                source = source.FullName,
                destination = destination.FullName,
                backupType = backupType
            });

            json = JsonConvert.SerializeObject(saveData.ToArray(), Formatting.Indented); //Formatting.Indented is used for a pretty json file                                                  
            File.AppendAllText(BackupsFile, json); // Write string to file in json format
        }


        public static void RealTimeJson(FileInfo fi, int totalFolderFiles, DirectoryInfo destination)
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


        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {

            int totalFolderFiles = source.GetFiles("*", SearchOption.AllDirectories).Length;

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                Thread.Sleep(2500);  // a second sleep for each file is copied (just to look cool :D)
                filesNBcount++;
                copiedBytes += fi.Length; // increment the size of each copied file.
                if (seqType)
                {
                    //drawTextProgressBar(SeqtotalFiles); // Backup progress Bar
                }
                else
                {
                    //drawTextProgressBar(totalFolderFiles); // Backup progress Bar

                }
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
                // Serialize real-time data to json file
                RealTimeJson(fi, totalFolderFiles, target);
                // Write string to file in json format
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                filesNBcount++;
                //drawTextProgressBar(totalFolderFiles); //  Completing progress Bar for sub files

                DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }


        public static void Differential(DirectoryInfo source, DirectoryInfo destination)
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
                        file.CopyTo(Path.Combine(destination.FullName, file.Name), true);
                        filesNBcount++;
                        RealTimeJson(file, totalFolderFiles, destination);
                    }
                    // Copy all new files  
                    else if (!File.Exists(Path.Combine(destination.FullName, file.Name)))
                    {
                        file.CopyTo(Path.Combine(destination.FullName, file.Name), true);
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

    }
}
