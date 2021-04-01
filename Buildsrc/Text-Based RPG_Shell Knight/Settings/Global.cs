using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    class Global
    {
        //player
        public static string PLAYER_DEFAULTNAME = "PLAYER";
        public static char PLAYER_AVATAR = '@';
        public static int PLAYER_HEALTH = 100; // the value of the current and max health
        public static int PLAYER_SHIELD = 77; // the value of the current and max shield
        public static int[] PLAYER_DAMGERANGE = { 30, 67 }; // the range of damage the player can do, change to weapon system
        public static int[] PLAYER_SPAWNPOINT = { 9, 71 }; //X and Y  // should start player in house

        //message
        public static string MESSAGE_GAMEOVER = " > GAME OVER < ";

    }
}
