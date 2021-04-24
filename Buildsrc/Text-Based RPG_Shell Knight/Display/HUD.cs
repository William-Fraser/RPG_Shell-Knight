using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    class HUD
    {
        // stretch goal: interactable and/or infinate hud that saves to a file, could be included in a Save Game update

        //player stats
        private string name;
        private string status; // health and shield in string form
        private int[] health = { 0, 0 };
        private int[] shield = { 0, 0 };
        private string arm;
        private string hotbar;

        //text box
        private string displayedBar;
        public string blank = "";

        string currentMessage;
        string recentMessage = "";
        string middleMessage = "";
        string decayingMessage = "";

        //constructor 
        public HUD(string name)
        {
            //init values
            this.name = name;
            displayedBar = "";

            //fix blank space
            AdjustBlank();
        }

        // ----- gets / sets
        public void setHudHealthAndShield(int[] health, int[] shield)
        {
            if (shield == null) { shield = this.shield; } // sets shield if it's null
            this.health[0] = health[0]; 
            this.health[1] = health[1];
            this.shield[0] = shield[0];
            this.shield[1] = shield[1];

            // construct HUD bar value
            status = "";
            status += $" {name} |";
            status += $" Shell : {this.shield[0]}/{this.shield[1]} |";
            status += $" Health : {this.health[0]}/{this.health[1]} ";
        }

        // ----- private methods
        private void AdjustBlank() // adjusts the size of blank space according to the camera border
        {
            blank = "";
            for (int i = 0; i < Camera.borderWidth - 2; i++)
            {
                blank += " ";
            }
        }
        private void SaveMessage(string message)
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

        // ----- public methods
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
            for (int i = 0; i < status.Length; i++) { Console.Write("═"); }
            Console.Write("╦");
            for (int i = 0; i < (Camera.displayWidth - status.Length - 2); i++) { Console.Write("═"); }
            if (Console.WindowHeight <= Camera.minConsoleSizeHeight) { Console.Write("╣"); }
            else { Console.Write("╗"); }

            // clears HUD bar if need be also sets position
            string checkHudBar = status + hotbar;
            if (checkHudBar != displayedBar)
            {
                if (Console.WindowHeight > Camera.minConsoleSizeHeight)
                { Console.SetCursorPosition(moveUI, Console.WindowHeight - 8); }
                else if (Console.WindowHeight == Camera.minConsoleSizeHeight && Console.WindowWidth == Camera.minConsoleSizeWidth)
                { Console.SetCursorPosition(0, Console.WindowHeight - 8); }
                else { }
                Console.WriteLine(blank);
                displayedBar = checkHudBar;
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
            for (int i = 0; i < status.Length; i++) { Console.Write("═"); }
            Console.Write("╩");
            for (int i = 0; i < (Camera.displayWidth - status.Length - 2); i++) { Console.Write("═"); }
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
            Console.Write(status);
            Console.Write("║");
            Console.Write(hotbar);
            if (Console.WindowHeight > Camera.minConsoleSizeHeight && Console.WindowWidth > Camera.minConsoleSizeWidth)
            { Console.SetCursorPosition(moveUI+ (Camera.borderWidth - 1), Console.WindowHeight - 8); }
            else if (Console.WindowHeight == Camera.minConsoleSizeHeight && Console.WindowWidth == Camera.minConsoleSizeWidth)
            { Console.SetCursorPosition(Camera.borderWidth - 1, Console.WindowHeight - 8); }
            else { }
            Console.Write("║");
            
        }
        public void UpdateTextBox()
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
        public void UpdateHotBar(Player player, Inventory inventory) // fixes hotbar to represent important invetory items
        {
            hotbar = "";
            Weapon weapon = player.EquipedWeapon();
            int[] stock = inventory.ItemStock();
            
            
            //item update
            // SP up
            if (stock[(int)ITEM.POTSHELL] > 0 && stock[(int)ITEM.POTSHELL] < 2) { hotbar += $" [Z] ï "; hotbar += "|"; }
            else if (stock[(int)ITEM.POTSHELL] > 1) { hotbar += $" [Z] ï x {stock[(int)ITEM.POTSHELL]} "; hotbar += "|"; }
            // HP up
            if (stock[(int)ITEM.POTHEAL] > 0 && stock[(int)ITEM.POTHEAL] < 2) { hotbar += $" [X] ö "; hotbar += "|"; }
            else if (stock[(int)ITEM.POTHEAL] > 1) { hotbar += $" [X] ö x {stock[(int)ITEM.POTHEAL]} "; hotbar += "|"; }
            // Big Key
            if (stock[(int)ITEM.KEYBIG] > 0 && stock[(int)ITEM.KEYBIG] < 2) { hotbar += $" K "; hotbar += "|"; }
            else if (stock[(int)ITEM.KEYBIG] > 1) { hotbar += $" K x {stock[(int)ITEM.KEYBIG]} "; hotbar += "|"; }
            // Small Key
            if (stock[(int)ITEM.KEYSMALL] > 0 && stock[(int)ITEM.KEYSMALL] < 2) { hotbar += $" k "; hotbar += "|"; }
            else if (stock[(int)ITEM.KEYSMALL] > 1) { hotbar += $" k x {stock[(int)ITEM.KEYSMALL]} "; hotbar += "|"; }

            //weapon update
            this.hotbar += " Arm: " + weapon.Avatar();
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

            //positon
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
        public void Update(Player player, Inventory inventory)
        {
            //UpdateTextBox();
            UpdateHotBar(player, inventory);
            setHudHealthAndShield(player.Health(), player.Shield());
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
