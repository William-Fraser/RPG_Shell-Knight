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
        private static ConsoleKeyInfo savedReset; // set to null on purpose, for resetting choice 
        static GameManager game;

        static void Main()
        {
            game = new GameManager();
            _state = PROGRAMSTATE_PLAYMENU;

            //only exits if program is in ending state
            while (_state != PROGRAMSTATE_ENDING)
            {
                while (_state == PROGRAMSTATE_PLAYMENU) // Main Menu
                {
                    string mainMenu = "@ SHELL KNIGHT ";
                    //Display Main Menu and obtain choice
                    TwoOptionMenu(mainMenu, "Start", PROGRAMSTATE_PLAYGAME, "Exit",  PROGRAMSTATE_GAMEOVER);
                }
                //skip gameplay if exiting in gameover state
                while (_state != PROGRAMSTATE_GAMEOVER)
                {
                    //plays game while in play state
                    if (_state == PROGRAMSTATE_PLAYGAME)
                    {
                        // start game
                        game.Game();

                        //game ends
                        if (game._gameState == GameManager.GAMESTATE.GAMEOVER)
                        _state = PROGRAMSTATE_GAMEOVER;
                        
                    }
                }
                ////ask to return to menu
                //string returnToMenu = "Return to Menu?";
                //TwoOptionMenu(returnToMenu, "Start Screen", PROGRAMSTATE_PLAYMENU, "Exit Game", PROGRAMSTATE_ENDING);

                //set to exit by default
                Console.Clear();
                Console.WriteLine("\n");
                Console.WriteLine("     END OF PROGRAM");
                Console.ReadKey(true);
                if (_state == PROGRAMSTATE_GAMEOVER)
                    _state = PROGRAMSTATE_ENDING;
            }
        }

        //used for program menus
        public static void TwoOptionMenu(string MenuDisplay, string optionS, int resultS, string optionE,int resultE)
        {
            
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
            Console.Clear();
        }
    }
}

