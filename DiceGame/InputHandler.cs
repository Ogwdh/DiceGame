using System;

namespace DiceGame
{
    public class InputHandler
    {
        public static string RequestUserInput(int numOfOptions, List<Dice> dice)
        {
            string userInput;
            bool validInput = false;

            do
            {
                Console.Write("Your selection: ");
                userInput = Console.ReadLine()?.Trim().ToUpper() ?? "";

                if (userInput == "X")
                {
                    if (!ConfirmExit())
                        continue;
                }
                else if (userInput == "?")
                {
                    HelpTable.ShowHelpTable(dice);
                    continue;
                }

                validInput = IsInputValid(userInput, numOfOptions);

                if (!validInput)
                    Console.WriteLine("Please, select a suggested option from the menu.");
            }
            while (!validInput);

            return userInput;
        }

        public static bool IsInputValid(string input, int numOfOptions)
        {
            return (int.TryParse(input, out int number) && number >= 0 && number < numOfOptions);
        }

        private static bool ConfirmExit()
        {
            Console.Write("Are you sure? (y to confirm): ");
            string response = Console.ReadLine()?.Trim().ToLower() ?? "";

            if (response == "y")
                Environment.Exit(0);

            return false;
        }
    }
}
