using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
    class TradeMenu
    {

        Camera camera;
        HUD hud;
        Vendor vendorBeingTradedWith;

        public TradeMenu()
        {
            camera = new Camera();
           // hud = new HUD();

        }

        public void Draw()
        {
            hud.Draw();
            camera.DrawBorder();
        }
        public GAMESTATE Begin(Player player, Vendor vendor, Inventory inventory, bool isPlayer = false)
        {
            vendorBeingTradedWith = vendor;
            hud = new HUD(player.Name());
            Console.Clear();
            hud.Update(player, inventory);
            hud.DrawTextBox();
            return GAMESTATE.TRADING;
        }

        public GAMESTATE Update(Player player, GAMESTATE gameState, Item item, Inventory inventory)
        {
            hud.Update(player, inventory);
            

           
            return gameState;
        }

    }
}
