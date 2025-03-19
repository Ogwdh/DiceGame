using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceGame
{
    public class Dice(int[] sides)
    {
        public int[] Configuration { get; set; } = sides;
    }
}
