using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace EasySave 
{

	public class Model
	{
		//initialize variable for backup
		public string SrcDir { get; set; }
		public string DestDir { get; set; }
		public string BackupName { get; set; }
		public int BackupType { get; set; }
		public bool AllowedBackup { get; set; }


        public long Size { get; private set; }
		public TimeSpan TimeTransfert { get; private set; }
		public List<Newbackup> list { get; set; }
		private string jsonList = @"..\..\..\..\CreatedBackups.json";

		public int backupCount = 0;

		public Model()
		{
			list = LoadList();
			AllowBackup(list);
		}

		//Loads json list with the saves you created
		public List<Newbackup> LoadList()
		{
			
			if (!File.Exists(jsonList))
				File.Create(jsonList);
			
			StreamReader sr = new StreamReader(jsonList);

			var json = sr.ReadToEnd();
			list = JsonConvert.DeserializeObject<List<Newbackup>>(json);
			sr.Close();

			if (list != null)
				this.backupCount = list.Count;

			return list;
		}

		//Allows you or not to create more backups than authorized.
		public void AllowBackup(List<Newbackup> ls)
		{
            if (new FileInfo(jsonList).Length == 0)
            {
				File.Create(jsonList);
				AllowedBackup = true;
			}
			else if (File.ReadAllLines(jsonList) == null)
				AllowedBackup = true;
			else if (new FileInfo(jsonList).Length != 0)
				if (ls.Capacity >= 5)
					AllowedBackup = false;
			else 
				AllowedBackup = true;
            
			
        }

	}
}