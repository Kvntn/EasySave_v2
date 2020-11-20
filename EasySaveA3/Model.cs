using System;


namespace EasySave { 

	public class Model
	{
		public string SrcDir { get; set; }
		public string DestDir { get; set; }
		public string BackupName { get; set; }
		public int BackupType { get; set; }
        public bool AllowedBackup { get; set; }	
		public BackupList list { get; set; }

		private string stateFile;
		private string logFile;


		public Model()
		{
			list = new BackupList();
		}

		public bool BackupFile()
        {
			return true;
        }

		public void StateFile()
		{
			
		}

		public void LogFile()
		{

		}

		public void Fullbackup()
		{

		}

		public void DifferentialBackup()
		{

		}

		public void AllowBackup()
		{

		}




	}
}