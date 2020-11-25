using System;

namespace EasySave
{
    class Program
    {
        //Starts main program
        static void Main(string[] args)
        {
            Model model = new Model();
            View view = new View();
            Controller ctrl = new Controller(model, view);
        }
    }
}
