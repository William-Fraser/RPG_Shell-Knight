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

        // ----- gets/sets
        public int getAxisX()
        {
            return _posX;
        }
        public int getAxisY()
        {
            return _posY;
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
        public void CheckForWall(bool wall)
        {
            if (wall)
            {
                if (_directionMoving == "UP") { _posY++; }
                else if (_directionMoving == "DOWN") { _posY--; }
                else if (_directionMoving == "LEFT") { _posX++; }
                else if (_directionMoving == "RIGHT") { _posX--; }
            }
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
