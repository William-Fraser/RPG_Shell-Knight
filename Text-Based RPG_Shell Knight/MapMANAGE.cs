using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    class MapMANAGE
    {
        private Char[] borderPieces
        public void DrawWindowBorder() // ═ ║
        {
            Console.SetCursorPosition(0, 0);
            Console.Write("╔");
            for (int i = 1; i < Console.WindowWidth - 1; i++ )
            {
                Console.Write("═");
            }
            Console.Write("╗");
            for (int i = 1; i < Console.WindowHeight - 2; i++)
            {
                Console.Write("║");
                for (int j = 1; j < Console.WindowWidth - 1; j++)
                {
                    Console.Write(" ");
                }
                Console.Write("║");
            }
            Console.Write("╚");
            for (int i = 1; i < Console.WindowWidth - 1; i++)
            {
                Console.Write("═");
            }
            Console.Write("╝");

        }
    }
}
