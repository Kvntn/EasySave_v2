using System;
using System.Collections.Generic;
using System.Text;

namespace EasySave.Model.Remote
{
    public class ClientObjectFormat
    {
        private string _name;
        private int _percent;

        public ClientObjectFormat(string name, int percentage)
        {
            Name = name;
            Percent = percentage;
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int Percent
        {
            get { return _percent; }
            set { _percent = value; }
        }

       

    }
}
