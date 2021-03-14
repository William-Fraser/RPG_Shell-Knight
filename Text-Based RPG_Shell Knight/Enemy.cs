using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <EnemyAvatars>
/// #   - Spider / Health: 20 Attack: 7-17 / runs towards player, runs to walls and along them until player is close then runs at player
/// 
/// %   - Knight / Health: 70 Attack: 9-25 / runs towards player, smart runs around walls similar to spider? 
/// 
/// $   - King / Health: 200 Attack: 1-15 /current Boss, walks back and fourth not strong
/// </icons used to display enemies>

namespace Text_Based_RPG_Shell_Knight
{
    class Enemy : GameCharacter
    {
        //constructor
        public Enemy(string enemyInfo) 
        {
            
            readEnemyInfo(enemyInfo);
        }

        //read info
        private void readEnemyInfo(string enemyInfo) // Constructor child: reads all enemies available to print on map
        {


            string[] avatarAndPos = enemyInfo.Split(':');

                string avatarHold = avatarAndPos[0];
                string posHold = avatarAndPos[1];
                char identity = avatarHold[0];
                string[] identifyed = recognizeInfo(identity).Split(';');
                
            // creating enemy with recognized information
                _avatar = avatarHold[0];
                _name = identifyed[0];
                string[] setHealth = identifyed[1].Split(',');
                string[] setDamage = identifyed[2].Split(',');
                for (int i = 0; i < 2; i++)
                {
                    _health[i] = Int32.Parse(setHealth[i]);
                    _damage[i] = Int32.Parse(setDamage[i]);
                }
                string[] setPos = posHold.Split(',');
                x = Int32.Parse(setPos[0]);
                y = Int32.Parse(setPos[1]);
                aliveInWorld = true;
                _directionMoving = DIRECTION_NULL;

        }// Constructor child: reads all enemies available to print on map
        private string recognizeInfo(char identity) ///holds stats for enemys found in ^EnemyAvatars^
        {
            string identifyed = "";
            if (identity == '#') ///#   - Spider / Health: 20 Attack: 7-17 /
            {
                identifyed += "Spider;";
                identifyed += "20,20;";
                identifyed += "7,17";

            } ///#   - Spider / Health: 20 Attack: 7-17 /
            else if (identity == '%') /// %   - Knight / Health: 70 Attack: 9-25 /
            {
                identifyed += "Knight;";
                identifyed += "70,70;";
                identifyed += "9,25";
            } /// %   - Knight / Health: 70 Attack: 9-25 /
            else if (identity == '$') /// $   - King / Health: 200 Attack: 1-15 /
            {
                identifyed += "King;";
                identifyed += "200,200;";
                identifyed += "1,15";
            } /// $   - King / Health: 200 Attack: 1-15 /
            return identifyed;
        } // read enemy info child

        // ----- public methods

        //AI
        public void MoveChasePlayer(int playerX, int playerY)
        {
            bool moveAlongAxis = false; // if /true = y/ else /false = x/
            int mapQuadrent = 0; // enemy location is 0, 0 /x,-y = 0/x, y = 1/-x, -y = 2/-x, y = 3/ <NW

            if (x == playerX && y == playerY)
            {
                _directionMoving = DIRECTION_NULL;
            }
            else 
            {
                    if (y == playerY)
                    {
                        moveAlongAxis = false;
                    }
                    else if (x == playerX)
                    {
                        moveAlongAxis = true;
                    }
                    else
                    {
                        // finding the quadrent with the player
                        if (playerX > x)
                        {
                            mapQuadrent = 1; // <NE
                            if (playerY > y)
                            {
                                mapQuadrent = 3; // <SE
                            }

                        }
                        else if (playerY > y)
                        {
                            mapQuadrent = 2; // <SW
                        }

                        // check quadrent for which axis to move
                        if (mapQuadrent == 0)
                        {
                            if ((x - playerX) < (y - playerY))
                            {
                                moveAlongAxis = true;
                            }
                            else { moveAlongAxis = false; }
                        }
                        else if (mapQuadrent == 1)
                        {
                            if ((playerX - x) < (playerY - y))
                            {
                                moveAlongAxis = true;
                            }
                            else { moveAlongAxis = false; }
                        }
                        else if (mapQuadrent == 2)
                        {
                            if ((x - playerX) < (playerY - y))
                            {
                                moveAlongAxis = true;
                            }
                            else { moveAlongAxis = false; }
                        }
                        else if (mapQuadrent == 3)
                        {
                            if ((playerX - x) < (playerY - y))
                            {
                                moveAlongAxis = true;
                            }
                            else { moveAlongAxis = false; }
                        }
                    }

                    // check direction to move axis in
                    if (moveAlongAxis == false)
                    {
                        if (playerX > x)
                        {
                            Move(DIRECTION_RIGHT);
                        }
                        else
                        {
                            Move(DIRECTION_LEFT);
                        }
                    }
                    else
                    {
                        if (playerY > y)
                        {
                            Move(DIRECTION_DOWN);
                        }
                        else
                        {
                            Move(DIRECTION_UP);
                        }
                    }

            }
        }

        //update
        public void Update(Player player, Map map, UI ui)
        { 
                ChangetoDealDamage(player.X, player.Y, player.Alive, player.getHealth(), ui);
                MoveChasePlayer(player.X, player.Y);
                CheckForWall(map.getTile(toolkit, _name, X, Y), map.getWallHold());

        }
    }
}
