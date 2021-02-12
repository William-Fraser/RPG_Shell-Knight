using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    abstract class CHAR_
    {
        protected string _name;
        protected char _avatar;
        protected int _posX;
        protected int _posY;
        protected bool _alive = true;
        
        // ----- gets/sets
        public int getAxisX()
        {
            return _posX;
        }
        public int getAxisY()
        {
            return _posY;
        }
        public bool getAliveStatus() 
        {
            return _alive;
        }
    }
}
