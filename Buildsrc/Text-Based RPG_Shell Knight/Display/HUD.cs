using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    /// <Contains>
    /// HUD
    /// player menu access by menu button
    ///     equipment & items
    ///     player revealed world map?
    /// Save & Load Screen
    /// </Contains>

    class HUD
    {
        //player status
        private string hudPlayerStatusBar;
        private string hudName;
        private int[] hudHealth = { 0, 0 };
        private int[] hudShield = { 0, 0 };
        private string hudBarSaved;

        //inventory
        public enum ITEM 
        {
        POTHEAL,
        POTSHELL,
        KEYBIG,
        KEYSMALL
        };
        private string hudInventory;
        private int[] _inventoryStock = new int[4];  // = to length of inventory// upgradeable?
        private char[] _inventoryAvatars = new char[4];//

        //text box
        public string blank = "";

        string currentMessage;
        string recentMessage = "";
        string middleMessage = "";
        string decayingMessage = "";

        //constructor 
        public HUD(string name)
        {
            //init values
            hudName = name;
            hudBarSaved = "";
            _inventoryStock[(int)ITEM.POTHEAL] = 0;
            _inventoryStock[(int)ITEM.POTSHELL] = 0;
            _inventoryStock[(int)ITEM.KEYBIG] = 0;
            _inventoryStock[(int)ITEM.KEYSMALL] = 0;

            //fix blank space
            AdjustBlank();
        }

        // ----- gets / sets
        public int[] InventoryStock()
        {
            return _inventoryStock;
        }
        public char[] InventoryAvatars()
        {
            return _inventoryAvatars;
        }
        public void InventoryStockItem(int item, int value)
        {
            _inventoryStock[item] = value;
        }
        public void HudHealthAndShield(int[] health, int[] shield)
        {
            if (shield == null) { shield = hudShield; } // sets shield if it's null
            hudHealth[0] = health[0]; 
            hudHealth[1] = health[1];
            hudShield[0] = shield[0];
            hudShield[1] = shield[1];

            // construct HUD bar value
            hudPlayerStatusBar = "";
            hudPlayerStatusBar += $" {hudName} |";
            hudPlayerStatusBar += $" Shell : {hudShield[0]}/{hudShield[1]} |";
            hudPlayerStatusBar += $" Health : {hudHealth[0]}/{hudHealth[1]} ";
        }

        //public methods
        public void AdjustTextBox()
        {
            // init position
            int moveUI = (Console.WindowWidth / 2) - (Camera.displayWidth / 2);
            if (moveUI < 0) { moveUI = 0; }
            
            // position and draw // used if console is bigger than displayed output
            int line = Console.WindowHeight - 6;
            if (Console.WindowHeight > Camera.minConsoleSizeHeight)
            {
                for (int i = 0; i < 4; i++)
                {
                    Console.SetCursorPosition(moveUI, line);
                    Console.Write($"║{blank}║");
                    line++;
                }
                Console.SetCursorPosition(moveUI, line);
            }

            //draw
            else if (Console.WindowHeight == Camera.minConsoleSizeHeight)
            {
                Console.SetCursorPosition(0, Console.WindowHeight - 6);
                Console.WriteLine($"║{blank}║");
                Console.WriteLine($"║{blank}║");
                Console.WriteLine($"║{blank}║");
                Console.WriteLine($"║{blank}║");
            }
            else { }

            // bottom of box
            Console.Write("╚");
            for (int i = 0; i < 117; i++) { Console.Write("═"); }
            Console.Write("╝");
        }
        public void Draw() // displays HUD bar on screen
        {
            //Shell
            //Health
            //Essence - magic unlocked? strech goal...

            //init position
            int moveUI = (Console.WindowWidth / 2) - (Camera.displayWidth / 2);
            if (moveUI < 0) { moveUI = 0; }

            //seperates HUD from the Camera display
            //position
            if (Console.WindowHeight > Camera.minConsoleSizeHeight || Console.WindowWidth > Camera.minConsoleSizeWidth)
            { Console.SetCursorPosition(moveUI, Console.WindowHeight - 9); }
            else if (Console.WindowHeight == Camera.minConsoleSizeHeight)
            { Console.SetCursorPosition(0, Console.WindowHeight - 9); }
            else { }
            //display
            if (Console.WindowHeight <= Camera.minConsoleSizeHeight) { Console.Write("╠"); }
            else { Console.Write("╔"); }
            for (int i = 0; i < hudPlayerStatusBar.Length; i++) { Console.Write("═"); }
            Console.Write("╦");
            for (int i = 0; i < (Camera.displayWidth - hudPlayerStatusBar.Length - 2); i++) { Console.Write("═"); }
            if (Console.WindowHeight <= Camera.minConsoleSizeHeight) { Console.Write("╣"); }
            else { Console.Write("╗"); }

            // clears HUD bar if need be also sets position
            string checkHudBar = hudPlayerStatusBar + hudInventory;
            if (checkHudBar != hudBarSaved)
            {
                if (Console.WindowHeight > Camera.minConsoleSizeHeight)
                { Console.SetCursorPosition(moveUI, Console.WindowHeight - 8); }
                else if (Console.WindowHeight == Camera.minConsoleSizeHeight && Console.WindowWidth == Camera.minConsoleSizeWidth)
                { Console.SetCursorPosition(0, Console.WindowHeight - 8); }
                else { }
                Console.WriteLine(blank);
                hudBarSaved = checkHudBar;
            }
            else // position
            {   
                if (Console.WindowHeight > Camera.minConsoleSizeHeight || Console.WindowWidth > Camera.minConsoleSizeWidth)
                { Console.SetCursorPosition(moveUI, Console.WindowHeight - 8); }
                else if (Console.WindowHeight == Camera.minConsoleSizeHeight)
                { Console.SetCursorPosition(0, Console.WindowHeight - 8); }
                Console.WriteLine();
            }

            //seperates HUD bar from the Text box
            //position
            if (Console.WindowHeight > Camera.minConsoleSizeHeight || Console.WindowWidth > Camera.minConsoleSizeWidth)
            { Console.SetCursorPosition(moveUI, Console.WindowHeight - 7); }
            else if (Console.WindowHeight == Camera.minConsoleSizeHeight)
            {
                Console.SetCursorPosition(0, Console.WindowHeight - 7);

            }
            //display
            Console.Write("╠");
            for (int i = 0; i < hudPlayerStatusBar.Length; i++) { Console.Write("═"); }
            Console.Write("╩");
            for (int i = 0; i < (Camera.displayWidth - hudPlayerStatusBar.Length - 2); i++) { Console.Write("═"); }
            Console.WriteLine("╣");

            // write HUD bar values
            //position
            if (Console.WindowHeight > Camera.minConsoleSizeHeight || Console.WindowWidth > Camera.minConsoleSizeWidth)
            { Console.SetCursorPosition(moveUI, Console.WindowHeight - 8); }
            else if (Console.WindowHeight == Camera.minConsoleSizeHeight)
            { Console.SetCursorPosition(0, Console.WindowHeight - 8); }
            else { }
            //display HUD Bar with positioning
            Console.Write("║");
            if (Console.WindowHeight > Camera.minConsoleSizeHeight || Console.WindowWidth > Camera.minConsoleSizeWidth)
            { Console.SetCursorPosition(moveUI + 1, Console.WindowHeight - 8); }
            else if (Console.WindowHeight == Camera.minConsoleSizeHeight)
            { Console.SetCursorPosition(1, Console.WindowHeight - 8); }
            else { }
            Console.Write(hudPlayerStatusBar);
            Console.Write("║");
            Console.Write(hudInventory);
            if (Console.WindowHeight > Camera.minConsoleSizeHeight && Console.WindowWidth > Camera.minConsoleSizeWidth)
            { Console.SetCursorPosition(moveUI+ (Camera.borderWidth - 1), Console.WindowHeight - 8); }
            else if (Console.WindowHeight == Camera.minConsoleSizeHeight && Console.WindowWidth == Camera.minConsoleSizeWidth)
            { Console.SetCursorPosition(Camera.borderWidth - 1, Console.WindowHeight - 8); }
            else { }
            Console.Write("║");
            
        }
        public void AdjustBlank() // adjusts the size of blank space according to the camera border
        {
            blank = "";
            for (int i = 0; i < Camera.borderWidth - 2; i++)
            {
                blank += " ";
            }
        }
        public void AdjustInvetory(List<Item> items = null) // adds items to inventory when picked up
        {
            if (items != null)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i].PickedUp())
                    {
                        switch (items[i].Avatar())
                        {
                            case 'ö':
                                _inventoryAvatars[(int)ITEM.POTHEAL] = 'ö';
                                _inventoryStock[(int)ITEM.POTHEAL]++;
                                break;

                            case 'ï':
                                _inventoryAvatars[(int)ITEM.POTSHELL] = 'ï';
                                _inventoryStock[(int)ITEM.POTSHELL]++;
                                break;

                            case 'K':
                                _inventoryAvatars[(int)ITEM.KEYBIG] = 'K';
                                _inventoryStock[(int)ITEM.KEYBIG]++;
                                break;

                            case 'k':
                                _inventoryAvatars[(int)ITEM.KEYSMALL] = 'k';
                                _inventoryStock[(int)ITEM.KEYSMALL]++;
                                break;
                        }

                        // finishes the pick up process of the item
                        items[i].PickedUp(false);
                    }
                }
            }

            //display information
            hudInventory = "";
            if (_inventoryStock[(int)ITEM.POTSHELL] > 0 && _inventoryStock[(int)ITEM.POTSHELL] < 2)    { hudInventory += $" [Z] ï "; hudInventory += "|"; }
            else if (_inventoryStock[(int)ITEM.POTSHELL] > 1)                 { hudInventory += $" [Z] ï x {_inventoryStock[(int)ITEM.POTSHELL]} "; hudInventory += "|"; }

            if (_inventoryStock[(int)ITEM.POTHEAL] > 0 && _inventoryStock[(int)ITEM.POTHEAL] < 2) { hudInventory += $" [X] ö "; hudInventory += "|";}
            else if (_inventoryStock[(int)ITEM.POTHEAL] > 1)               { hudInventory += $" [X] ö x {_inventoryStock[(int)ITEM.POTHEAL]} "; hudInventory += "|"; }

            if (_inventoryStock[(int)ITEM.KEYBIG] > 0 && _inventoryStock[(int)ITEM.KEYBIG] < 2)       { hudInventory += $" K "; hudInventory += "|"; }
            else if (_inventoryStock[(int)ITEM.KEYBIG] > 1)                   { hudInventory += $" K x {_inventoryStock[(int)ITEM.KEYBIG]} "; hudInventory += "|"; }
            
            if (_inventoryStock[(int)ITEM.KEYSMALL] > 0 && _inventoryStock[(int)ITEM.KEYSMALL] < 2)   { hudInventory += $" k "; hudInventory += "|"; }
            else if (_inventoryStock[(int)ITEM.KEYSMALL] > 1)                 { hudInventory += $" k x {_inventoryStock[(int)ITEM.KEYSMALL]} "; hudInventory += "|"; }
        }
        
        public void DisplayText(string message, bool waitForKey = true) // bool for game over text
        {
            AdjustBlank();

            //init position
            int moveUI = (Console.WindowWidth / 2) - (Camera.displayWidth / 2);
            if (moveUI < 0) { moveUI = 0; }

            //clears and saves current message, if message is not the blank
            if (message != blank)
            {
                SaveMessage(currentMessage);
                DisplayText(blank);
            }

            //posiiton
            if (Console.WindowHeight != Camera.minConsoleSizeHeight || Console.WindowWidth != Camera.minConsoleSizeWidth)
            { Console.SetCursorPosition(moveUI + 1, Console.WindowHeight - 3); }
            else
            { Console.SetCursorPosition(1, Console.WindowHeight - 3); }

            // writes message to console
            Console.Write(message);

            // sets current message and if waitforkey = true then waits for any key, only if message is not the blank
            if (message != blank)
            {
                if (waitForKey)
                {
                    Console.CursorVisible = true;



                    Console.Write(", press any key to continue");
                    Console.ReadKey(true);
                    Console.CursorVisible = false;
                    DisplayText(blank);
                }
                currentMessage = message;
                //position
                if (Console.WindowHeight != Camera.minConsoleSizeHeight || Console.WindowWidth != Camera.minConsoleSizeWidth)
                { Console.SetCursorPosition(moveUI + 1, Console.WindowHeight - 3); }
                else
                { Console.SetCursorPosition(1, Console.WindowHeight - 3); }
                Console.Write(currentMessage); 
            }
        }
        public void SaveMessage(string message)
        {
            if (message != "")
            {
                
                decayingMessage = middleMessage;
                middleMessage = recentMessage;
                recentMessage = currentMessage;

                int moveUI = (Console.WindowWidth / 2) - (Camera.displayWidth / 2);
                if (moveUI < 0) { moveUI = 0; }

                if (Console.WindowHeight != Camera.minConsoleSizeHeight || Console.WindowWidth != Camera.minConsoleSizeWidth)
                { Console.SetCursorPosition(moveUI + 1, Console.WindowHeight - 4); }
                else
                { Console.SetCursorPosition(1, Console.WindowHeight - 4); }
                Console.Write(blank);
                if (Console.WindowHeight != Camera.minConsoleSizeHeight || Console.WindowWidth != Camera.minConsoleSizeWidth)
                { Console.SetCursorPosition(moveUI + 1, Console.WindowHeight - 4); }
                else
                { Console.SetCursorPosition(1, Console.WindowHeight - 4); }
                Console.Write(recentMessage);
                if (Console.WindowHeight != Camera.minConsoleSizeHeight || Console.WindowWidth != Camera.minConsoleSizeWidth)
                { Console.SetCursorPosition(moveUI + 1, Console.WindowHeight - 5); }
                else
                { Console.SetCursorPosition(1, Console.WindowHeight - 5); }
                Console.Write(blank);
                if (Console.WindowHeight != Camera.minConsoleSizeHeight || Console.WindowWidth != Camera.minConsoleSizeWidth)
                { Console.SetCursorPosition(moveUI + 1, Console.WindowHeight - 5); }
                else
                { Console.SetCursorPosition(1, Console.WindowHeight - 5); }
                Console.Write(middleMessage);
                if (Console.WindowHeight != Camera.minConsoleSizeHeight || Console.WindowWidth != Camera.minConsoleSizeWidth)
                { Console.SetCursorPosition(moveUI + 1, Console.WindowHeight - 6); }
                else
                { Console.SetCursorPosition(1, Console.WindowHeight - 6); }
                Console.Write(blank); 
                if (Console.WindowHeight != Camera.minConsoleSizeHeight || Console.WindowWidth != Camera.minConsoleSizeWidth)
                { Console.SetCursorPosition(moveUI + 1, Console.WindowHeight - 6); }
                else
                { Console.SetCursorPosition(1, Console.WindowHeight - 6); }
                Console.Write(decayingMessage);
            }
        }
        public void Update(Player player, List<Item> items)
        {
            HudHealthAndShield(player.Health(), player.Shield());
            AdjustInvetory(items);
            Draw();
        }

        //scrolling text

        //cursor = true
        //    string continueMessage = ", Press any key to Continue.";
        //    char[] printContinue = new char[continueMessage.Length];

        //            for (int j = 0; j<continueMessage.Length; j++) {
        //                printContinue[j] = continueMessage[j];
        //            }
        //Thread.Sleep(100);
        //            for (int i = 0; i<printContinue.Length; i++) {
        //                Thread.Sleep(7);
        //                Console.Write(printContinue[i]);
        //            }
        //cursor = false
    }
}
