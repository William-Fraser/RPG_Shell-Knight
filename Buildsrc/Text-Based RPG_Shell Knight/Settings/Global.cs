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
        public static int[] PLAYER_SPAWNPOINT = { 9, 71 }; //X and Y  // should start player in house
        public static int[] PLAYER_WINPOINT = { 213, 19 }; //X and Y  // should finish the game in the throne
        public static string PLAYER_DEFAULTNAME = "PLAYER"; // playername shoule be no more than 16 chars
        public static char PLAYER_AVATAR = '@';
        public static int PLAYER_HEALTH = 100; // the value of the current and max health
        public static int PLAYER_SHIELD = 77; // the value of the current and max shield
        public static int[] PLAYER_DAMGERANGE = { 30, 67 }; // the range of damage the player can do, change to weapon system

        //Characters
        public static char CHARACTER_DEADAVATAR = 'X';

        //message
        public static string MESSAGE_GAMEOVER = " > GAME OVER < ";

        // messages below start with characters name and <> is already included /// 90 CHARACTER LIMIT OTHERWISE EXPECT BUGS
        public static string MESSAGE_SLAIN = "has been slain";
        public static string MESSAGE_POTHEALTHDRINK = "drinks a Health Potion [+50 HP]";
        public static string MESSAGE_POTHEALTHMISSING = "looked for a HealthPotion but found none";
        public static string MESSAGE_POTSHIELDDRINK = "used some Shell Banding [+30 SP]";
        public static string MESSAGE_POTSHIELDMISSING = "is fresh out of Shell Banding";
        public static string MESSAGE_DOORSMALLOPEN = "opened a small door with a small key";
        public static string MESSAGE_DOORSMALLLOCKED = "tried to open the small door, but it was locked";
        public static string MESSAGE_DOORBIGOPEN = "opened the big door with the big key";
        public static string MESSAGE_DOORBIGLOCKED = "tried to open the big door, but it's sealed shut";
        public static string MESSAGE_PLAYERVICTORY = "The King has been userped, A new Lord has been crowned!";
    }
}