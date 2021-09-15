using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    class Vendor : Character

    {
        //different types of vendors
        private enum Type
        {
            BLACKSMITH,
            POTIONEER
        }
        private Type type;

        public Vendor(string vendorInfo, string name = "vendor", char avatar = '!') :base(name, avatar, 0)
        {
            readVendorInfo(vendorInfo);
        }

       
        private void readVendorInfo(string vendorInfo)
        {
            string[] avatarAndPos = vendorInfo.Split(':');
            string avatarHold = avatarAndPos[0];
            string posHold = avatarAndPos[1];

            // reading identity for creating
            char identity = avatarHold[0];
            string[] identifyed = RecognizeInfo(identity).Split(';');

            // creating enemy form identity

            //init of fields
            _avatar = avatarHold[0];
            _name = identifyed[0];
        }

        private string RecognizeInfo(char identity)
        {
            string identifyed = "";

            if (identity == 'B') 
            {
                identifyed += "Black Smith;";
                type = Type.BLACKSMITH;
                
            }
            else if (identity == '&') 
            {
                identifyed += "Potioneer;";
                type = Type.POTIONEER;
               
            }

            return identifyed;
        }

        public GAMESTATE Update(Player player, Map map, Camera camera, HUD hud, Battle battle, Inventory inventory, GAMESTATE gameState)
        {
            //if (aliveInWorld)
            //{
                //switch (_ai)
                //{
                   // case AI.CHASE:
                        //AIMoveChasePlayer(player);
                        //break;
                    //case AI.FLEE:
                        //AIMoveFleePlayer(player);
                       // break;
                    //case AI.FLEEANDCHASE:
                        //AIMoveFleeThenChaseInProx(player);
                        //break;
                    //case AI.IDLEANDCHASE:
                        //AIIdleThenChaseInProx(player);
                        //break;
                //}

                //bool collision = false;
                //if (CheckForCharacterCollision(player.X(), player.Y(), player.AliveInWorld())) // enemy values read as zero on firstcontact, needs enemy locate to read adjesent tile's
                //{
                    //collision = true;
                    //gameState = StartAttacking(aliveInWorld, battle, player, this, gameState, inventory);
                //}

                //if (!CheckForWallCollision(map.getTile(_XYHolder[0], _XYHolder[1] - 1), map.getWallHold())) // -1 to fix bug from result of other fix
                //{
                    //if (!collision)
                    //{
                       // Move();
                   // }
               // }
                //CheckForDying(camera, hud);
            //}
            return gameState;
        }




    }
}
