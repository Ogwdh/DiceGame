using System;
using System.Collections.Generic;

namespace DiceGame
{
    public class DiceRoller
    {
        private readonly List<Dice> _dice;
        private readonly UIManager _uiManager;
        private const int DiceSides = 6;

        public DiceRoller(List<Dice> dice)
        {
            _dice = dice;
            _uiManager = new UIManager(dice);
        }

        public int RollDice(int diceIndex, bool isComputer)
        {
            string actor = isComputer ? "my" : "your";
            Console.WriteLine($"It's time for {actor} roll.");

            int randomValue = RandomGenerator.GenerateRandomNumber(DiceSides);
            var (hmacKey, hmacValue) = RandomGenerator.ComputeHMAC(randomValue.ToString());

            Console.WriteLine($"I selected a random value in the range 0..5 (HMAC: {hmacValue})");
            Console.WriteLine("Add your number modulo 6.");

            _uiManager.ShowMenu(DiceSides, isFirstMove: true);

            string userInput = InputHandler.RequestUserInput(DiceSides, _dice);

            int userNumber = int.Parse(userInput);
            Console.WriteLine($"My number is {randomValue} (KEY: {hmacKey})");

            int fairResult = RandomGenerator.CalculateResult(userNumber, randomValue, DiceSides);
            Console.WriteLine($"The fair number generation result is {randomValue} + {userNumber} = {fairResult} (mod 6)");

            int diceValue = _dice[diceIndex].Configuration[fairResult];
            Console.WriteLine($"{(isComputer ? "My" : "Your")} roll result is {diceValue}.");

            return diceValue;
        }
    }
}
