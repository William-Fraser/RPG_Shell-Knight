using System;
using System.IO;
using System.Collections.Generic;

namespace Text_Based_RPG_Shell_Knight
{
    class Map
    {
        static int MapDisplay_X = Console.WindowWidth+8;
        static int MapDisplay_Y = Console.WindowHeight-2;
        
        public List<string> wallHold = new List<string>();

        // Map
        private char[,] displayMap = new char[MapDisplay_X, MapDisplay_Y];

        // border
        private string borderString = "";
        private string[,] borderMap;

        // ----- constructor
        public Map()
        {

        }

        // ----- gets/sets||add/remove
        public char[] getTile(int x, int y) 
        {
            char[] directions = new char[5];
            directions[1] = displayMap[x, y-1];
            directions[2] = displayMap[x+1, y];
            directions[3] = displayMap[x, y+1];
            directions[4] = displayMap[x-1, y];
            return directions;
        }
        public string getwallHold()
        { 
            string allwalls = string.Join("", wallHold);
            return allwalls;
        }
        private void addWall(string walls)
        {
            wallHold.Add(walls);
        }
        private void removeWall(string walls)
        {
            wallHold.Remove(walls);
        }

        // ----- Manager tools
        public void checkPosition(int x, int y) { // debug
            string allWalls = string.Join("", wallHold);
            Console.SetCursorPosition(1, 0);
            Console.Write($"selected map tile: [{displayMap[x, y]}] walls available: [{allWalls}]"); // --- debug
            Console.ReadKey(true);
        }
        /// legacy code
        //public bool isWall(int y, int x ) {
        //    string allWalls = string.Join("", wallHold);
        //    string[] wallgroup = allWalls.Split();
        //    for (int i = 0; i < wallgroup.Length; i++) {
        //        if (displayMap[x, y] == wallgroup[i]) {
        //            return true;
        //        }
        //        else { } // do nothing
        //    }
        //    return false;

        

        // ----- Manager Builders
        public void CreateWindowBorder() // walls ═ ║ 
        {
            borderMap = new string[Console.WindowWidth, Console.WindowHeight];

            Console.SetCursorPosition(0, 0);
            borderString += "╔,"; //Console.Write("╔"); 
            for (int i = 0; i < Console.WindowWidth -2; i++)
            {
                borderString += "═,"; //Console.Write("═"); 
            }
            borderString += "╗;"; //Console.Write("╗"); 
            for (int i = 0; i < Console.WindowHeight - 3; i++)
            {
                borderString += "║,"; //Console.Write("║"); 
                for (int j = 0; j < Console.WindowWidth - 2; j++)
                {
                    borderString += " ,"; //Console.Write(" "); 
                }
                borderString += "║;"; //Console.Write("║"); 
            }
            borderString += "╚,"; //Console.Write("╚"); 
            for (int i = 0; i < Console.WindowWidth -2; i++)
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
                    borderMap[j, i] = borderX[j];
                }
            }

        }
        public void DrawWindowBorder() 
        {
            string[] borderY = borderString.Split(';');
            string[] borderXLength = borderY[0].Split(','); //used for loop length

            //Console.WriteLine($"x: {MapXLength.Length} y: {MapY.Length}");   //debug
            //Console.WriteLine($"Map 0,0: [{borderMap[0, 0]}]"); // --- /debug/


            Console.SetCursorPosition(1, 0);
            for (int i = 0; i < borderY.Length; i++) // nested loop to write window border
            {
                for (int j = 0; j > borderXLength.Length; j++)
                {
                    Console.Write(borderMap[i, j]);                
                }
            }
            //for (int i = 0; i <= borderY.Length; i++)
            //{
            //    Console.Write(borderY[i]);
            //}
            Console.SetCursorPosition(1, 1);
        }
        public void SetCurrent()//(currently gets Map_test for prototype))
        {
            removeWall(getwallHold());
            string[] MapY = File.ReadAllLines("Map_test.txt");// this changes to string input for Maps is _test for now
            if (!File.Exists("Map_test.txt")) {// input same string
                throw new Exception("File path does not Exist");
            }
            
            string walls = MapY[0];// line 0 passes walls
            for (int y = 1; y < MapDisplay_Y; y++){ // starts at 1 because line 0 is to pass information
                char[] MapX = new char[MapDisplay_X];
                string xHolder =  MapY[y];
                for (int i = 0; i < xHolder.Length; i++)
                {
                    MapX[i] = xHolder[i];
                }
                for (int x = 0; x < MapDisplay_X; x++) {
                    //create array
                    displayMap[x, y] = MapX[x];
                }
            }
            addWall(walls);
        }
        public void DrawCurrent() 
        {
            //Console.SetCursorPosition(0,0);
            //Console.Write(displayMap[0,0]);
            int y;
            int x;
            Console.SetCursorPosition(0, 1);
            for (y = 1; y < MapDisplay_Y; y++) { // line 0 is to pass info,  
                for (x = 0; x < MapDisplay_X; x++) {
                    Console.Write(displayMap[x, y]);
                }
            }
        } 

    }
}
