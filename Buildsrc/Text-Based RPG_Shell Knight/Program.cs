using System;

namespace Text_Based_RPG_Shell_Knight
{
    class Program
    {
        // program states / change to enum and switch
        private static int _state;
        private static readonly int PROGRAMSTATE_GAMEOVER = 0;
        private static readonly int PROGRAMSTATE_PLAYGAME = 1;
        private static readonly int PROGRAMSTATE_PLAYMENU = 2;
        private static readonly int PROGRAMSTATE_ENDING = 3;

        // menu controls
        private static bool retryMenu = false;
        private static ConsoleKeyInfo decision;
        private static ConsoleKeyInfo savedReset; // set to null on purpose
        static GameManager game;

        static void Main()
        {
            game = new GameManager();
            _state = PROGRAMSTATE_PLAYMENU;

            while (_state != PROGRAMSTATE_ENDING)
            {
                while (_state == PROGRAMSTATE_PLAYMENU) // Main Menu
                {
                    string mainMenu = "@ SHELL KNIGHT ";
                    //Display Main Menu and obtain choice
                    TwoOptionMenu(mainMenu, "Start", PROGRAMSTATE_PLAYGAME, "Exit",  PROGRAMSTATE_GAMEOVER);
                }
                while (_state != PROGRAMSTATE_GAMEOVER)
                {
                    if (_state == PROGRAMSTATE_PLAYGAME)
                    {
                        // start game
                        game.Game();

                        //game ends
                        _state = PROGRAMSTATE_GAMEOVER;
                        
                    }
                }
                ////ask to return to menu
                //string returnToMenu = "Return to Menu?";
                //TwoOptionMenu(returnToMenu, "Start Screen", PROGRAMSTATE_PLAYMENU, "Exit Game", PROGRAMSTATE_ENDING);
            }
            Console.Clear();
            Console.WriteLine("Thank you so much for playing my game");
            Console.ReadKey(true);
            if (_state == PROGRAMSTATE_GAMEOVER)
            _state = PROGRAMSTATE_ENDING;
        }

        //used for program menus
        public static void TwoOptionMenu(string MenuDisplay, string optionS, int resultS, string optionE,int resultE)
        {
            Console.Clear();
            //choices
            if (decision.Key == ConsoleKey.S || decision.Key == ConsoleKey.E) 
            {

                retryMenu = false;

                switch (decision.Key)
                {
                    case ConsoleKey.S:
                        
                        decision = savedReset;
                        _state = resultS;
                        return;

                    case ConsoleKey.E:
                        decision = savedReset;
                        _state = resultE;
                        return;
                }

                //reset Menu
                
                Console.ReadKey(true);
            }
            //menu
            else
            {
                
                // if player has failed to perform operation ask for proper value
                if (retryMenu)
                {
                    Console.WriteLine();
                    Console.WriteLine(" please enter a menu value");
                }

                //menu and decision
                Console.WriteLine();
                Console.WriteLine($"     {MenuDisplay}\n        S - {optionS}\n        E - {optionE}");
                decision = Console.ReadKey(true);

                //sets to ask for proper value if trying again
                retryMenu = true;

            }
        }
    }
}

