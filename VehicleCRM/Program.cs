using System;

namespace VehicleCRM
{
    class Program
    {
        static int userInput;
        static Controller controller;

        static void Main(string[] args)
        {
            controller = new Controller();
            Menu();
            Console.ReadLine();
        }

        static public void Menu()
        {
            Console.WriteLine("Hello, please choose a number between 1-4 to generate one of the following reports...\n" +
                "1) All known customers and any vehicles they own.\n" +
                "2) All customers between the age of 20 and 30.\n" +
                "3) All vehicles registered before 1st January 2010.\n" +
                "4) All vehicles with engine size over 1000cc.\n");

            // Loops until valid input provided, then calls method in controller class
            while (!int.TryParse(Console.ReadLine(), out userInput) || (userInput > 4 || (userInput < 1)))
            {
                Console.WriteLine("Invalid input, try again.");
            }
            Console.WriteLine("\n");
            controller.HandleUserInput(userInput);

            // Loops until valid input provided, then either displays the menu again or exits.
            Console.WriteLine("\nPress '1' for the menu, or '2' to exit.");
            while (!int.TryParse(Console.ReadLine(), out userInput) || (userInput > 3 || (userInput < 1)))
            {
                Console.WriteLine("Enter a valid input.");
            }
            if (userInput == 1)
            {
                Console.Clear();
                Menu();
            }
            else
            {
                Environment.Exit(0);
            }
        }
    }
}