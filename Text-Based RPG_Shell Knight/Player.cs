using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    class Player:CHAR_
    {
        private ConsoleKeyInfo _playerInput;
        private string _directionMoving;

        //constructor
        public Player(string name, char avatar)
        {
            _name = name;
            _avatar = avatar;
            _posX = Console.WindowWidth / 2;
            _posY = Console.WindowHeight / 2;

        }

        // ----- Private Methods
        private void directionalOutput() {
            
            if (_playerInput.Key == ConsoleKey.W) { if (_posY > 0) { _posY -= 1; _directionMoving = "UP"; } }
            else if (_playerInput.Key == ConsoleKey.S) { if (_posY < Console.WindowHeight-1) { _posY += 1; _directionMoving = "DOWN"; } }
            else if (_playerInput.Key == ConsoleKey.A) { if (_posX > 0) { _posX -= 1; _directionMoving = "LEFT"; } }
            else if (_playerInput.Key == ConsoleKey.D) { if (_posX < Console.WindowWidth-1) { _posX += 1; _directionMoving = "RIGHT"; } }
        }
       
        // ----- Public Methods
        public void GetInput()
        {
            _playerInput = Console.ReadKey(true);
        }
        public void CheckForWall(string map, string walls)
        {
            string[] wallGroup = walls.Split();
            for (int i = 0; i < wallGroup.Length; i++) {
                if (map == wallGroup[i]) {
                    if (_directionMoving == "UP") { _posY++; }
                    else if (_directionMoving == "DOWN") { _posY--; }
                    else if (_directionMoving == "LEFT") { _posX++; }
                    else if (_directionMoving == "RIGHT") { _posX--; }
                }
            }
        }
        public void AttackedifAlive(bool attack) { // if bool false = player died else player lives
            if (attack) {
                Console.SetCursorPosition(1, Console.WindowHeight - 2);
                Console.WriteLine($"Player {_name} has been slain");
                _alive = false;
                Console.ReadKey();
            }
            else { }
        }
        public bool attack(int enemyX, int enemyY)
        {
            if (enemyX == _posX)
            {
                if (enemyY == _posY)
                {
                    return true;
                }
            }
            return false;
        }
        public void Draw() {
            //Console.SetCursorPosition(1, 1);              //
            //Console.Write($"X: { _posX } Y: { _posY }");  // --- debug / display cursor XY
            Console.SetCursorPosition(_posX, _posY);      
            Console.Write(_avatar);
        }
        public void Update() {
            GetInput();
            directionalOutput();
        }
    }
}
