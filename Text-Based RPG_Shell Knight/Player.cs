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
        private string DirectionMoved;
        //constructor
        public Player(string name, char avatar) {
            _name = name;
            _avatar = avatar;
            _posX = Console.WindowWidth / 2;
            _posY = Console.WindowHeight / 2;
        }

        // ----- Private Methods
        
        private void DirectionalOutput() {
            if (_playerInput.Key == ConsoleKey.W) { _posY -= 1; DirectionMoved = "UP"; }
            else if (_playerInput.Key == ConsoleKey.S) { _posY += 1; DirectionMoved = "DOWN"; }
            else if (_playerInput.Key == ConsoleKey.A) { _posX -= 1; DirectionMoved = "LEFT"; }
            else if (_playerInput.Key == ConsoleKey.D) { _posX += 1; DirectionMoved = "RIGHT"; }
        }

        // ----- Public Methods

        public void GetInput()
        {
            _playerInput = Console.ReadKey(true);
        }

        public void Draw() {
            Console.SetCursorPosition(_posX, _posY);
            Console.Write(_avatar);
        }

        public void Update() {
            GetInput();
            DirectionalOutput();
        }
    }
}
