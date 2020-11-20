using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace EasySave
{
    public class View
    {

        public View()
        {

        }

        public void Start(int nbBackup)
        {
            Console.WriteLine("\n$$$$$$$$\\                               $$$$$$\\ "
                            + "\n$$  _____ |                            $$  __$$\\                               "
                            + "\n$$ |      $$$$$$\\   $$$$$$$\\ $$\\   $$\\ $$ /  \\__ | $$$$$$\\ $$\\    $$\\  $$$$$$\\  "
                            + "\n$$$$$\\    \\____$$\\ $$  _____ |$$ |  $$ |\\$$$$$$\\   \\____$$\\\\$$\\  $$  |$$  __$$\\ "
                            + "\n$$  __ |   $$$$$$$ |\\$$$$$$\\  $$ |  $$ | \\____$$\\  $$$$$$$ |\\$$\\$$  / $$$$$$$$ |"
                            + "\n$$ |     $$  __$$ | \\____$$\\ $$ |  $$ |$$\\   $$ |$$  __$$ | \\$$$  /  $$   ____ |"
                            + "\n$$$$$$$$\\\\$$$$$$$ |$$$$$$$  |\\$$$$$$$ |\\$$$$$$  |\\$$$$$$$ |  \\$  /   \\$$$$$$$\\ "
                            + "\n\\________|\\_______|\\_______/  \\____$$ | \\______/  \\_______|   \\_/     \\_______|"
                            + "\n                             $$\\   $$ |                                        "
                            + "\n                             \\$$$$$$  |                                        "
                            + "\n                              \\______/                                         "
                            );
            Menu(nbBackup);
            
        }

        public void Menu(int nbBackup)
        {
            Console.WriteLine("\n\n=====================================================================" +
                "\n\n Choose among the following options :" +
                "\n\t 1 - Create a backup (" + nbBackup + "/5) " +
                "\n\t 2 - Use an existing backup"
                );

        }

        public void SrcDir()
        {
            Console.WriteLine("\nDrag and drop the source directory for the backup :");
        }

        public void DestDir()
        {
            Console.WriteLine("\nDrag and drop the destination directory for the backup :");
        }

        public void OutputError(string e)
        {
            Console.WriteLine(e);
        }

        public void BackupType()
        {
            Console.WriteLine("\nChoose your backup type :" +
                "\n\t 1 - Full backup" +
                "\n\t 2 - Differential Backup");
        }

        public void GetBackupName()
        {

        }

        public void ResultBackup(string src, string dst)
        {
            Console.WriteLine(src + " will be saved in : " + dst);
        }

        public void Leave()
        {
            Console.WriteLine("\n\nThanks for using our software. See you next time." +
                "\n\nType any key to end the program.");

        }

    }
}