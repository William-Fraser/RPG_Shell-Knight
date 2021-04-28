using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    public enum OBJECTS
    {
        ITEM,
        DOOR
    };
    abstract class Object
    {
        //
        //protected Toolkit toolkit = new Toolkit();

        protected int x;
        protected int y;

        protected string _name;
        protected char _avatar;
        protected private bool aliveInWorld = true;

        // ----- gets
        public string Name() { return _name; }
        public char Avatar() { return _avatar; }
        public int X() { return x; }
        public int Y() { return y; }
        public bool AliveInWorld() { return aliveInWorld; }

        // ----- public methods
        public void Draw(Camera camera)
        {
            if (aliveInWorld)
            {
                //Console.SetCursorPosition(1, 1);              //
                //Console.Write($"X: { _posX } Y: { _posY }");  // --- debug / display cursor XY
                camera.GameWorldTile(_avatar, x, y);
            }
        }
    }
}
