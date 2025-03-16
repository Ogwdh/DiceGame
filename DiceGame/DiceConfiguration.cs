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
                Console.WriteLine("Please enter valid number of dice ( > 2 ) with valid number of sides ( 6 ) " +
                    "(each die separated with space, comma-separated numbers for sides):");
                diceList = ReadDiceInput();
            } while (!IsValidDiceCount(diceList) || !HasValidSides(diceList));

            return diceList;
        }

        private static List<Dice> ReadDiceInput()
        {
            return Console.ReadLine().TrimEnd().Split(' ')
                .Select(dice => new Dice(dice.Split(',')
                .Select(int.Parse).ToArray()))
                .ToList();
        }

        private static bool IsValidDiceCount(List<Dice> diceList)
        {
            if (diceList.Count <= 2)
            {
                Console.WriteLine("The number of dice must be greater than 2. Please try again.");
                return false;
            }
            return true;
        }

        private static bool HasValidSides(List<Dice> diceList)
        {
            if (!diceList.All(dice => dice.Configuration.Length == 6))
            {
                Console.WriteLine("One or more dice have an invalid number of sides (must be 6).");
                return false;
            }
            return true;
        }
    }
}
