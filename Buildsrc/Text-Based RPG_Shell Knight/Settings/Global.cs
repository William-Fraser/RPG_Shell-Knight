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
        public static string PLAYER_DEFAULTNAME = "John Smith";
        public static char PLAYER_AVATAR = '@';
        public static int PLAYER_HEALTH = 100;
        public static int PLAYER_SHIELD = 77;
        public static int[] PLAYER_DAMGERANGE = { 30, 67 };
        public static int[] PLAYER_SPAWNPOINT = { 9, 51 }; // should start player in bed

        //message
        public static string MESSAGE_GAMEOVER = " > GAME OVER < ";

    }
}
