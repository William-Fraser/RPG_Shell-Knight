using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    class Program
    {
        private static int PROGRAMSTATE_GAMEOVER = 0;
        private static int PROGRAMSTATE_PLAYGAME = 1;
        private static int PROGRAMSTATE_PLAYMENU = 2;
        private static int PROGRAMSTATE_ENDING = 3;
        static void Main(string[] args)
        {
            int _state = PROGRAMSTATE_PLAYGAME;
            GameManager game = new GameManager();

            while (_state != PROGRAMSTATE_ENDING)
            {
                if (_state == PROGRAMSTATE_PLAYMENU)
                { 
                    
                }
                while (_state != PROGRAMSTATE_GAMEOVER)
                {
                    if (_state == PROGRAMSTATE_PLAYGAME)
                    {
                        game.Game();
                        if (game.getGameState() == PROGRAMSTATE_GAMEOVER){
                            _state = PROGRAMSTATE_GAMEOVER;
                        }
                    }
                }
                _state = PROGRAMSTATE_ENDING;
            }
        }
        public void Menu()
        { 
        
        }
    }
}
