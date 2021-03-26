using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <EnemyAvatars>
///
/// #   - Spider / Health: 20 / Attack: 7-17 / runs away from player but chases when it's in vacinity
/// 
/// &   - Goblin / Health: 70 / Attack: 9-25 / Chases Player
///
/// %   - Knight / Health: 150 / Attack: 17-40 / Chases player once in radius
/// 
/// $   - King / Health: 275 Attack: 20-50 / Stronger Knight
///
/// </icons used to display enemies, and discriptions of what they should do>

namespace Text_Based_RPG_Shell_Knight
{
    class Enemy : Character
    {
        private int _stateAI;
        private const int AI_CHASE = 0;
        private const int AI_FLEE = 1;
        private const int AI_FLEEANDCHASE = 2;
        private const int AI_IDLEANDCHASE = 3;

        //constructor
        public Enemy(string enemyInfo, string name = "errBlank", char avatar = '!') : base(name, avatar, 0) // starts enemy blank
        {
            ReadEnemyInfo(enemyInfo);
        }

        // ----- private methods
        private bool CheckForCharacterCollision(Player player)
        {
            bool collision;
            collision = base.CheckForCharacterCollision(player.X(), player.Y(), player.AliveInWorld());
            return collision;
        }
        private void DealDamage(Player player, HUD ui, Toolkit toolkit)
        {
                base.DealDamage(player.Name(), player.AliveInWorld(), player.Health(), false, ui, toolkit, player.Shield());
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
                _stateAI = Int32.Parse(identifyed[3]);
                for (int i = 0; i < 2; i++)
                {
                    _health[i] = Int32.Parse(setHealth[i]);
                    _damage[i] = Int32.Parse(setDamage[i]);
                }
                string[] setPos = posHold.Split(',');
                x = Int32.Parse(setPos[0]);
                y = Int32.Parse(setPos[1]);
                aliveInWorld = true;
                int[] _XYHolder = new int[] { 100,100 };
                
                _directionMoving = DIRECTION_NULL;

        }// Constructor child: reads all enemies available to print on map
        private string RecognizeInfo(char identity) ///holds stats for enemys found in ^EnemyAvatars^
        {
            string identifyed = "";
            if (identity == '#') ///#   - Spider / Health: 20 Attack: 7-17 / position / AI WallCling
            {
                identifyed += "Spider;";
                identifyed += "20,20;";
                identifyed += "7,17;";
                identifyed += Enemy.AI_FLEEANDCHASE.ToString();
            }
            else if (identity == '&') /// & - Goblin / Health: 70 / Attack: 9-25 / Chases Player
            {
                identifyed += "Goblin;";
                identifyed += "70,70;";
                identifyed += "9,25;";
                identifyed += Enemy.AI_CHASE.ToString();
            }
            else if (identity == '%') /// %   - Knight / Health: 70 Attack: 9-25 // AI Chase
            {
                identifyed += "Knight;";
                identifyed += "150,150;";
                identifyed += "17,40;";
                identifyed += Enemy.AI_IDLEANDCHASE.ToString();
            }
            else if (identity == '$') /// $   - King / Health: 200 Attack: 1-15 /
            {
                identifyed += "King;";
                identifyed += "275,275;";
                identifyed += "20,50;";
                identifyed += Enemy.AI_IDLEANDCHASE.ToString();
            } /// $   - King / Health: 200 Attack: 1-15 /
            return identifyed;
        } // read enemy info child

        //AI
        private void AIMoveChasePlayer(Player player)
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
                Console.WriteLine();
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
        private void AIMoveFleePlayer(Player player)
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
                        CheckDirection(DIRECTION_LEFT);
                    }
                    else
                    {
                        CheckDirection(DIRECTION_RIGHT);
                    }
                }
                else
                {
                    if (playerY > y)
                    {
                        CheckDirection(DIRECTION_UP);
                    }
                    else
                    {
                        CheckDirection(DIRECTION_DOWN);
                    }
                }

            }
        }
        private void AIMoveFleeThenChaseInProx(Player player)
        {
            int playerX = player.X();
            int playerY = player.Y();

            if (
            (x - playerX) + (y - playerY) >= 10 ||
            (playerX - x) + (y - playerY) >= 10 ||
            (x - playerX) + (playerY - y) >= 10 ||
            (playerX - x) + (playerY - y) >= 10)
            {
                AIMoveFleePlayer(player);
            }
            else 
            { AIMoveChasePlayer(player); }
        }
        private void AIChaseInProx(Player player)
        {
            int playerX = player.X();
            int playerY = player.Y();

            if (
            (x - playerX) + (y - playerY) >= 20 ||
            (playerX - x) + (y - playerY) >= 20 ||
            (x - playerX) + (playerY - y) >= 20 ||
            (playerX - x) + (playerY - y) >= 20)
            {
                x = X();
                y = Y();
            }

            else
            { AIMoveChasePlayer(player); }
        }


        // ----- public methods


        //update
        private void KillIfDead(Camera camera, Map map, HUD hud, int state)
        {
            if (aliveInWorld)
            {
                
                
                KillCharacter(_name, camera, map, hud, state);
            }
        }
            public void Update(Player player, Map map, Camera camera, HUD hud, Toolkit toolkit, int state)
            {
            KillIfDead(camera, map, hud, state);
            if (aliveInWorld)
            {
                switch(_stateAI)
                {
                    case AI_CHASE:
                        AIMoveChasePlayer(player);
                        break;
                    case AI_FLEE:
                        AIMoveFleePlayer(player);
                        break;
                    case AI_FLEEANDCHASE:
                        AIMoveFleeThenChaseInProx(player);
                        break;
                    case AI_IDLEANDCHASE:
                        AIChaseInProx(player);
                        break;
                }

                bool collision = false;
                if (CheckForCharacterCollision(player)) // enemy values read as zero on firstcontact, needs enemy locate to read adjesent tile's
                {
                    collision = true;
                    DealDamage(player, hud, toolkit);
                }

                if (!CheckForWall(map.getTile(_XYHolder[0], _XYHolder[1]), map.getWallHold()))
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
