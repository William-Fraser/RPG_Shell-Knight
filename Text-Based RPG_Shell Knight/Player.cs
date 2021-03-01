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

            _posX = Console.WindowWidth / 2;
            _posY = Console.WindowHeight / 2;
        }

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
        public bool GethasKey()
        {
            return hasKey;
        }
        public void GetInput()
        {
            _playerInput = Console.ReadKey(true);
        }
        public void GetKey(bool pickupitem)
        {
            hasKey = pickupitem;
        }
        public void Update(int enemyX, int enemyY, char[] map, string walls, bool alive, int[] health, UI ui) {
            //toolkit.DisplayText(toolkit.blank);// clears the text after it's been displayed once - changed
            ChecktoTakeDamage(enemyX, enemyY, alive, health, ui);
            GetInput();
            toolkit.SetConsoleSize();
            directionalOutput();
                    Console.ReadKey(true);
            if (CheckForCharacterCollision(enemyX, enemyY, alive) == true) // enemy values read as zero on firstcontact, needs enemy locate to read adjesent tile's
            { 
                moveBack();
                Console.SetCursorPosition(1, 0);
                Console.Write("Contact");
            }
            CheckForWall(map, walls);
            
        }
    }
}
