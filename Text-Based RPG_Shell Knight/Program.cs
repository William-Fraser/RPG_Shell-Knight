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
        static void Main()
        {
            int _state = PROGRAMSTATE_PLAYGAME;
            GameManager game = new GameManager();

            while (_state != PROGRAMSTATE_ENDING)
            {
                if (_state == PROGRAMSTATE_PLAYMENU)
                {
                    //Menu();  
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
                _state = PROGRAMSTATE_ENDING;
            }
            //Console.ReadKey(true);
        }
        public void Menu()
        { 
        
        }
    }
}
