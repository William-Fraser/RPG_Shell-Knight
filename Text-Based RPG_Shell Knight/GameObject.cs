using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    abstract class GameObject
    {
        protected int _posX;
        protected int _posY;

        protected string _name;
        protected char _avatar;
        protected private bool aliveInWorld = true;

        // ----- gets / sets
        public string getName()
        {
            return _name;
        }
        public bool getAlive()
        {
            return aliveInWorld;
        }

        // ----- public methods
        public void Draw()
        {
            if (aliveInWorld)
            {
                //Console.SetCursorPosition(1, 1);              //
                //Console.Write($"X: { _posX } Y: { _posY }");  // --- debug / display cursor XY
                Console.SetCursorPosition(_posX, _posY);
                Console.Write(_avatar);
            }
        }
    }
}
