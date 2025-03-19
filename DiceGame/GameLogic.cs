using System;
using System.Collections.Generic;

namespace DiceGame
{
    internal class GameLogic(List<Dice> dice)
    {
        private readonly List<Dice> _dice = dice;
        private readonly int _diceCount = dice.Count;

        public static (string, string, int) GenerateHMAC(int range)
        {
            int randomNum = RandomGenerator.GenerateRandomNumber(range);
            (string hmacKey, string hmacValue) = RandomGenerator.ComputeHMAC(randomNum.ToString());
            return (hmacKey, hmacValue, randomNum);
        }

        public int SelectRandomDice(int excludeIndex)
        {
            int computerDiceIndex;
            do
            {
                computerDiceIndex = RandomGenerator.GenerateRandomNumber(_diceCount);
            } while (computerDiceIndex == excludeIndex);

            return computerDiceIndex;
        }

        public static int CalculateFairResult(int computerValue, int userValue, int modulo)
        {
            return (computerValue + userValue) % modulo;
        }

        public int GetDiceValue(int diceIndex, int faceIndex)
        {
            return _dice[diceIndex].Configuration[faceIndex];
        }

        public List<int> GetAvailableDiceIndices(int excludeIndex)
        {
            var indices = new List<int>();
            for (int i = 0; i < _diceCount; i++)
            {
                if (i != excludeIndex)
                {
                    indices.Add(i);
                }
            }
            return indices;
        }
    }
}
