using System;

namespace Text_Based_RPG_Shell_Knight
{
    enum PROGRAMSTATE
    {
        START,
        PLAYGAME,
        RETRY,
        END
    }
    class Program
    {
        // menu controls
        private static GameManager game;
        

        static void Main()
        {
            PROGRAMSTATE _programState = PROGRAMSTATE.START;
            game = new GameManager();
            bool play = true;

            while (play)
            {
                switch (_programState)
                {
                    case PROGRAMSTATE.START:
                        string mainMenu = Global.START_SCREEN_TITLE;
                        //Display Main Menu and obtain choice
                        _programState = (PROGRAMSTATE)Toolkit.TwoOptionMenu(mainMenu, (int)_programState, 
                            "Start", (int)PROGRAMSTATE.PLAYGAME, "Exit", (int)PROGRAMSTATE.END);
                        break;
                    case PROGRAMSTATE.PLAYGAME:
                        game.Game();

                        //game ends
                        if (game._gameState == GAMESTATE.GAMEOVER)
                            _programState = PROGRAMSTATE.RETRY;
                        break;
                    case PROGRAMSTATE.RETRY:
                        // ask to return to menu
                        string returnToMenu = "Return to Menu?";
                        _programState = (PROGRAMSTATE)Toolkit.TwoOptionMenu(returnToMenu, (int)_programState, 
                            "Menu", (int)PROGRAMSTATE.START, "Exit", (int)PROGRAMSTATE.END);
                        break;
                    case PROGRAMSTATE.END:
                        Console.Clear();
                        Console.WriteLine("\n     END OF PROGRAM");
                        Console.ReadKey(true);
                        break;
                }
            }

        }
        
    }
}

