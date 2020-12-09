using System;
using System.Collections.Generic; // used for data types
using System.Diagnostics; // used to calculate time
using System.IO; // used to manage files and directories
using System.Threading; // used for progression tests (not required)
using Newtonsoft.Json; // used for json

namespace EasySave_liv2.Model
{
    public class RealTimeJson
    {
        // initialize variables for real time json file (Used once a backup is executed)

        public DateTime creationTime { get; set; }
        public DateTime lastWriteTime { get; set; }
        public string extension { get; set; }
        public string path { get; set; }
        public string name { get; set; }
        public long CurrentFileSize { get; set; }
        public int remaining_files { get; set; }
        public string progression { get; set; }
        public string bytesRemaining { get; set; }

    }
}
