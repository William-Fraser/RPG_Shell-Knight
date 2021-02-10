using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    class MapMANAGE
    {
        public char[] _walls = new char[] { '═', '║' };
        public string[,] borderMap = new string[consoleHeight, consoleWidth];

        private string borderString = "";
        private static int consoleWidth = 120;
        private static int consoleHeight = 40;
        private string[,] borderSavedMap = new string [consoleHeight, consoleWidth];

        // ----- Manager tools
        public bool isWall(int y, int x ) {
            Console.SetCursorPosition(Console.WindowHeight, 1);
            for (int i = 0; i < _walls.Length; i++) {
                if (borderMap[y, x] == _walls[i].ToString()) {
                    return true;
                }
                else { } // do nothing
            }
            return false;
        }

        // ----- Manager Builders
        public void CreateWindowBorder() // walls ═ ║ 
        {

            Console.SetCursorPosition(0, 0);
            borderString += "╔,"; //Console.Write("╔"); 
            for (int i = 1; i < Console.WindowWidth - 1; i++)
            {
                borderString += "═,"; //Console.Write("═"); 
            }
            borderString += "╗;"; //Console.Write("╗"); 
            for (int i = 1; i < Console.WindowHeight - 2; i++)
            {
                borderString += "║,"; //Console.Write("║"); 
                for (int j = 1; j < Console.WindowWidth - 1; j++)
                {
                    borderString += " ,"; //Console.Write(" "); 
                }
                borderString += "║;"; //Console.Write("║"); 
            }
            borderString += "╚,"; //Console.Write("╚"); 
            for (int i = 1; i < Console.WindowWidth - 1; i++)
            {
                borderString += "═,"; //Console.Write("═"); 
            }
            borderString += "╝;"; //Console.Write("╝");

            string[] MapY = borderString.Split(';');
            
            for (int i = 0; i < MapY.Length; i++) 
            {
                string[] MapX = MapY[i].Split(','); // used for .Length and to Map the Xcoordinates on 
                for (int j = 0; j < MapX.Length; j++)
                {
                    borderMap[i, j] = MapX[j];
                    borderSavedMap[i, j] = MapX[j];
                }
            }
        }
        
        public void DrawWindowBorder() 
        {
            string[] MapY = borderString.Split(';');
            string[] MapXLength = MapY[0].Split(','); //used for loop length

            //Console.WriteLine($"x: {MapXLength.Length} y: {MapY.Length}");   //debug
            //Console.WriteLine($"Map 0,0: [{borderMap[0, 0]}]"); // --- /debug/


            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < MapY.Length; i++) // nested loop to write window border
            {
                for (int j = 0; j < MapXLength.Length; j++)
                {
                    Console.Write(borderMap[i, j]);
                    
                }
            }
            Console.SetCursorPosition(1, 1);
        }


    }
}
