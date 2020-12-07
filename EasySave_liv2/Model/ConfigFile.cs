using System;
using System.Collections.Generic;
using System.Text;

namespace EasySave_liv2.Model
{
    class ConfigFile
    {
        public ConfigFile(List<string> prog, List<string> ext)
        {
            Extensions = ext;
            Program = prog;
        }

        public List<string> Extensions { get; set; }
        public List<string> Program { get; set; }
    }
}
