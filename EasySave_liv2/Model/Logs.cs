using System;
using System.Collections.Generic; // used for data types
using System.Diagnostics; // used to calculate time
using System.IO; // used to manage files and directories
using System.Threading; // used for progression tests (not required)
using Newtonsoft.Json; // used for json
namespace EasySave_liv2.Model
{
    public class Logs
    {
        // (used for backup logs, created in each backup creation)

        public string taskname { get; set; }
        public string source { get; set; }
        public string destination { get; set; }
        public int backupType { get; set; }
        public long filesize { get; set; }
        public DateTime Horodotage { get; set; }
        public double time { get; set; }
        public double encryptionTime { get; set; }
    }

}
