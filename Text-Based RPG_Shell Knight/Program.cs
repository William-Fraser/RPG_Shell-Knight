using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    class Program
    {
        static bool turnOffCursor = false;
        static void Main(string[] args)
        {

            Console.CursorVisible = false; 
            Player player = new Player("John Smith", '@');
            MapMANAGE MapMAN = new MapMANAGE();
            
            while (true)
            {
                Console.Clear();
                MapMAN.DrawWindowBorder();
                player.Draw();
                player.Update();
            }
        }
    }
}
