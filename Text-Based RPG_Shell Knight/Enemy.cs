using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    class Enemy : GameCharacter
    {
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
        public void MoveChasePlayer(int playerX, int playerY)
        {
            bool moveAlongAxis = false; // if /true = y/ else /false = x/
            int mapQuadrent = 0; // enemy location is 0, 0 /x,-y = 0/x, y = 1/-x, -y = 2/-x, y = 3/ <NW

            if (_posY == playerY)
            {
                moveAlongAxis = false;
            }
            else if (_posX == playerX)
            {
                moveAlongAxis = true;
            }
            else
            {
                // finding the quadrent with the player
                if (playerX > _posX)
                {
                    mapQuadrent = 1; // <NE
                    if (playerY > _posY)
                    {
                        mapQuadrent = 3; // <SE
                    }

                }
                else if (playerY > _posY)
                {
                    mapQuadrent = 2; // <SW
                }

                // check quadrent for which axis to move
                if (mapQuadrent == 0)
                {
                    if ((_posX - playerX) < (_posY - playerY))
                    {
                        moveAlongAxis = true;
                    }
                    else { moveAlongAxis = false; }
                }
                else if (mapQuadrent == 1)
                {
                    if ((playerX - _posX) < (playerY - _posY))
                    {
                        moveAlongAxis = true;
                    }
                    else { moveAlongAxis = false; }
                }
                else if (mapQuadrent == 2)
                {
                    if ((_posX - playerX) < (playerY - _posY))
                    {
                        moveAlongAxis = true;
                    }
                    else { moveAlongAxis = false; }
                }
                else if (mapQuadrent == 3)
                {
                    if ((playerX - _posX) < (playerY - _posY))
                    {
                        moveAlongAxis = true;
                    }
                    else { moveAlongAxis = false; }
                }
            }

            // check direction to move axis in
            if (moveAlongAxis == false)
            {
                if (playerX > _posX)
                {
                    Move(DIRECTION_RIGHT);
                }
                else
                {
                    Move(DIRECTION_LEFT);
                }
            }
            else {
                if (playerY > _posY)
                {
                    Move(DIRECTION_DOWN);
                }
                else
                {
                    Move(DIRECTION_UP);
                }
            }
        }
        public void Update(int playerX, int playerY, char[] mapMovement, string walls, int[] damage)
        {
            ChecktoTakeDamage(playerX, playerY, damage);
            MoveChasePlayer(playerX, playerY);
            CheckForWall(mapMovement, walls);
        }
    }
}
