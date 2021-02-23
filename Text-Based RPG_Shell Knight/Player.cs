using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    class Player:GameCharacter
    {
        private ConsoleKeyInfo _playerInput;

        //constructor
        public Player(string name, char avatar)
        {
            _name = name;
            _avatar = avatar;
            _health = new int[] { 100, 100 };
            _damage = new int[] { 75, 50 };

            _posX = Console.WindowWidth / 2;
            _posY = Console.WindowHeight / 2;
        }

        // ----- gets / sets
        public int[] getDamage() 
        {
            return _damage;
        }

        // ----- Private Methods
        private void directionalOutput() {
            
            if (_playerInput.Key == ConsoleKey.S || _playerInput.Key == ConsoleKey.DownArrow) {  
                                            Move(DIRECTION_DOWN); 
            }   
            else if (_playerInput.Key == ConsoleKey.W || _playerInput.Key == ConsoleKey.UpArrow) {  
                                            Move(DIRECTION_UP); 
            }
            else if (_playerInput.Key == ConsoleKey.A || _playerInput.Key == ConsoleKey.LeftArrow) { 
                                            Move(DIRECTION_LEFT);
            }
            else if (_playerInput.Key == ConsoleKey.D || _playerInput.Key == ConsoleKey.RightArrow) { 
                                            Move(DIRECTION_RIGHT); 
            }
        }
       
        // ----- Public Methods
        public void GetInput()
        {
            _playerInput = Console.ReadKey(true);
        }
        public void Update() {
            toolkit.DisplayText(toolkit.blank);// clears the text after it's been displayed once
            GetInput();
            toolkit.SetConsoleSize();
            directionalOutput();
        }
    }
}
