using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    class Global
    { 
        //Characters
        public const char CHARACTER_DEADAVATAR = 'X';

        //player
        public static readonly int[] PLAYER_SPAWNPOINT = { 9, 71 }; //X and Y  // should start player in house
        public static readonly int[] PLAYER_WINPOINT = { 213, 19 }; //X and Y  // should finish the game in the throne
        public const string PLAYER_DEFAULTNAME = "PLAYER"; // playername shoule be no more than 16 chars
        public const char PLAYER_AVATAR = '@';
        public const int PLAYER_HEALTH = 10000; // the value of the current and max health
        public const int PLAYER_SHIELD = 77; // the value of the current and max shield
        public static readonly int[] PLAYER_DAMGERANGE = { 30, 67 }; // the range of damage the player can do, change to weapon system

        //Weapons
        public static string WEAPON_NAME(WEAPON w)          { switch (w) { case WEAPON.FISTS: return "Fists";           case WEAPON.DAGGER: return "Dagger";            case WEAPON.SHORTSWORD: return "Short Sword";       case WEAPON.BROADSWORD: return "Broad Sword";       case WEAPON.LONGSWORD: return "Long Sword";             case WEAPON.CLAYMORE: return "Claymore";         case WEAPON.KALIBURN: return "Kaliburn";               default: return String.Empty; } }
        public static string WEAPON_AVATAR(WEAPON w)        { switch (w) { case WEAPON.FISTS: return "@";               case WEAPON.DAGGER: return "┼──";               case WEAPON.SHORTSWORD: return "─┼═══─";            case WEAPON.BROADSWORD: return "──╬═════─";         case WEAPON.LONGSWORD: return "o────╬■■■▄▄▄▄▄▄■■■■▀▀";  case WEAPON.CLAYMORE: return "├═┼══╣█████████■"; case WEAPON.KALIBURN: return "╔──┼──╬■■█■■■■■■▄▄▄_";   default: return String.Empty; } }
        public static int[] WEAPON_DAMAGERANGE(WEAPON w)    { switch (w) { case WEAPON.FISTS: return fistsRange;        case WEAPON.DAGGER: return daggerRange;         case WEAPON.SHORTSWORD: return shortswordRange;     case WEAPON.BROADSWORD: return broadswordRange;     case WEAPON.LONGSWORD: return longswordRange;           case WEAPON.CLAYMORE: return claymoreRange;      case WEAPON.KALIBURN: return kaliburnRange;            default: return null; } }
                                                                    public static int[] fistsRange = { 5, 10 };  public static int[] daggerRange = { 7, 17 };    public static int[] shortswordRange = { 20, 32 };   public static int[] broadswordRange = { 27, 40 };   public static int[] longswordRange = { 40, 57 };        public static int[] claymoreRange = { 55, 68 };  public static int[] kaliburnRange = { 70, 100 };
        //Items
        public static string ITEM_NAME(ITEM i) { switch (i) { case ITEM.POTHEAL: return "Health Potion"; case ITEM.POTSHELL: return "Shell Banding"; case ITEM.KEYBIG: return "Big Key"; case ITEM.KEYSMALL: return "Small Key"; default: return String.Empty; } }
        public static char ITEM_AVATAR(ITEM i) { switch (i) { case ITEM.POTHEAL: return 'ö'; case ITEM.POTSHELL: return 'ï'; case ITEM.KEYBIG: return 'K'; case ITEM.KEYSMALL: return 'k'; default: return String.Empty[0]; } }
        public static int ITEM_POWER(ITEM i) { switch (i) { case ITEM.POTHEAL: return 50; case ITEM.POTSHELL: return 30; default: return 0; } }

        //messages
            //general
        public const string START_SCREEN_TITLE = "@ SHELL KNIGHT                                    By William.Fr";
        public const string MESSAGE_GAMEOVER = " > GAME OVER < ";
        public const string MESSAGE_PLAYERVICTORY = "The King has been userped, A new Lord has been crowned!";

        // messages below start with characters name and <> is already included /// 90 CHARACTER LIMIT OTHERWISE EXPECT BUGS
            //general
        public const string MESSAGE_SLAIN = "has been slain";
            //items
        public const string MESSAGE_POTHEALTHDRINK = "drinks a Health Potion [+50 HP]";
        public const string MESSAGE_POTHEALTHMISSING = "looked for a HealthPotion but found none";
        public const string MESSAGE_POTSHIELDDRINK = "used some Shell Banding [+30 SP]";
        public const string MESSAGE_POTSHIELDMISSING = "is fresh out of Shell Banding";
        public const string MESSAGE_DOORSMALLOPEN = "opened a small door with a small key";
        public const string MESSAGE_DOORSMALLLOCKED = "tried to open the small door, but it was locked";
        public const string MESSAGE_DOORBIGOPEN = "opened the big door with the big key";
        public const string MESSAGE_DOORBIGLOCKED = "tried to open the big door, but it's sealed shut";
    }
}