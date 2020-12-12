using System;
using System.Collections.Generic;
using System.Text;

namespace EasySave.Model
{
    class ConfigFile
    {
        public ConfigFile(List<string> prog, List<string> ext, List<string> pext)
        {
            Extensions = ext;
            Program = prog;
            PriorityExt = pext;
        }

        public List<string> Extensions { get; set; }
        public List<string> Program { get; set; }
        public List<string> PriorityExt { get; set; }
    }
}
