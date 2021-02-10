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

            Console.CursorVisible = turnOffCursor; 
            MapMANAGE MapMAN = new MapMANAGE();
            Player player = new Player("John Smith", '@');
            
            MapMAN.CreateWindowBorder();
            
            while (true)
            {
                MapMAN.DrawWindowBorder();
                player.Draw();
                player.Update();
                player.CheckForWall(MapMAN.isWall(player.getAxisY(), player.getAxisX()));
            }
        }
    }
}
