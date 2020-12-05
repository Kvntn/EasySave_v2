using System;

namespace EasySave
{
    class Program
    {
        //Starts main program
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome / Bienvenue");
                
        missSpelled:

            Console.WriteLine("\n\nSelect a language /Choisissez une langue" +
                "\n\ntype EN for English / Tapez FR pour francais");

            string tellMeYourLanguage = Console.ReadLine();
            if (tellMeYourLanguage == "EN")
            {
                Console.Clear();
                new Controller(new Model(), new View()); 
            }

            else if (tellMeYourLanguage == "FR")
            {
                Console.Clear();
                new Controller(new Model(), new View_FR());
            }

            else
            {
                Console.Clear();
                Console.WriteLine("\n\nYou wrote something wrong / Vous avez mal saisi");
                goto missSpelled;
            }

        }
    }
}
