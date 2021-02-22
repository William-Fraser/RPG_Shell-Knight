using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    class Enemy : GameCharacter
    {
        int idle_movement_timer = 0;
        int idle_movement_direction = 0;

        //constructor
        public Enemy(string name, char avatar, int pos) 
        {
            _name = name;
            _avatar = avatar;
            _health = new int[] { 100, 100 };
            _damage = new int[] { 65, 50 };

            setEnemyPos(pos);
        }

        // ----- gets / sets
        public int[] getDamage()
        {
            return _damage;
        }
        public void setEnemyPos(int pos) 
        {
            if (pos == 0) {
                _posX = Console.WindowWidth / 3;
                _posY = Console.WindowHeight / 3;
            }
            else if (pos == 1)
            {
                _posX = (Console.WindowWidth / 4) * 3;
                _posY = Console.WindowHeight / 4;
            }
            else if (pos == 2)
            {
                _posX = Console.WindowWidth / 4;
                _posY = (Console.WindowHeight / 4) * 3;
            }
            else if (pos == 3)
            {
                _posX = (Console.WindowWidth / 4) * 3;
                _posY = (Console.WindowHeight / 4) * 3;
            }
        }

        // ----- public methods
        public void idleMove()
        {
            idle_movement_timer++;
            if (idle_movement_timer >= 2)
            {

                idle_movement_timer = 0;

                if (idle_movement_direction == 0) { _posX++; idle_movement_direction++; }
                else if (idle_movement_direction == 1) { _posY++; idle_movement_direction++; }
                else if (idle_movement_direction == 2) { _posX--; idle_movement_direction++; }
                else if (idle_movement_direction == 3) { _posY--; idle_movement_direction = 0; }

            }
        }
    }
}
