using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    class Camera
    {
        // states for if the player is on the border or not
        //private int _stateMap;
        //public static int MAP_BORDER = 0;
        //public static int MAP_MIDDLE = 1;

        //full map with objects placed on it
        private char[,] _gameWorld;

        //displayed game screen
        private char[,] _display;
        private char[,] _buffer;

        // border
        private string borderString = "";
        private string[,] borderArray;

        // height and width of camera
        public static int borderHeight = Console.WindowHeight - 9;
        public static int borderWidth = Console.WindowWidth - 1;

        // height and width of displayed screen 
        public static int displayHeight = Camera.borderHeight - 2;
        public static int displayWidth = Camera.borderWidth - 2;

        // saved console height and width for checks to clear a size modifyed console
        public static int savedHeight = 0;
        public static int savedWidth = 0;

        // min height and width
        public const int minConsoleSizeWidth = 120;
        public const int minConsoleSizeHeight = 30;

        // state of Console size used for HUD compacting
        private int _stateConsoleSize;
        public const int CONSOLESIZE_MIN = 0;
        public const int CONSOLESIZE_ADJUSTED = 1;



        // map holder
        Map map;
        char[,] _map;

        //constructor
        public Camera(Map map)
        {
            this.map = map;
            _gameWorld = new char[Map.height, Map.width];
            _display = new char[displayHeight, displayWidth];
            _buffer = new char[displayHeight, displayWidth];
        }

        // ----- gets / sets
        public int getStateConsoleSize()
        {
            return _stateConsoleSize;
        }
        public void GameWorldGetMap() // set
        {
            _map = map.getMap();
            _gameWorld = _map;
        }
        public void GameWorldTile(char avatar, int x, int y) // set
        {
            _gameWorld[y -1, x] = avatar; // y-1 because of the gap on top of the consoleMinSize
        }
        public char GameWorldTile(int x, int y) // get
        {
            return _gameWorld[y - 1, x]; // y-1 because of the gap on top of the consoleMinSize
        }

        // ----- private methods
        private void setDisplay(int yStart, int yEnd, int xStart, int xEnd)
        {
            int setY = 0;
            for (int y = yStart; y < yEnd; y++)
            {
                int setX = 0;
                for (int x = xStart; x < xEnd; x++)
                {
                    _display[setY, setX] = _gameWorld[y, x];
                    if (setX < displayWidth - 2) { setX++; }
                    else { }
                }
                if (setY < displayHeight - 2) { setY++; }

            }
            
        }

        // ----- public methods
        public void Draw(Player player)
        {
            AdjustDisplayedArea(player);

            //init positioning for display
            int moveUIX = (Console.WindowWidth / 2) - (Camera.displayWidth / 2);
            if (moveUIX < 0) { moveUIX = 0; }

            int moveUIY = (Console.WindowHeight / 3) - (Camera.displayHeight / 2);
            if (moveUIY < 0) { moveUIY = 0; }

            if (Console.WindowHeight != Camera.minConsoleSizeHeight || Console.WindowWidth != Camera.minConsoleSizeWidth)
            { Console.SetCursorPosition(moveUIX + 1, moveUIY + 2); }
            else
            { Console.SetCursorPosition(1, 2); }

            //write the display
            int line = 2;
            for (int y = 0; y < displayHeight - 1; y++)
            {
                for (int x = 0; x < displayWidth - 1; x++)
                {
                    if (_display[y, x] != _buffer[y, x])
                    {
                        Console.Write(_display[y, x]);

                        //on last frame of writing display set the buffer
                        if (x == displayWidth - 1)
                        { _buffer = _display; }
                    }
                    else
                    { }
                }

                //move down a line
                line++;
                if (Console.WindowHeight != Camera.minConsoleSizeHeight || Console.WindowWidth != Camera.minConsoleSizeWidth)
                { Console.SetCursorPosition(moveUIX + 1, moveUIY + line); }
                else
                {
                    Console.SetCursorPosition(1, line);
                }
            }
        }
        public void AdjustDisplayedArea(Player player) //draws the area around the player
        {
            //// reset bounds for display
            //borderHeight = Console.WindowHeight - 9;
            //borderWidth = Console.WindowWidth - 1;

            //// reset height and width of displayed screen 
            //displayHeight = Camera.borderHeight - 1;
            //displayWidth = Camera.borderWidth - 2;

            int xStart = player.X() - (Camera.displayWidth / 2);
            int xEnd = player.X() + (Camera.displayWidth / 2) - 1;
            int yStart = player.Y() - (Camera.displayHeight / 2);
            int yEnd = player.Y() + (Camera.displayHeight / 2) - 1;


            if (xStart <= 0)
            {
                xStart = 0; xEnd = Camera.displayWidth - 1;
            }
            else if (xEnd >= Map.width)
            {
                xStart = Map.width - (Camera.displayWidth - 1); xEnd = Map.width;
            }
            if (yStart <= 0)
            {
                yStart = 0; yEnd = Camera.displayHeight - 1;
            }
            if (yEnd >= Map.height)
            {
                yStart = Map.height - (Camera.displayHeight - 1); yEnd = Map.height;
            }
            else
            {
            }
            setDisplay(yStart, yEnd, xStart, xEnd);
        }
        public void ResetConsole(HUD hud) // resets the display if the console is modifyed
        {
            if (Console.WindowHeight != savedHeight || Console.WindowWidth != savedWidth)
            {
                // reset range check to current size
                savedHeight = Console.WindowHeight;
                savedWidth = Console.WindowWidth;

                // set state for defaults to min
                _stateConsoleSize = CONSOLESIZE_MIN;

                // set state if window is different
                if (Console.WindowHeight != minConsoleSizeHeight || Console.WindowWidth != minConsoleSizeWidth)
                {
                    _stateConsoleSize = CONSOLESIZE_ADJUSTED;
                }
                if (Console.WindowHeight < minConsoleSizeHeight || Console.WindowWidth < minConsoleSizeWidth)
                {
                    Console.WindowHeight = minConsoleSizeHeight; Console.WindowWidth = minConsoleSizeWidth;
                    ResetConsole(hud);
                }

                // set up display for new size
                Console.Clear();
                hud.AdjustTextBox();
                DrawBorder(); // border
            }
        }
        public void Update(Player player)
        {
            map.loadMap();
            GameWorldGetMap();
        }

        //camera border
        public void UpdateBorder() // 
        {
            //// fix border to window size
            //borderHeight = Console.WindowHeight - 9;
            //borderWidth = Console.WindowWidth - 1;

            // create new border instance for update
            borderArray = new string[borderHeight, borderWidth];

            //reset borderstring

            int moveUI = (Console.WindowWidth / 2) - (Camera.displayWidth / 2);
            if (moveUI < 0) { moveUI = 0; }

            borderString = "";

            if (Console.WindowHeight != Camera.minConsoleSizeHeight || Console.WindowWidth != Camera.minConsoleSizeWidth)
            { Console.SetCursorPosition(moveUI, 0); }
            else
            { Console.SetCursorPosition(0, 0); }
            borderString += "╔,"; //Console.Write("╔"); 
            for (int i = 0; i < borderWidth - 2; i++)
            {
                borderString += "═,"; //Console.Write("═"); 
            }
            borderString += "╗;"; //Console.Write("╗"); 
            for (int i = 0; i < borderHeight - 2; i++)
            {
                borderString += "║,"; //Console.Write("║"); 
                for (int j = 0; j < borderWidth - 2; j++)
                {
                    borderString += _display[i, j] + ","; // LEGACY : Console.Write(" "); 
                }
                borderString += "║;"; //Console.Write("║"); 
            }
            borderString += "╚,"; //Console.Write("╚"); 
            for (int i = 0; i < borderWidth - 2; i++)
            {
                borderString += "═,"; //Console.Write("═"); 
            }
            borderString += "╝"; //Console.Write("╝");

            string[] borderY = borderString.Split(';');

            for (int i = 0; i < borderY.Length; i++)
            {
                string[] borderX = borderY[i].Split(','); // used for .Length and to Map the Xcoordinates on 
                for (int j = 0; j < borderX.Length; j++)
                {
                    borderArray[i, j] = borderX[j];
                }
            }

        }
        public void DrawBorder()
        {
            UpdateBorder();
            string[] borderY = borderString.Split(';');
            string[] borderXLength = borderY[0].Split(','); //used for loop length

            int moveUIX = (Console.WindowWidth / 2) - (Camera.displayWidth / 2);
            if (moveUIX < 0) { moveUIX = 0; }

            int moveUIY = (Console.WindowHeight / 3) - (Camera.displayHeight / 2);
            if (moveUIY < 0) { moveUIY = 0; }

            //Console.WriteLine($"x: {MapXLength.Length} y: {MapY.Length}");   //debug
            //Console.WriteLine($"Map 0,0: [{borderMap[0, 0]}]"); // --- /debug/

            if (Console.WindowHeight != Camera.minConsoleSizeHeight || Console.WindowWidth != Camera.minConsoleSizeWidth)
            { Console.SetCursorPosition(moveUIX, moveUIY+1); }
            else
            { Console.SetCursorPosition(0, 1); }
            int line = 2;
            for (int i = 0; i < borderHeight; i++) // nested loop to write window border
            {
                for (int j = 0; j < borderWidth; j++)
                {
                    Console.Write(borderArray[i, j]);
                }
                if (Console.WindowHeight != Camera.minConsoleSizeHeight || Console.WindowWidth != Camera.minConsoleSizeWidth)
                { Console.SetCursorPosition(moveUIX, moveUIY + line); }
                else
                { Console.SetCursorPosition(0, line); }
                line++;
            }
            //for (int i = 0; i <= borderY.Length; i++)
            //{
            //    Console.Write(borderY[i]);
            //}
        }
    }
}
