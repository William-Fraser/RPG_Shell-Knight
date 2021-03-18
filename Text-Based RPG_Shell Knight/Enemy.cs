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
/// </icons used to display enemies, and discriptions of what they should do>

namespace Text_Based_RPG_Shell_Knight
{
    class Enemy : Character
    {
        //constructor
        public Enemy(string enemyInfo, string name = "errBlank", char avatar = '!') : base(name, avatar, 0) // starts enemy blank
        {
            ReadEnemyInfo(enemyInfo);
        }

        //read info
        private void ReadEnemyInfo(string enemyInfo) // Constructor child: reads all enemies available to print on map
        {


            string[] avatarAndPos = enemyInfo.Split(':');

                string avatarHold = avatarAndPos[0];
                string posHold = avatarAndPos[1];
                char identity = avatarHold[0];
                string[] identifyed = RecognizeInfo(identity).Split(';');
                
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
        private string RecognizeInfo(char identity) ///holds stats for enemys found in ^EnemyAvatars^
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
        public bool CheckForCharacterCollision(Player player)
        {
            bool collision;
            collision = base.CheckForCharacterCollision(player.X(), player.Y(), player.AliveInWorld());
            return collision;
        }
        public void DealDamage(Player player, UI ui, Toolkit toolkit)
        {
                base.DealDamage(player.Name(), player.AliveInWorld(), player.Health(), false, ui, toolkit, player.Shield());
        }
        //AI
        public void AIMoveChasePlayer(Player player)
        {
            int playerX = player.X();
            int playerY = player.Y();

            bool moveYAlongAxis = false; // if /true = y/ else /false = x/
            int quadrent = 0; // creates a grid with enemy location at 0,0 //x,-y = 0//x, y = 1//-x, -y = 2//-x, y = 3//  0 <NW compass orientation to help explain

            if (x == playerX && y == playerY)
            {
                _directionMoving = DIRECTION_NULL;
                _XYHolder[0] = x;
                _XYHolder[1] = y;
            }
            else
            {
                if (y == playerY)
                {
                    moveYAlongAxis = false;
                }
                else if (x == playerX)
                {
                    moveYAlongAxis = true;
                }
                else
                {
                    // finding the quadrent with the player
                    if (playerX > x)
                    {
                        quadrent = 1; // <NE
                        if (playerY > y)
                        {
                            quadrent = 3; // <SE
                        }

                    }
                    else if (playerY > y)
                    {
                        quadrent = 2; // <SW
                    }

                    // check quadrent for which axis to move // note in this calc enemy is not 0,0 yet
                    if (quadrent == 0)
                    {
                        if ((x - playerX) <= (y - playerY))
                        {
                            moveYAlongAxis = true;
                        }
                        else { moveYAlongAxis = false; }
                    }
                    else if (quadrent == 1)
                    {
                        if ((playerX - x) <= (y - playerY))
                        {
                            moveYAlongAxis = true;
                        }
                        else { moveYAlongAxis = false; }
                    }
                    else if (quadrent == 2)
                    {
                        if ((x - playerX) <= (playerY - y))
                        {
                            moveYAlongAxis = true;
                        }
                        else { moveYAlongAxis = false; }
                    }
                    else if (quadrent == 3)
                    {
                        if ((playerX - x) <= (playerY - y))
                        {
                            moveYAlongAxis = true;
                        }
                        else { moveYAlongAxis = false; }
                    }
                }

                // check direction to move axis in
                if (moveYAlongAxis == false)
                {
                    if (playerX > x)
                    {
                        CheckDirection(DIRECTION_RIGHT);
                    }
                    else
                    {
                        CheckDirection(DIRECTION_LEFT);
                    }
                }
                else
                {
                    if (playerY > y)
                    {
                        CheckDirection(DIRECTION_DOWN);
                    }
                    else
                    {
                        CheckDirection(DIRECTION_UP);
                    }
                }

            }
        }

        //update
        public void Update(Player player, Map map, UI ui, Toolkit toolkit)
        {
            KillIfDead(toolkit);
            if (aliveInWorld)
            {
                AIMoveChasePlayer(player);

                bool collision = false;
                if (CheckForCharacterCollision(player)) // enemy values read as zero on firstcontact, needs enemy locate to read adjesent tile's
                {
                    collision = true;
                    DealDamage(player, ui, toolkit);
                }

                if (!CheckForWall(map.getTile(_XYHolder[0], _XYHolder[1] - 1), map.getWallHold()))
                {
                    if (!collision)
                    {
                        Move();
                    }
                }
            }
        }
    }
}
