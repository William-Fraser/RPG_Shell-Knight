using System;
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
        private ItemValues itemValues;

        //get/set
        public EnemyValues EnemyValues { set { enemyValues = value; } }
        public WeaponValues WeaponValues { set { weaponValues = value; } }
        public ItemValues ItemValues { get{ return itemValues; } set{ itemValues = value; } }


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
        public static string ITEM_NAME(string iID) { return Global.globalAccess.itemValues.names[Global.globalAccess.itemIDs[iID]]; }
        public static char ITEM_AVATAR(string iID) { return Global.globalAccess.itemValues.avatars[Global.globalAccess.itemIDs[iID]]; }
        public static int ITEM_POWER(string iID) { return Global.globalAccess.itemValues.powers[Global.globalAccess.itemIDs[iID]]; }
        public static string ITEM_TEXTDESC(string iID) { return Global.globalAccess.ItemValues.desc[Global.globalAccess.itemIDs[iID]]; }
        public static string ITEM_TEXTSUCCESS(string iID) { return Global.globalAccess.itemValues.textSuccess[Global.globalAccess.itemIDs[iID]]; }
        public static string ITEM_TEXTFAILURE(string iID) { return Global.globalAccess.itemValues.textFailure[Global.globalAccess.itemIDs[iID]]; }

        //general
        public const char CHARACTER_DEADAVATAR = 'X';
        // messages below start with characters name and <> is already included /// 90 CHARACTER LIMIT OTHERWISE EXPECT BUGS
        public const string MESSAGE_SLAIN = "has been slain";
            //items
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
    class ItemValues
    {
        public List<string> names = new List<string>();
        public List<char> avatars = new List<char>();
        public List<string> effects = new List<string>();
        public List<int> powers = new List<int>();
        public List<string> desc = new List<string>();
        public List<string> textSuccess = new List<string>();
        public List<string> textFailure = new List<string>();
    }
}