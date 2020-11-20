using System;
using System.Collections.Generic;
using System.Text;

namespace EasySave
{
    class Controller
    {
        private Model model;
        private View view;
        public string srcDir;
        public string destDir;

        public Controller(Model modelx, View viewx)
        {
            model = modelx;
            view = viewx;
            view.Start();
        }

        public bool CheckDirExistence()
        {
            return true;
        }

        public void GetSrcDir()
        {

        }

        public void GetDestDir()
        {

        }


    }
}
