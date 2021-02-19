using System;
using System.IO;
using System.Collections.Generic;

namespace Text_Based_RPG_Shell_Knight
{
    class MapMANAGE
    {
        static int MapDisplay_X = Console.WindowWidth;
        static int MapDisplay_Y = Console.WindowHeight-2;
        
        public List<string> wallHold = new List<string>();

        // Map
        private string holdMap;
        private string[,] displayMap = new string[MapDisplay_X, MapDisplay_Y];

        // border
        private string borderString = "";
        private string[,] borderMap = new string[MapDisplay_X, MapDisplay_Y];

        // ----- gets sets

        public string getdisplayMapTile(int x, int y) 
        {
            return displayMap[x, y];
        }
        public string getwallHold()
        { 
            string allwalls = string.Join("", wallHold);
            return allwalls;
        }

        // ----- Manager tools
        public void checkPosition(int x, int y) {
            string allWalls = string.Join("", wallHold);
            Console.SetCursorPosition(1, 0);
            Console.Write($"selected map tile: [{displayMap[x, y]}] walls available: [{allWalls}]"); // --- debug
            Console.ReadKey(true);
        }

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
        //}
        private void addWall(string walls)
        {
            wallHold.Add(walls);   
        }
        private void removeWall(string walls) 
        {
            wallHold.Remove(walls);
            
        }

        //wall list might clear when entering new area?

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
            removeWall(getwallHold());
            holdMap = File.ReadAllText("Map_test.txt");// this changes to string input for Maps is _test for now
            if (!File.Exists("Map_test.txt")) {// input same string
                throw new Exception("File path does not Exist");
            }

            string[] MapY = holdMap.Split();
            string walls = MapY[0];// line 0 passes walls
            for (int y = 1; y < MapDisplay_Y; y++){ // starts at 1 because line 0 is to pass information
                string[] MapX = new string[MapDisplay_X];
                MapX[y] = MapY[y];
                for (int x = 0; x < MapDisplay_X; x++) {
                    //create array
                    displayMap[x, y] = MapX[x];
                }
            }
            addWall(walls);
        }
        public void DrawCurrentMap() 
        {
            Console.SetCursorPosition(0,0);
            Console.Write(displayMap[0,0]);
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
