using System;
using System.Collections.Generic;

namespace DiceGame
{
    internal class ProbabilitiyCalculator
    {
        public Dictionary<(int, int), double> CalculateProbabilityData(List<Dice> dice)
        {
            var probabilityData = new Dictionary<(int, int), double>();

            for (int i = 0; i < dice.Count; i++)
            {
                for (int j = 0; j < dice.Count; j++)
                {
                    if (i == j)
                    {
                        probabilityData.Add((i, j), 0.3333);
                    }
                    else
                    {
                        double probability = CalculateWinProbability(dice[i], dice[j]);
                        probabilityData.Add((i, j), probability);
                    }
                }
            }

            return probabilityData;
        }
        private double CalculateWinProbability(Dice dice1, Dice dice2)
        {
            int wins = 0;
            int totalOutcomes = dice1.Configuration.Length * dice2.Configuration.Length;

            for (int i = 0; i < dice1.Configuration.Length; i++)
            {
                for (int j = 0; j < dice2.Configuration.Length; j++)
                {
                    if (dice1.Configuration[i] > dice2.Configuration[j])
                    {
                        wins++;
                    }
                }
            }

            return (double)wins / totalOutcomes;
        }
    }
}
