using System;


namespace EasySave
{
	public class Model
	{
		public string SrcDir { get; set; }
		public string DestDir { get; set; }
		public string SaveName { get; set; }
		public int SaveType { get; set; }
		public string stateFile;
		public string logFile;

		public Model()
		{

		}

		public bool SaveFile()
        {
			return true;
        }

		public void StateFile()
		{
			
		}

		public void LogFile()
		{
			
		}


	}
}