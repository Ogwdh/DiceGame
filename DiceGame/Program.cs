using ConsoleTables;
using DiceGame;
using Microsoft.VisualBasic;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Macs;
using System.Security.Cryptography;
using System.Text;
using System;
using System.Linq;

internal class Program
{
    public const int FirstMoveRange = 2;
    public static int DiceNum { get; private set; }
    public static List<Dice> Dice { get; private set; }

    private static void Main(string[] args)
    {
        //try
        {
            Dice = DiceConfiguration.GetConfiguration(args);
            DiceNum = Dice.Count;

            if (DiceNum < 2)
            {
                Console.WriteLine("Error: At least 2 dice configurations are required.");
                Environment.Exit(1);
            }

            Play();
        }
        //catch (Exception ex)
        //{
        //    Console.WriteLine($"Error: {ex.Message}");
        //    Environment.Exit(1);
        //}
    }

    private static void Play()
    {
        int firstMovePlayer = DetermineFirstMove();
        int playerDiceIndex = SelectDice(firstMovePlayer);
        DetermineWinner(playerDiceIndex, firstMovePlayer);
    }

    private static int DetermineFirstMove()
    {
        Console.WriteLine("Let's determine who makes the first move.");

        var (hmacKey, hmacValue, randomValue) = GenerateHMAC(FirstMoveRange);

        Console.WriteLine($"I selected a random value in the range 0..{FirstMoveRange - 1} (HMAC: {hmacValue})");
        Console.WriteLine("Try to guess my selection.");

        ShowMenu(FirstMoveRange, isFirstMove: true);

        string userInput = RequestUserInput(FirstMoveRange);
        ProcessUserChoice(userInput);

        int userGuess = int.Parse(userInput);
        Console.WriteLine($"My selection: {randomValue} (KEY: {hmacKey})");

        if (userGuess == randomValue)
        {
            Console.WriteLine("You guessed correctly! You make the first move.");
            return 0;
        }
        else
        {
            Console.WriteLine("Your guess was incorrect. I make the first move.");
            return 1;
        }
    }

    private static int SelectDice(int firstMovePlayer)
    {
        int computerDiceIndex;
        int playerDiceIndex;

        if (firstMovePlayer == 0)
        {
            Console.WriteLine("You make the first move.");
            ShowMenu(DiceNum, isFirstMove: false);
            string userInput = RequestUserInput(DiceNum);
            ProcessUserChoice(userInput);
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

            Console.WriteLine("Choose your dice:");
            for (int i = 0; i < availableDice.Count; i++)
            {
                Console.WriteLine($"{i} - [{string.Join(", ", availableDice[i].Configuration)}]");
            }
            Console.WriteLine("X - exit");
            Console.WriteLine("? - help");

            string userInput = RequestUserInput(availableDice.Count);
            ProcessUserChoice(userInput);

            int selection = int.Parse(userInput);
            playerDiceIndex = availableIndices[selection];

            Console.WriteLine($"You choose the [{string.Join(", ", Dice[playerDiceIndex].Configuration)}] dice.");
        }

        return playerDiceIndex;
    }



