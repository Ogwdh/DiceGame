using DiceGame;
using System;
using System.Collections.Generic;

internal class Program
{
    private static void Main(string[] args)
    {
        try
        {
            var gameManager = new GameManager(args);
            gameManager.StartGame();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            Environment.Exit(1);
        }
    }
}