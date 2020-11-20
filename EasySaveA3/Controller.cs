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
        private string BackupName;

        public Controller(Model modelx, View viewx)
        {
            model = modelx;
            view = viewx;
            AppStart();
        }

        public void AppStart()
        {
            bool stop = false;
            int choice;
            do
            {
                view.Start(model.list.index);
                choice = UserChoice();
                //verif model.list.Length < 5 (créer une liste dans le model (json.deserialize ?))

                if (choice == 1 && model.list.index < 5)
                {
                    view.SrcDir();
                    model.SrcDir = GetSrcDir();
                    view.DestDir();
                    model.DestDir = GetDestDir();

                }
                else if (choice == 1 && model.list.index >= 5)
                    Console.WriteLine("\nYou have reached maximum quantity of different backups");

                stop = AppEnd();

            } while (!stop);

            Console.ReadKey();
            Environment.Exit(1); // ONLY FOR CONSOLE APP
                                 // USE : (System.) Windows.Forms.Application.Exit(); FOR WINFORM

        }

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
                        Console.WriteLine("You wrote something wrong");
                        isAllowed = false;
                    }

                }
            } while (!isAllowed);

            return opt;

        }

        public bool AppEnd()
        {

            bool end = false;
            bool isCorrect = false;

            do
            {
                Console.WriteLine("Do you want to continue ? " +
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
                        view.Leave();
                        break;
                    default:
                        Console.WriteLine("You wrote something wrong");
                        end = false;
                        break;
                   
                }
            } while (!isCorrect);

            return end;
        }

        public bool CheckDirExistence()
        {
            return true;
        }

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

        private int GetSaveType()
        {
            string type = Console.ReadLine();
            int.TryParse(type, out int nb);
            return nb;
        }

        private string GetBackupName()
        {
            string name = Console.ReadLine();
            return name;
        }


    }
}
