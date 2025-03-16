using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceGame
{
    internal class DiceConfiguration()
    {
        public static List<Dice> GetConfiguration(string[] arg)
        {
            var diceList = arg.Select(arg =>
                new Dice(arg.Split(',')
                            .Select(int.Parse)
                            .ToArray()))
                .ToList();

            return (diceList.Count > 2) ? diceList : InquireCorrectInput();
        }

        private static List<Dice> InquireCorrectInput()
        {
            List<Dice> diceList;

            do
            {
                Console.WriteLine("Please enter valid number of dice > 2 (each separated with space, comma-separated numbers for sides):");
                diceList = Console.ReadLine().Split(' ')
                    .Select(dice => new Dice(dice.Split(',')
                    .Select(int.Parse).ToArray()))
                    .ToList();

                if (diceList.Count <= 2)
                {
                    Console.WriteLine("The number of dice than 2. Please try again.");

                }

            } while (diceList.Count <= 2);

            return diceList;
        }
    }
}
