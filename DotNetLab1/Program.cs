using System;
using System.Collections.Generic;
using System.Text;
using ClassLibrary;

namespace DotNetLab1
{
    internal class Program
    {
        private static UserMenu userInterface;

        static void Main(string[] args)
        {
            userInterface = new UserMenu();
            userInterface.ConfigureConsole();
            userInterface.InitializeBank();
            userInterface.RunMainMenu();
        }
    }
}