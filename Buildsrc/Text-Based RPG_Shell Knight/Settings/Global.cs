﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    enum AXIS 
    { 
        X,
        Y
    } //Z
    class Global
    {
        // global reference
        public static Global globalAccess = new Global(); // this is used to access important information loaded from files

        //dictionaries
        public IDictionary<string, int> enemyIDs = new Dictionary<string, int>();
        public IDictionary<string, int> weaponIDs = new Dictionary<string, int>();
        public IDictionary<string, int> itemIDs = new Dictionary<string, int>();

        //values
        private EnemyValues enemyValues;
        private WeaponValues weaponValues;
        //private ItemValues itemValues;

        //get/set
        public EnemyValues EnemyValues { set { enemyValues = value; } }
        public WeaponValues WeaponValues { set { weaponValues = value; } }
        //public ItemValues ItemValues { get; set; }


        // values are string to pertain to loading order from DataLoader, they are parameters for a ID Dictionary

        //player
        public static readonly int[] PLAYER_SPAWNPOINT = { 9, 71 }; //X and Y  // should start player in house
        public static readonly int[] PLAYER_WINPOINT = { 213, 19 }; //X and Y  // should finish the game in the throne
        public const string PLAYER_DEFAULTNAME = "PLAYER"; // playername shoule be no more than 16 chars
        public const char PLAYER_AVATAR = '@';
        public const int PLAYER_HEALTH = 100; // the value of the current and max health
        public const int PLAYER_SHIELD = 77; // the value of the current and max shield
        public static readonly int[] PLAYER_DAMGERANGE = { 30, 67 }; // the range of damage the player can do, change to weapon system

        //Weapons
        public static string WEAPON_NAME(string wID) { return globalAccess.weaponValues.names[globalAccess.weaponIDs[wID]]; }
        public static string WEAPON_AVATAR(string wID) { return globalAccess.weaponValues.avatars[globalAccess.weaponIDs[wID]]; }
        public static int[] WEAPON_DAMAGERANGE(string wID) { return globalAccess.weaponValues.damages[globalAccess.weaponIDs[wID]]; }

        //Enemy 
        public static string GetEnemyName(string eID) { return Global.globalAccess.enemyValues.names[Global.globalAccess.enemyIDs[eID]]; }
        public static char GetEnemyAvatar(string eID) { return Global.globalAccess.enemyValues.avatars[Global.globalAccess.enemyIDs[eID]]; }
        public static int GetEnemyHealth(string eID) { return Global.globalAccess.enemyValues.healths[Global.globalAccess.enemyIDs[eID]]; }
        public static int[] GetEnemyDamageRange(string eID) { return Global.globalAccess.enemyValues.damages[Global.globalAccess.enemyIDs[eID]]; }
        public static AI GetEnemyEnemyAI(string eID) { return Global.globalAccess.enemyValues.aIs[Global.globalAccess.enemyIDs[eID]]; }

        //messages
        //general
        public const string START_SCREEN_TITLE = "@ SHELL KNIGHT                                    By William.Fr";
        public const string MESSAGE_GAMEOVER = " > GAME OVER < ";
        public const string MESSAGE_PLAYERVICTORY = "The King has been userped, A new Lord has been crowned!";
        
        //Items
        public static string ITEM_NAME(ITEM i) { switch (i) { case ITEM.POTHEAL: return "Health Potion"; case ITEM.POTSHELL: return "Shell Banding"; case ITEM.KEYBIG: return "Big Key"; case ITEM.KEYSMALL: return "Small Key"; default: return String.Empty; } }
        public static char ITEM_AVATAR(ITEM i) { switch (i) { case ITEM.POTHEAL: return 'ö'; case ITEM.POTSHELL: return 'ï'; case ITEM.KEYBIG: return 'K'; case ITEM.KEYSMALL: return 'k'; default: return String.Empty[0]; } }
        public static int ITEM_POWER(ITEM i) { switch (i) { case ITEM.POTHEAL: return 50; case ITEM.POTSHELL: return 30; default: return 0; } }
        
        //general
        public const char CHARACTER_DEADAVATAR = 'X';
        // messages below start with characters name and <> is already included /// 90 CHARACTER LIMIT OTHERWISE EXPECT BUGS
        public const string MESSAGE_SLAIN = "has been slain";
        public const string MESSAGE_POTHEALTHDRINK = "drinks a Health Potion";
        public const string MESSAGE_POTHEALTHMISSING = "looked for a HealthPotion but found none";
        public const string MESSAGE_POTSHIELDDRINK = "used some Shell Banding";
        public const string MESSAGE_POTSHIELDMISSING = "is fresh out of Shell Banding";
        public const string MESSAGE_DOORSMALLOPEN = "opened a small door with a small key";
        public const string MESSAGE_DOORSMALLLOCKED = "tried to open the small door, but it was locked";
        public const string MESSAGE_DOORBIGOPEN = "opened the big door with the big key";
        public const string MESSAGE_DOORBIGLOCKED = "tried to open the big door, but it's sealed shut";
        
    }
    class EnemyValues
    {
        public List<string> names = new List<string>();
        public List<char> avatars = new List<char>();
        public List<int> healths = new List<int>();
        public List<int[]> damages = new List<int[]>();
        public List<AI> aIs = new List<AI>();
    }
    class WeaponValues
    {
        public List<string> names = new List<string>();
        public List<string> avatars = new List<string>();
        public List<int[]> damages = new List<int[]>();
    }
    /*class ItemValues
    {
        public List<string> names = new List<string>();
        public List<char> avatars = new List<char>();
        public List<string> effects = new List<string>();
        public List<int> powers = new List<int>();
        public List<string> desc = new List<string>();
        public List<string> textSuccess = new List<string>();
        public List<string> textFailure = new List<string>();
    }*/
}