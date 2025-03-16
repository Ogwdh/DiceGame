using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceGame
{
    internal class Dice(int[] sides)
    {
        public int[] Configuration { get; set; } = new int[sides.Length];
    }
}
