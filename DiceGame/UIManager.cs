using System;
using System.Collections.Generic;

namespace DiceGame
{
    public class UIManager(List<Dice> dice)
    {
        private readonly List<Dice> _dice = dice;

        public void ShowMenu(int numOfOptions, bool isFirstMove)
        {
            for (int i = 0; i < numOfOptions; i++)
            {
                string option = isFirstMove ?
                    i.ToString() :
                    $"[{string.Join(", ", _dice[i].Configuration)}]";

                Console.WriteLine($"{i} - {option}");
            }
            Console.WriteLine("X - exit");
            Console.WriteLine("? - help");
        }

        public static void ShowAvailableDice(List<Dice> availableDice)
        {
            Console.WriteLine("Choose your dice:");
            for (int i = 0; i < availableDice.Count; i++)
            {
                Console.WriteLine($"{i} - [{string.Join(", ", availableDice[i].Configuration)}]");
            }
            Console.WriteLine("X - exit");
            Console.WriteLine("? - help");
        }
    }
}
