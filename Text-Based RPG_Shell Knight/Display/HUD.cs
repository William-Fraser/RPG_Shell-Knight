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
        private string hudNameHealthShield;
        private string hudName;
        private int[] hudHealth = { 0, 0 };
        private int[] hudShield = { 0, 0 };

        //inventory
        public static int ITEM_POTHEAL  = 0;
        public static int ITEM_POTSHELL = 1;
        public static int ITEM_KEYBIG   = 2;
        public static int ITEM_KEYSMALL = 3;

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
            hudName = name;

            _inventoryStock[ITEM_POTHEAL] = 0;
            _inventoryStock[ITEM_POTSHELL] = 0;
            _inventoryStock[ITEM_KEYBIG] = 0;
            _inventoryStock[ITEM_KEYSMALL] = 0;

            AdjustBlank();
            AdjustTextBox();
        }

        // ----- gets / sets
        public int[] getInventoryStock()
        {
            return _inventoryStock;
        }
        public char[] getInventoryAvatars()
        {
            return _inventoryAvatars;
        }
        public void setInventoryStockItem(int item, int value)
        {
            _inventoryStock[item] = value;
        }
        public void setHudHealthAndShield(int[] health, int[] shield)
        {
            if (shield == null) { shield = hudShield; } // sets shield if it's null from character attack
            hudHealth[0] = health[0]; 
            hudHealth[1] = health[1];
            hudShield[0] = shield[0];
            hudShield[1] = shield[1];

            hudNameHealthShield = "";
            hudNameHealthShield += $" {hudName} |";
            hudNameHealthShield += $" Shell : {hudShield[0]}/{hudShield[1]} |";
            hudNameHealthShield += $" Health : {hudHealth[0]}/{hudHealth[1]} ";
        }

        //public methods
        public void AdjustTextBox()
        {
            // draws textbox
            int moveUI = (Console.WindowWidth / 2) - (Camera.displayWidth / 2);
            if (moveUI < 0) { moveUI = 0; }

            int line = Console.WindowHeight - 6;
            if (Console.WindowHeight != Camera.minConsoleSizeHeight || Console.WindowWidth != Camera.minConsoleSizeWidth) 
            {
                for (int i = 0; i < 4; i++)
                {
                    Console.SetCursorPosition(moveUI, line);
                    Console.Write($"║{blank}║");
                    line++;
                }
                Console.SetCursorPosition(moveUI, line);
            }
            else
            {
                Console.SetCursorPosition(0, Console.WindowHeight - 6); 
                Console.WriteLine($"║{blank}║");
                Console.WriteLine($"║{blank}║");
                Console.WriteLine($"║{blank}║");
                Console.WriteLine($"║{blank}║");
            }
            Console.Write("╚");
            for (int i = 0; i < 117; i++) { Console.Write("═"); }
            Console.Write("╝");
        }
        public void Draw(Toolkit toolkit) // displays HUD on screen
        {
            //Shell
            //Health
            //Essence - magic unlocked? strech goal...

            int moveUI = (Console.WindowWidth / 2) - (Camera.displayWidth / 2);
            if (moveUI < 0) { moveUI = 0; }

            //seperates HUD from the Camera display
            if (Console.WindowHeight != Camera.minConsoleSizeHeight || Console.WindowWidth != Camera.minConsoleSizeWidth)
            { Console.SetCursorPosition(moveUI, Console.WindowHeight - 9); }
            else
            { Console.SetCursorPosition(0, Console.WindowHeight - 9); }
            if (Console.WindowHeight == Camera.minConsoleSizeHeight || Console.WindowWidth == Camera.minConsoleSizeWidth) { Console.Write("╠"); }
            else { Console.Write("╔"); }
            for (int i = 0; i < hudNameHealthShield.Length; i++) { Console.Write("═"); }
            Console.Write("╦");
            for (int i = 0; i < (Camera.displayWidth - hudNameHealthShield.Length - 2); i++) { Console.Write("═"); }
            if (Console.WindowHeight == Camera.minConsoleSizeHeight || Console.WindowWidth == Camera.minConsoleSizeWidth) { Console.Write("╣"); }
            else { Console.Write("╗"); }

            // clears HUD
            if (Console.WindowHeight != Camera.minConsoleSizeHeight || Console.WindowWidth != Camera.minConsoleSizeWidth)
            { Console.SetCursorPosition(moveUI + 1, Console.WindowHeight - 8); }
            else
            { Console.SetCursorPosition(1, Console.WindowHeight - 8); }
            Console.WriteLine(blank);

            //seperates HUD from the Text box

            if (Console.WindowHeight != Camera.minConsoleSizeHeight || Console.WindowWidth != Camera.minConsoleSizeWidth)
            { Console.SetCursorPosition(moveUI, Console.WindowHeight - 7); }
            Console.Write("╠");
            for (int i = 0; i < hudNameHealthShield.Length; i++) { Console.Write("═"); }
            Console.Write("╩");
            for (int i = 0; i < (Camera.displayWidth - hudNameHealthShield.Length - 2); i++) { Console.Write("═"); }
            Console.WriteLine("╣");

            // write HUD values
            if (Console.WindowHeight != Camera.minConsoleSizeHeight || Console.WindowWidth != Camera.minConsoleSizeWidth)
            { Console.SetCursorPosition(moveUI, Console.WindowHeight - 8); }
            else
            { Console.SetCursorPosition(0, Console.WindowHeight - 8); }
            Console.Write("║");
            if (Console.WindowHeight != Camera.minConsoleSizeHeight || Console.WindowWidth != Camera.minConsoleSizeWidth)
            { Console.SetCursorPosition(moveUI+1, Console.WindowHeight - 8); }
            else
            { Console.SetCursorPosition(1, Console.WindowHeight - 8); }
            Console.Write(hudNameHealthShield);
            Console.Write("║");
            Console.Write(hudInventory);
            if (Console.WindowHeight != Camera.minConsoleSizeHeight || Console.WindowWidth != Camera.minConsoleSizeWidth)
            { Console.SetCursorPosition(moveUI+ (Camera.borderWidth - 1), Console.WindowHeight - 8); }
            else
            { Console.SetCursorPosition(Camera.borderWidth - 1, Console.WindowHeight - 8); }
            Console.Write("║");
            
        }
        public void AdjustBlank()
        {
            blank = "";
            for (int i = 0; i < Camera.borderWidth - 2; i++)
            {
                blank += " ";
            }
        }
        public void AdjustInvetory(List<Item> items) // adds items to inventory when picked up
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].PickedUp())
                {
                    switch (items[i].Avatar())
                    {
                        case 'ö':
                            _inventoryAvatars[ITEM_POTHEAL] = 'ö';
                            _inventoryStock[ITEM_POTHEAL]++;
                            break;

                        case 'ï':
                            _inventoryAvatars[ITEM_POTSHELL] = 'ï';
                            _inventoryStock[ITEM_POTSHELL]++;
                            break;
                        
                        case 'K':
                            _inventoryAvatars[ITEM_KEYBIG] = 'K';
                            _inventoryStock[ITEM_KEYBIG]++;
                            break;
                        
                        case 'k':
                            _inventoryAvatars[ITEM_KEYSMALL] = 'k';
                            _inventoryStock[ITEM_KEYSMALL]++;
                            break;
                    }

                    // finishes the pick up process of the item
                    items[i].PickedUp(false);
                }
            }
            hudInventory = "";
            if (_inventoryStock[ITEM_POTSHELL] > 0 && _inventoryStock[ITEM_POTSHELL] < 2)    { hudInventory += $" [Z] ï "; hudInventory += "|"; }
            else if (_inventoryStock[ITEM_POTSHELL] > 1)                 { hudInventory += $" [Z] ï x {_inventoryStock[ITEM_POTSHELL]} "; hudInventory += "|"; }

            if (_inventoryStock[ITEM_POTHEAL] > 0 && _inventoryStock[ITEM_POTHEAL] < 2) { hudInventory += $" [X] ö "; hudInventory += "|";}
            else if (_inventoryStock[ITEM_POTHEAL] > 1)               { hudInventory += $" [X] ö x {_inventoryStock[ITEM_POTHEAL]} "; hudInventory += "|"; }

            if (_inventoryStock[ITEM_KEYBIG] > 0 && _inventoryStock[ITEM_KEYBIG] < 2)       { hudInventory += $" K "; hudInventory += "|"; }
            else if (_inventoryStock[ITEM_KEYBIG] > 1)                   { hudInventory += $" K x {_inventoryStock[ITEM_KEYBIG]} "; hudInventory += "|"; }
            
            if (_inventoryStock[ITEM_KEYSMALL] > 0 && _inventoryStock[ITEM_KEYSMALL] < 2)   { hudInventory += $" k "; hudInventory += "|"; }
            else if (_inventoryStock[ITEM_KEYSMALL] > 1)                 { hudInventory += $" k x {_inventoryStock[ITEM_KEYSMALL]} "; hudInventory += "|"; }
        }
        public void Update(Player player, List<Item> items)
        {
            setHudHealthAndShield(player.Health(), player.Shield());
            AdjustInvetory(items);
        }
        public void DisplayText(string message, bool waitForKey = true) // bool for game over text
        {
            AdjustBlank();

            //if (message != blank)
            //{
            //    string PaktC = ", Press any key to Continue.";
            //    message += PaktC;
            //}

            int moveUI = (Console.WindowWidth / 2) - (Camera.displayWidth / 2);
            if (moveUI < 0) { moveUI = 0; }

            if (message != blank)
            {
                SaveMessage(currentMessage);
                DisplayText(blank);
            }
            if (Console.WindowHeight != Camera.minConsoleSizeHeight || Console.WindowWidth != Camera.minConsoleSizeWidth)
            { Console.SetCursorPosition(moveUI + 1, Console.WindowHeight - 3); }
            else
            { Console.SetCursorPosition(1, Console.WindowHeight - 3); }
            Console.Write(message);
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
