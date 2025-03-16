using ConsoleTables;
using DiceGame;
using Microsoft.VisualBasic;
using Org.BouncyCastle.Crypto.Digests;
using System.Security.Cryptography;
using System.Text;
internal class Program
{
    public const int firstMoveRange = 2;
    public  static int DiceNum {get;set;}

    public static List<Dice> Dice {  get; set; }

    private static void Main(string[] args)
    {
        Dice = DiceConfiguration.GetConfiguration(args);
        DiceNum = Dice.Count;
          
        DrawLots(firstMoveRange);
    }

    private static bool DrawLots(int range)
    {
        Console.WriteLine("Let's determine who makes the first move.");
        Console.Write($"I selected a random value in the range 0..{range - 1} ");

        int randomNum = RandomGenerator.GenerateRandomNumber(range);
        //Console.WriteLine("CHECK: " + randomNum);
        (string, string) hmac = RandomGenerator.ComputeHMAC(randomNum.ToString());
        
        Console.WriteLine("(HMAC: " + hmac.Item2 + ")");


        Console.WriteLine("Try to guess my selection.");

        ShowMenu(firstMoveRange);
        ShowMenu(DiceNum);

        //Console.WriteLine("KEY: " + hmac.Item1);

        return true;
    }

    private static void ShowMenu(int numOfOptions)
    {
        Enumerable.Range(0, numOfOptions).ToList().ForEach(i =>
    Console.WriteLine($"{i} - " +
        ((numOfOptions == firstMoveRange) ?
            i.ToString() :
            $"[{string.Join(", ", Dice[i].Configuration)}]")));
        Console.WriteLine("X - exit");
        Console.WriteLine("? - help");
    }

    
}

// TODO: input dice ✅
// TODO: define first move (random generation + HMAC) 
// TODO: dice selection
// TODO: roll the dice (random generation + HMAC)
// TODO: calculate the result as (x + y) % 6
// TODO: invalid parameters launch


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
//The implementation of the fair number generation "protocol" should be in a separate class.
//The random key/number generation and HMAC calculation should be in a separate class.
//The dice configuration parsing should be in a separate class. ✅
//The dice abstraction should be in a separate class. ✅

//You should use the core class libraries and third-party libraries to the maximum, and not reinvent the wheel. 

//THE NUMBER OF DICE CAN BE ARBITRARY ( > 2).