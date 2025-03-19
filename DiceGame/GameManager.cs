using System;
using System.Collections.Generic;

namespace DiceGame
{
    public class GameManager
    {
        public const int FirstMoveRange = 2;
        public int DiceNum { get; private set; }
        public List<Dice> Dice { get; private set; }
        private readonly DiceRoller _diceRoller;
        private readonly UIManager _uiManager;

        public GameManager(string[] args)
        {
            Dice = DiceConfiguration.GetConfiguration(args);
            DiceNum = Dice.Count;

            if (DiceNum < 2)
            {
                throw new ArgumentException("At least 2 dice configurations are required.");
            }

            _diceRoller = new DiceRoller(Dice);
            _uiManager = new UIManager(Dice);
        }

        public void StartGame()
        {
            int firstMovePlayer = DetermineFirstMove();
            int playerDiceIndex = SelectDice(firstMovePlayer);
            DetermineWinner(playerDiceIndex, firstMovePlayer);
        }

        private int DetermineFirstMove()
        {
            Console.WriteLine("Let's determine who makes the first move.");

            int randomValue = RandomGenerator.GenerateRandomNumber(FirstMoveRange);
            var (hmacKey, hmacValue) = RandomGenerator.ComputeHMAC(randomValue.ToString());

            Console.WriteLine($"I selected a random value in the range 0..{FirstMoveRange - 1} (HMAC: {hmacValue})");
            Console.WriteLine("Try to guess my selection.");

            _uiManager.ShowMenu(FirstMoveRange, isFirstMove: true);

            string userInput = InputHandler.RequestUserInput(FirstMoveRange, Dice);

            int userGuess = int.Parse(userInput);
            Console.WriteLine($"My selection: {randomValue} (KEY: {hmacKey})");

            if (userGuess == randomValue)
            {
                Console.WriteLine("You guessed correctly! ");
                return 0;
            }
            else
            {
                Console.WriteLine("Your guess was incorrect. ");
                return 1;
            }
        }

        private int SelectDice(int firstMovePlayer)
        {
            int computerDiceIndex;
            int playerDiceIndex;

            if (firstMovePlayer == 0)
            {
                Console.WriteLine("You make the first move.");
                _uiManager.ShowMenu(DiceNum, isFirstMove: false);
                string userInput = InputHandler.RequestUserInput(DiceNum, Dice);
                playerDiceIndex = int.Parse(userInput);

                computerDiceIndex = SelectRandomDice(playerDiceIndex);
                Console.WriteLine($"I choose the [{string.Join(", ", Dice[computerDiceIndex].Configuration)}] dice.");
            }
            else
            {
                computerDiceIndex = RandomGenerator.GenerateRandomNumber(DiceNum);
                Console.WriteLine($"I make the first move and choose the [{string.Join(", ", Dice[computerDiceIndex].Configuration)}] dice.");

                var availableDice = new List<Dice>();
                var availableIndices = new List<int>();

                for (int i = 0; i < DiceNum; i++)
                {
                    if (i != computerDiceIndex)
                    {
                        availableDice.Add(Dice[i]);
                        availableIndices.Add(i);
                    }
                }

                UIManager.ShowAvailableDice(availableDice);

                string userInput = InputHandler.RequestUserInput(availableDice.Count, Dice);

                int selection = int.Parse(userInput);
                playerDiceIndex = availableIndices[selection];

                Console.WriteLine($"You choose the [{string.Join(", ", Dice[playerDiceIndex].Configuration)}] dice.");
            }

            return playerDiceIndex;
        }

        private void DetermineWinner(int playerDiceIndex, int firstMovePlayer)
        {
            int computerRoll, playerRoll;

            if (firstMovePlayer == 0)
            {
                playerRoll = _diceRoller.RollDice(playerDiceIndex, isComputer: false);
                computerRoll = _diceRoller.RollDice(SelectRandomDice(playerDiceIndex), isComputer: true);
            }
            else
            {
                computerRoll = _diceRoller.RollDice(SelectRandomDice(playerDiceIndex), isComputer: true);
                playerRoll = _diceRoller.RollDice(playerDiceIndex, isComputer: false);
            }

            if (playerRoll > computerRoll)
            {
                Console.WriteLine($"You win ({playerRoll} > {computerRoll})!");
            }
            else if (computerRoll > playerRoll)
            {
                Console.WriteLine($"I win ({computerRoll} > {playerRoll})!");
            }
            else
            {
                Console.WriteLine($"It's a tie ({playerRoll} = {computerRoll})!");
            }
        }

        private int SelectRandomDice(int playerDiceIndex)
        {
            int computerDiceIndex;
            do
            {
                computerDiceIndex = RandomGenerator.GenerateRandomNumber(DiceNum);
            } while (computerDiceIndex == playerDiceIndex);

            return computerDiceIndex;
        }
    }
}
