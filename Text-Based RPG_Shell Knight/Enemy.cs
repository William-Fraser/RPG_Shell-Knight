using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    class Enemy : CHAR_
    {
        int idle_movement_timer = 0;
        int idle_movement_direction = 0;
        public Enemy(string name, char avatar, int pos) 
        {
            _name = name;
            _avatar = avatar;
            setEnemyPos(pos);
        }
        public void setEnemyPos(int pos) 
        {
            if (pos == 0) {
                _posX = Console.WindowWidth / 4;
                _posY = Console.WindowHeight / 4;
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
        public void Draw()
        {
            //Console.SetCursorPosition(1, 1);              //
            //Console.Write($"X: { _posX } Y: { _posY }");  // --- debug / display cursor XY
            Console.SetCursorPosition(_posX, _posY);
            Console.Write(_avatar);
        }
        public void idleMove()
        {
            idle_movement_timer++;
            if (idle_movement_timer <= 2)
            {

                idle_movement_timer = 0;

                if (idle_movement_direction == 0) { _posX++; idle_movement_direction++; }
                else if (idle_movement_direction == 1) { _posY++; idle_movement_direction++; }
                else if (idle_movement_direction == 2) { _posX--; idle_movement_direction++; }
                else if (idle_movement_direction == 3) { _posY--; idle_movement_direction = 0; }

            }
        }
        public bool attack(int playerX, int playerY) 
        {
            
            if (playerX == _posX) {
                if (playerY == _posY) {
                    return true;
                }
            }
            return false;
        }
        public void AttackedifAlive(bool attack)
        { // if bool false = enemy died else enemy lives
            if (attack)
            {
                Console.SetCursorPosition(1, Console.WindowHeight - 2);
                Console.WriteLine($"Enemy {_name} has been slain");
                _alive = false;
                Console.ReadKey();
            }
            else { }
        }
    }
}
