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
            //recognize item info
            string[] avatarAndPos = vendorInfo.Split(':');
            string[] recognizedItem = RecognizeInfo(vendorInfo[0]).Split(';'); ;

            //set item fields
            _avatar = vendorInfo[0];
            _name = "Vendor";

            //set position
            string[] posHold = avatarAndPos[1].Split(',');
            int[] posXY = new int[2];
            for (int i = 0; i < posHold.Length; i++)
            { posXY[i] = Int32.Parse(posHold[i]); }
            x = posXY[0];
            y = posXY[1];
        }

        private string RecognizeInfo(char identity)
        {
            string identifyed = "";

            if (identity == 'B') 
            {
                identifyed += "Black Smith;";
                type = Type.BLACKSMITH;
                aliveInWorld = true;

            }
            else if (identity == 'P') 
            {
                identifyed += "Potioneer;";
                type = Type.POTIONEER;
                aliveInWorld = true;

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
