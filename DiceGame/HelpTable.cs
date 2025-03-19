using ConsoleTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceGame
{
    internal class HelpTable
    {
        public ConsoleTable Table { get; set; }

        public HelpTable() { }

        public static void ShowHelpTable()
        {
            Console.WriteLine("TABLE PRINT");
        }
    }
}
