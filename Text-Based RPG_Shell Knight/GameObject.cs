using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    abstract class GameObject
    {
        //
        protected Toolkit toolkit = new Toolkit();

        protected int x;
        protected int y;

        protected string _name;
        protected char _avatar;
        protected private bool aliveInWorld = true;

        // ----- gets / sets
        public string Name { get; }
        public bool Alive { get; }

        // ----- public methods
        public void Draw()
        {
            if (aliveInWorld)
            {
                //Console.SetCursorPosition(1, 1);              //
                //Console.Write($"X: { _posX } Y: { _posY }");  // --- debug / display cursor XY
                Console.SetCursorPosition(x, y);
                Console.Write(_avatar);
            }
        }
    }
}
