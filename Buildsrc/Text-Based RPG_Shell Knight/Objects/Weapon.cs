using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    public enum WEAPON//its important to note that if iterating through the actual number is off by -1
    {
        FISTS,
        DAGGER,
        SHORTSWORD,
        BROADSWORD,
        LONGSWORD,
        CLAYMORE,
        KALIBURN,
        TOTALWEAPONS // used to quantify enum
    };
    class Weapon : Object
    {
        // current weapons damage range
        private string _avatarWeapon { get; set; }
        private int[] _damageRange;

        // sets first Weapon / usually fists
        public Weapon(WEAPON w)
        {
            IdentifyAndEquip(w);
        }

        // ----- gets/ sets
        new public string Avatar() { return _avatarWeapon; }
        public int[] DamageRange() { return _damageRange; }

        // ----- public methods

        // set item stats when equiping item
        public void IdentifyAndEquip(WEAPON w) // used in Inventory // remodel other object accessors after this
        {
            _name = Global.WEAPON_NAME(w);
            _avatarWeapon = Global.WEAPON_AVATAR(w);
            _damageRange = Global.WEAPON_DAMAGERANGE(w);
        }
    }
}
