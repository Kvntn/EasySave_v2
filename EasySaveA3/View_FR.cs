using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace EasySave
{
    public class View_FR
    {
        public View_FR()
        {

        }

        //Shows when the app starts
        public void Start(int nbBackup, int maxBackup)
        {
            Console.WriteLine("\n   $$$$$$$$\\                               $$$$$$\\                                          "
                            + "\n   $$  _____ |                            $$  __$$\\                                          "
                            + "\n   $$ |      $$$$$$\\   $$$$$$$\\ $$\\   $$\\ $$ /  \\__ | $$$$$$\\ $$\\    $$\\  $$$$$$\\    "
                            + "\n   $$$$$\\    \\____$$\\ $$  _____ |$$ |  $$ |\\$$$$$$\\   \\____$$\\\\$$\\  $$  |$$  __$$\\  "
                            + "\n   $$  __ |   $$$$$$$ |\\$$$$$$\\  $$ |  $$ | \\____$$\\  $$$$$$$ |\\$$\\$$  / $$$$$$$$ |     "
                            + "\n   $$ |     $$  __$$ | \\____$$\\ $$ |  $$ |$$\\   $$ |$$  __$$ | \\$$$  /  $$   ____ |       "
                            + "\n   $$$$$$$$\\\\$$$$$$$ |$$$$$$$  |\\$$$$$$$ |\\$$$$$$  |\\$$$$$$$ |  \\$  /   \\$$$$$$$\\     "
                            + "\n   \\________|\\_______|\\_______/  \\____$$ | \\______/  \\_______|   \\_/     \\_______|    "
                            + "\n                                  $$\\   $$ |                                                 "
                            + "\n                                  \\$$$$$$  |                                                 "
                            + "\n                                   \\______/                                                  "
                            );
            Menu(nbBackup, maxBackup);

        }

        //Follows Start() method
        public void Menu(int nbBackup, int maxBackup)
        {
            Console.WriteLine("\n\n=====================================================================" +
                "\n\n Choisissez parmis les options suivantes :" +
                "\n\t 1 - Creer un travail de sauvegarde (" + nbBackup + "/" + maxBackup + ")" +
                "\n\t 2 - Utiliser une sauvegarde existante"
                );

        }

        //
        // The following methods are used when program needs to ask user input. Names are explicit.
        //

        //Source directory
        public void SrcDir()
        {
            Console.WriteLine("\nGlissez-déposez le dossier source de la sauvegarde :");
        }

        //Destination directory
        public void DestDir()
        {
            Console.WriteLine("\nnGlissez-déposez le dossier destinataire de la sauvegarde :");
        }

        //When an error pops up
        public void OutputError(string e)
        {
            Console.WriteLine(e);
        }

        //When user selects what type of save he wants
        public void BackupType()
        {
            Console.WriteLine("\nChoisissez votre type de sauvegarde :" +
                "\n\t 1 - Sauvegarde complete" +
                "\n\t 2 - Sauvegarde differentielle");
        }

        //The name for users' backup work
        public void GetBackupName()
        {
            Console.WriteLine("\nEntrez un nom pour la sauvegarde : ");

        }

        //Backup info check
        public void Confirm(string src, string dst, string name)
        {
            Console.WriteLine("\n" + src + " sera sauvegarde dans : " + dst
                + " en tant que " + name);
        }

        //When user is leaving
        public void Leave()
        {
            Console.WriteLine("\n\nMerci d'avoir utilisé notre logiciel, au revoir" +
                "\n\nPressez une touche pour quitter.");

        }

    }
}