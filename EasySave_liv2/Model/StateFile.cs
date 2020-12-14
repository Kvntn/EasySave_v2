using System;
using System.IO;
using Newtonsoft.Json;

namespace EasySave.Model
{
    public class StateFile
    {
        public string backupName;
        public DateTime timeStamp;
        public bool backupState;
        public int nbFiles = 0;
        public long sizeFiles = 0;
        public double progression;
        public int leftFiles;
        public long leftSize;
        public string sourcePath;
        public string targetPath;
        public int backupType;

        public StateFile(string sourcePath, string targetPath, string backupName, int type, int nbFiles, long sizeFiles)
        {
            if (type == 1)
            {
                this.sourcePath = sourcePath;
                this.targetPath = targetPath;
                this.backupName = backupName;
                this.timeStamp = DateTime.Now;

                this.GetSourceStateDiff(nbFiles, sizeFiles);

                this.GetTargetState();
                this.WriteStateFile();

            }
            else if (type == 0)
            {
                this.sourcePath = sourcePath;
                this.targetPath = targetPath;
                this.backupName = backupName;
                this.timeStamp = DateTime.Now;

                this.GetSourceStateFull();

                this.GetTargetState();
                this.WriteStateFile();

            } 
        }

        public void GetSourceStateFull()
        {
            //get source directory info (size, files)
            DirectoryInfo sourcedi = new DirectoryInfo(sourcePath);
            var sourcedirectories = sourcedi.GetFiles("*", SearchOption.AllDirectories);

            foreach (FileInfo d in sourcedirectories)
            {
                this.sizeFiles += d.Length;
                this.nbFiles++;

            }
        }

        public void GetSourceStateDiff(int nbFiles, long sizeFiles)
        {
            this.sizeFiles = sizeFiles;
            this.nbFiles = nbFiles;
        }


        public void GetTargetState()
        {
            //get target directory info (size, files)
            DirectoryInfo targetdi = new DirectoryInfo(targetPath);
            var targetdirectories = targetdi.GetFiles("*", SearchOption.AllDirectories);

            //tmp variables for target path
            long targetSize = 0;
            int targetFiles = 0;

            foreach (FileInfo d in targetdirectories)
            {
                targetSize += d.Length;
                targetFiles++;
            }

            this.leftSize = this.sizeFiles - targetSize;
            this.leftFiles = this.nbFiles - targetFiles;

            double tmp1 = targetSize / 1.0;
            double tmp2 = this.sizeFiles / 1.0;

            this.progression = tmp1 / tmp2 * 100;

        }


        public void WriteStateFile()
        {
            string pathToCreate = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string pathToCheck = pathToCreate + "\\EasySave\\statefile.json";

            if (!Directory.Exists(pathToCheck))
            {
                Directory.CreateDirectory(pathToCreate + "\\EasySave\\statefile.json");
            }
            string jsonPath = pathToCheck + "\\" + this.backupName + ".json";

            if (!File.Exists(jsonPath))
            {
                File.Create(pathToCheck + "\\" + this.backupName + ".json").Close();
            }

            if (new FileInfo(jsonPath).Length != 0)
            {
                File.WriteAllText(jsonPath, string.Empty);
            }
            string json = JsonConvert.SerializeObject(this);

            using (var tw = new StreamWriter(jsonPath, true))
            {
                tw.WriteLine(json.ToString());
                tw.Close();
            }
        }

        public void UpdateState()
        {
            this.GetTargetState();
            this.WriteStateFile();
        }

    }

}

