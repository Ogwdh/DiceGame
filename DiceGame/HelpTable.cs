using ConsoleTables;
using System;
using System.Collections.Generic;

namespace DiceGame
{
    internal static class HelpTable
    {
        public static void ShowHelpTable(List<Dice> dice)
        {
            var calculator = new ProbabilitiyCalculator();

            var probabilityData = calculator.CalculateProbabilityData(dice);

            Console.WriteLine("Probability of the win for the user:");

            var headers = new List<string> { "User dice v" };
            for (int i = 0; i < dice.Count; i++)
            {
                headers.Add($"[{string.Join(",", dice[i].Configuration)}]");
            }

            var table = new ConsoleTable(headers.ToArray());

            for (int i = 0; i < dice.Count; i++)
            {
                var rowData = new List<string>
                {
                    $"[{string.Join(",", dice[i].Configuration)}]"
                };

                for (int j = 0; j < dice.Count; j++)
                {
                    if (i == j)
                    {
                        rowData.Add("- (0.3333)");
                    }
                    else
                    {
                        double probability = probabilityData[(i, j)];
                        rowData.Add($"{probability:F4}");
                    }
                }

                table.AddRow(rowData.ToArray());
            }

            table.Write(Format.Alternative);
            Console.WriteLine();
        }
    }
}
