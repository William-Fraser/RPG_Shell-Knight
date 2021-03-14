using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    class Player : GameCharacter
    {
        private ConsoleKeyInfo _playerInput;
        private bool hasKey = false;

        //constructor
        public Player(string name, char avatar)
        {
            _name = name;
            _avatar = avatar;
            _health = new int[] { 999, 999 };
            _damage = new int[] { 50, 75 };

            x = Console.WindowWidth / 2;
            y = Console.WindowHeight / 2;
        }

        // ----- gets sets
        public bool HasKey { get; set; }
        
        // ----- Private Methods
        private void directionalOutput() {

            if (_playerInput.Key == ConsoleKey.S || _playerInput.Key == ConsoleKey.DownArrow)
            {
                Move(DIRECTION_DOWN);
            }
            else if (_playerInput.Key == ConsoleKey.W || _playerInput.Key == ConsoleKey.UpArrow)
            {
                Move(DIRECTION_UP);
            }
            else if (_playerInput.Key == ConsoleKey.A || _playerInput.Key == ConsoleKey.LeftArrow)
            {
                Move(DIRECTION_LEFT);
            }
            else if (_playerInput.Key == ConsoleKey.D || _playerInput.Key == ConsoleKey.RightArrow)
            {
                Move(DIRECTION_RIGHT);
            }
            else
            { _directionMoving = DIRECTION_NULL; }
        }
        
        // ----- Public Methods
        public void GetInput()
        {
            _playerInput = Console.ReadKey(true);
        }
        
        //  map.getWallHold(), enemy.getAlive(), enemy.getDamage(player.X(),player.Y()), ui
        public void Update(Enemy enemy, Map map, UI ui) {
            //toolkit.DisplayText(toolkit.blank);// clears the text after it's been displayed once - changed
            GetInput();
            toolkit.SetConsoleSize();
            directionalOutput();
                    Console.ReadKey(true);
            if (CheckForCharacterCollision(enemy.X, enemy.Y, enemy.Alive == true)) // enemy values read as zero on firstcontact, needs enemy locate to read adjesent tile's
            { 
                MoveBack();
                Console.SetCursorPosition(1, 0);
                Console.Write("Contact");
            }
            CheckForWall(map.getTile(toolkit, _name, X, Y), map.getWallHold());
            
        }
    }
}
