using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    class MapMANAGE
    {
        public char[] _walls = new char[] { '═', '║' };// make into list
        public string[,] borderMap = new string[40, 120];
        private string borderString = "";

        // ----- Manager tools
        public bool isWall(int y, int x ) {
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

            string[] borderY = borderString.Split(';');
            
            for (int i = 0; i < borderY.Length; i++) 
            {
                string[] borderX = borderY[i].Split(','); // used for .Length and to Map the Xcoordinates on 
                for (int j = 0; j < borderX.Length; j++)
                {
                    borderMap[i, j] = borderX[j];
                }
            }
        }
        public void DrawWindowBorder() 
        {
            string[] borderY = borderString.Split(';');
            string[] borderXLength = borderY[0].Split(','); //used for loop length

            //Console.WriteLine($"x: {MapXLength.Length} y: {MapY.Length}");   //debug
            //Console.WriteLine($"Map 0,0: [{borderMap[0, 0]}]"); // --- /debug/


            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < borderY.Length; i++) // nested loop to write window border
            {
                for (int j = 0; j < borderXLength.Length; j++)
                {
                    Console.Write(borderMap[i, j]);
                    
                }
            }
            Console.SetCursorPosition(1, 1);
        }
        public void SetCurrentMap()//(currently gets Map_test for prototype))
        {
            string[] MapY = System.IO.File.ReadAllLines("Map_test");// this changes to string input for Maps

            for (int i = 1; i < MapY.Length; i++){ // starts at 1 because line 0 is to pass information
                string[] MapX = MapY[i].Split();
                for (int j = 0; j < MapX.Length; j++) { 
                    //create array and append walls to list
                }
            }
        }


    }
}
