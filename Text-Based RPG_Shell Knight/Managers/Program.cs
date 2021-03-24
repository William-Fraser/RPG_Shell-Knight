using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    class Program
    {
        private static readonly int PROGRAMSTATE_GAMEOVER = 0;
        private static readonly int PROGRAMSTATE_PLAYGAME = 1;
        private static readonly int PROGRAMSTATE_PLAYMENU = 2;
        private static readonly int PROGRAMSTATE_ENDING = 3;

        // options outside gameplay
        private static ConsoleKeyInfo decision;

        static void Main()
        {
            int _state = PROGRAMSTATE_PLAYMENU;
            GameManager game = new GameManager();

            while (_state != PROGRAMSTATE_ENDING)
            {
                while (_state == PROGRAMSTATE_PLAYMENU)
                {
                    Console.WriteLine();
                    Console.WriteLine("SHELL KNIGHT \n   S - Start\n   E - Exit");
                    decision = Console.ReadKey(true);

                    if (decision.Key == ConsoleKey.S)
                    {
                        Console.Clear();
                        _state = PROGRAMSTATE_PLAYGAME;
                    }

                    else if (decision.Key == ConsoleKey.N)
                    {
                        Console.Clear();
                        _state = PROGRAMSTATE_GAMEOVER;
                    }

                    else
                    {
                        Console.WriteLine("please select a statement");
                        Console.ReadKey(true);
                        Console.Clear();
                        _state = PROGRAMSTATE_PLAYMENU;
                    }
                }
                while (_state != PROGRAMSTATE_GAMEOVER)
                {
                    if (_state == PROGRAMSTATE_PLAYGAME)
                    {
                        game.Game();
                        if (game.GameState() == PROGRAMSTATE_GAMEOVER)
                        {
                            game.Game(); // extra game to display GAMEOVER TEXT
                            _state = PROGRAMSTATE_GAMEOVER;
                        }
                    }
                }
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("Continue Game? \n   Y - Continue \n   N - Exit");
                decision = Console.ReadKey(true);

                if (decision.Key == ConsoleKey.Y)
                {
                    Console.Clear();
                    _state = PROGRAMSTATE_PLAYGAME;
                }

                else if (decision.Key == ConsoleKey.N)
                {
                    Console.Clear();
                    _state = PROGRAMSTATE_ENDING;
                }

                else
                {
                    Console.WriteLine("please select a statement");
                    Console.ReadKey(true);
                    Console.Clear();
                }
            }
            //Console.ReadKey(true);
        }
        public void MainMenu()
        { 
        
        }
    }
}
