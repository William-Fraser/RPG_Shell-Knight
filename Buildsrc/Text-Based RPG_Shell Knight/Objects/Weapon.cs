using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    class Weapon : Object
    {
        // current weapons damage range
        private string _avatarWeapon { get; set; }
        private int[] _damageRange;

        // sets first Weapon / usually fists
        public Weapon(Global.WEAPON w)
        {
            IdentifyEquip(w);
        }

        // ----- gets/ sets
        public string avatarWeapon() { return _avatarWeapon; }
        public int[] damageRange() { return _damageRange; }

        // ----- private methods

        // set item stats when equiping item
        private void IdentifyEquip(Global.WEAPON w) // used in Inventory
        {
            _name = Global.WEAPON_NAME(w);
            _avatarWeapon = Global.WEAPON_AVATAR(w);
            _damageRange = Global.WEAPON_DAMAGERANGE(w);
        }
    }
}
