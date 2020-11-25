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

        //Shows when the app starts
        public void Start(int nbBackup, int maxBackup)
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
            Menu(nbBackup, maxBackup);
            
        }

        //Follows Start() method
        public void Menu(int nbBackup, int maxBackup)
        {
            Console.WriteLine("\n\n=====================================================================" +
                "\n\n Choose among the following options :" +
                "\n\t 1 - Create a backup work (" + nbBackup + "/"+ maxBackup +")" +
                "\n\t 2 - Use an existing backup"
                );

        }

        //
        // The following methods are used when program needs to ask user input. Names are explicit.
        //

        //Source directory
        public void SrcDir()
        {
            Console.WriteLine("\nDrag and drop the source directory for the backup :");
        }

        //Destination directory
        public void DestDir()
        {
            Console.WriteLine("\nDrag and drop the destination directory for the backup :");
        }

        //When an error pops up
        public void OutputError(string e)
        {
            Console.WriteLine(e);
        }

        //When user selects what type of save he wants
        public void BackupType()
        {
            Console.WriteLine("\nChoose your backup type :" +
                "\n\t 1 - Full backup" +
                "\n\t 2 - Differential backup");
        }

        //The name for users' backup work
        public void GetBackupName()
        {
            Console.WriteLine("\nEnter backup name : ");

        }

        //Backup info check
        public void Confirm(string src, string dst, string name)
        {
            Console.WriteLine("\n" + src + " will be saved in : " + dst
                + " as " + name);
        }

        //When user is leaving
        public void Leave()
        {
            Console.WriteLine("\n\nThanks for using our software. See you next time." +
                "\n\nType any key to end the program.");

        }

    }
}