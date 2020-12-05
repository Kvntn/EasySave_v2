using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EasySave
{
    class Controller
    {
        //attributes for controller 
        private Model model;
        private dynamic view;

        private bool FR = false;

        public Controller(Model modelx, View viewx)
        {
            model = modelx;
            view = viewx;
            AppStart();
        }

        public Controller(Model modelx, View_FR viewx)
        {
            FR = true;
            model = modelx;
            view = viewx;
            AppStart();
        }

        //starts the program, this method is called in Main(string[] args)
        private void AppStart()
        {
            bool stop = false;
            int choice;
            do
            {
                BackupDirectory.BackupsList = model.LoadList();
                view.Start(model.backupCount, 5);
                choice = UserChoice();

                if (choice == 1 && model.AllowedBackup) // create a backup using this option
                {
                    view.BackupType();
                    model.BackupType = GetBackupType();

                    view.SrcDir();
                    model.SrcDir = GetSrcDir();

                    view.DestDir();
                    model.DestDir = GetDestDir();

                    view.GetBackupName();
                    model.BackupName = GetBackupName();

                    view.Confirm(model.SrcDir, model.DestDir, model.BackupName);
                    this.Confirm();

                    BackupDirectory.CreateBackup(new DirectoryInfo(model.SrcDir), new DirectoryInfo(model.DestDir), model.BackupName, model.BackupType);

                }

                else if (choice == 1 && model.backupCount >= 5) // can't create more backups
                    if (FR)
                        Console.WriteLine("\nVous avez atteint le nombre limite de sauvegardes (" 
                            + 5 + ")");
                    else
                        Console.WriteLine("\nYou have reached maximum quantity of different backups ("
                            + 5 + ")");

                else if (choice == 2) // use a backup work
                {
                    view.BackupType();
                    model.BackupType = GetBackupType();

                    BackupDirectory.ExistingBackup(model.BackupType);
                }


                stop = AppEnd();

            } while (!stop);

            view.Leave();
            Console.ReadKey();
            Environment.Exit(1); // ONLY FOR CONSOLE APP
                                 // USE : (System.) Windows.Forms.Application.Exit(); FOR WINFORM
        }

        //returns the choice of the user (use or create backup)
        public int UserChoice() 
        {
            int opt;
            bool isAllowed = false;

            do
            {
                string input = Console.ReadLine();
                if(int.TryParse(input, out opt))
                {
                    if (opt == 1 || opt == 2)
                    {
                        isAllowed = true;
                    }
                    else
                    {
                        if (FR)
                            Console.WriteLine("\nVous avez mal ecris.");
                        else
                            Console.WriteLine("\nYou wrote something wrong.");
                        isAllowed = false;
                    }

                }
            } while (!isAllowed);

            return opt;

        }

        //returns boolean that confirm or not when user is leaving the program
        public bool AppEnd()
        {

            bool end = false;
            bool isCorrect = false;

            do
            {
                if (FR)
                    Console.WriteLine("\nVoulez-vous continuer ? " +
                    "\n\t Y - Oui" +
                    "\n\t N - Non");
                else
                    Console.WriteLine("\nDo you want to continue ? " +
                    "\n\t Y - Yes" +
                    "\n\t N - No");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "Y":
                        Console.Clear();
                        end = false;
                        isCorrect = true;
                        break;
                    case "N":
                        end = true;
                        isCorrect = true;
                        break;
                    default:
                        if (FR)
                            Console.WriteLine("\nVous avez mal ecris.");
                        else
                            Console.WriteLine("\nYou wrote something wrong.");
                        end = false;
                        break;
                   
                }
            } while (!isCorrect);

            return end;
        }
       
        //Confirmation for source, destination and backup name by the user
        public bool Confirm()
        {
         
            bool end, isCorrect = false;

            do
            {
                if (FR)
                    Console.WriteLine("\nVous confirmez ?");
                else
                    Console.WriteLine("\nDo you confirm ?");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "Y":
                        
                        end = false;
                        isCorrect = true;
                        break;
                    case "N":
                        end = true;
                        isCorrect = true;
                        break;
                    default:
                        if (FR)
                            Console.WriteLine("\nVous avez mal ecris.");
                        else
                            Console.WriteLine("\nYou wrote something wrong.");
                        end = false;
                        break;

                }
            } while (!isCorrect);

            Console.Clear();
            return end;

        }

        // Basic getters

        private string GetSrcDir()
        {
            string dir = Console.ReadLine();
            return dir;
        }

        private string GetDestDir()
        {
            string dir = Console.ReadLine();
            return dir;
        }

        private int GetBackupType()
        {
            string type = Console.ReadLine();
            int.TryParse(type, out int nb);
                
            if (nb != 1 && nb != 2)
                nb = GetBackupType();

            return nb;
        }

        private string GetBackupName()
        {
            string name = Console.ReadLine();
            return name;
        }

 
    }
}