    private static void DetermineWinner(int playerDiceIndex, int firstMovePlayer)
    {
        int computerRoll, playerRoll;

        if (firstMovePlayer == 0)
        {
            playerRoll = RollDice(false);
            computerRoll = RollDice(true);
        }
        else
        {
            computerRoll = RollDice(true);
            playerRoll = RollDice(false);
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

    private static int RollDice(bool isComputer)
    {
        string actor = isComputer ? "my" : "your";
        Console.WriteLine($"It's time for {actor} roll.");

        var (hmacKey, hmacValue, randomValue) = GenerateHMAC(6);

        Console.WriteLine($"I selected a random value in the range 0..5 (HMAC: {hmacValue})");
        Console.WriteLine("Add your number modulo 6.");

        ShowMenu(6, isFirstMove: true);

        string userInput = RequestUserInput(6);
        ProcessUserChoice(userInput);

        int userNumber = int.Parse(userInput);
        Console.WriteLine($"My number is {randomValue} (KEY: {hmacKey})");

        int fairResult = (randomValue + userNumber) % 6;
        Console.WriteLine($"The fair number generation result is {randomValue} + {userNumber} = {fairResult} (mod 6)");

        int diceValue = isComputer ?
            Dice[0].Configuration[fairResult] :
            Dice[1].Configuration[fairResult];

        Console.WriteLine($"{(isComputer ? "My" : "Your")} roll result is {diceValue}.");

        return diceValue;
    }

    private static int SelectRandomDice(int playerDiceIndex)
    {
        int computerDiceIndex;
        do
        {
            computerDiceIndex = RandomGenerator.GenerateRandomNumber(DiceNum);
        } while (computerDiceIndex == playerDiceIndex);

        return computerDiceIndex;
    }

    private static (string, string, int) GenerateHMAC(int range)
    {
        int randomNum = RandomGenerator.GenerateRandomNumber(range);
        (string hmacKey, string hmacValue) = RandomGenerator.ComputeHMAC(randomNum.ToString());
        return (hmacKey, hmacValue, randomNum);
    }

    private static void ShowMenu(int numOfOptions, bool isFirstMove)
    {
        for (int i = 0; i < numOfOptions; i++)
        {
            string option = isFirstMove ?
                i.ToString() :
                $"[{string.Join(", ", Dice[i].Configuration)}]";

            Console.WriteLine($"{i} - {option}");
        }
        Console.WriteLine("X - exit");
        Console.WriteLine("? - help");
    }

    private static string RequestUserInput(int numOfOptions)
    {
        string userInput;
        do
        {
            Console.Write("Your selection: ");
            userInput = Console.ReadLine()?.Trim().ToUpper() ?? "";
        }
        while (!IsInputValid(userInput, numOfOptions));

        return userInput;
    }

    private static bool IsInputValid(string input, int numOfOptions)
    {
        if (input == "X" || input == "?")
            return true;

        if (int.TryParse(input, out int number) && number >= 0 && number < numOfOptions)
            return true;

        Console.WriteLine("Please, select a suggested option from the menu.");
        return false;
    }

    private static void ProcessUserChoice(string input)
    {
        if (string.IsNullOrEmpty(input))
            return;

        if (input == "?")
            HelpTable.ShowHelpTable();
        else if (input == "X")
        {
            if (!ConfirmExit())
            {
                return;
            }
        }
    }

    private static bool ConfirmExit()
    {
        Console.Write("Are you sure? (y/n): ");
        string response = Console.ReadLine()?.Trim().ToLower() ?? "";

        if (response == "y")
            Environment.Exit(0);
        return false;
    }

}

//> java - jar game.jar 2,2,4,4,9,9 6,8,1,1,8,6 7,5,3,7,5,3
//Let's determine who makes the first move.
//I selected a random value in the range 0..1 (HMAC=C8E79615E637E6B14DDACA2309069A76D0882A4DD8102D9DEAD3FD6AC4AE289A).
//Try to guess my selection.
//0 - 0
//1 - 1
//X - exit
//? - help
//Your selection: 0
//My selection: 1(KEY = BD9BE48334BB9C5EC263953DA54727F707E95544739FCE7359C267E734E380A2).
//I make the first move and choose the [6,8,1,1,8,6] dice.
//Choose your dice:
//0 - 2,2,4,4,9,9
//1 - 7,5,3,7,5,3
//X - exit
//? -help
//Your selection: 0
//You choose the [2,2,4,4,9,9] dice.
//It's time for my roll.
//I selected a random value in the range 0..5 (HMAC=AA29E7275FE17A8D1184E2D4B6B0F46D815224270C94907CF007F2118CF400F7).
//Add your number modulo 6.
//0 - 0
//1 - 1
//2 - 2
//3 - 3
//4 - 4
//5 - 5
//X - exit
//? - help
//Your selection: 4
//My number is 3 (KEY=7329ABD54A1633D2079EA7A48B401018D7EE6DD4C130AB5C31BC029CC8359637).
//The fair number generation result is 3 + 4 = 1 (mod 6).
//My roll result is 8.
//It's time for your roll.
//I selected a random value in the range 0..5 (HMAC=652863C27870CCA331458F4658D89413F405736FE5AA19B868FBDDAB5611A406).
//Add your number modulo 6.
//0 - 0
//1 - 1
//2 - 2
//3 - 3
//4 - 4
//5 - 5
//X - exit
//? - help
//Your selection: 5
//My number is 0 (KEY=92564A82A515DEBC3FE9842D20DCEA3F3AAFB2080314A09A1E9A2CC729EDAF44).
//The fair number generation result is 0 + 5 = 5 (mod 6).
//Your roll result is 9.
//You win (9 > 8)!

//The table generation should be in a separate class.
//The probability calculation should be in a separate class.
