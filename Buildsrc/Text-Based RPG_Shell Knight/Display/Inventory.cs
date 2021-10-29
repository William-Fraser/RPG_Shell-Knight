using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Text_Based_RPG_Shell_Knight
{

	class Inventory
	{
		///stretch goals: scrolling inventory for larger implementation

		///init fields
		//create inventory hud
		HUD hud;


		//these ints are to keep track of exactly how many items are buffed and which are not.....
		public int buffedHealthPotions;
		public int buffedShellHeal;


		private GAMESTATE returnto;

		//border dimensions
		readonly int height = Camera.displayHeight;
		readonly int width = Camera.displayWidth;

		//holds the inventory border
		string[,] background;

		//moving the cursor
		ConsoleKeyInfo menuInput;
		int cursorPos = 0;
		readonly char cursor = '>';

		readonly int[,] itemPos;
		readonly List<int[]> weaponPos = new List<int[]>();
		readonly int maxObjects;
		
		private int[] stockItems; // holds the amount of Items aquired
		private bool[] stockWeapons; // holds what Weapons have been aquired // player currently has all weapons

		public bool[] StockWeapons { get; set; }


		//constructor
		public Inventory(Player player) // creates the size of the inventory
		{
			//inventory object positions, changed in SetPosition() 
			int[,] createItemPos = { /// these can be any item, changed in SetPosition()
			{10, 5 }, {10, 9 }, {10, 13 }, {10, 17 },
			{25, 5 }, {25, 9 }, {25, 13 }, {25, 17 },
			{40, 5 }, {40, 9 }, {40, 13 }, {40, 17 },
			{55, 5 }, {55, 9 }, {55, 13 }, {55, 17 },
			{70, 5 }, {70, 9 }, {70, 13 }, {70, 17 } };
			itemPos = createItemPos;

			int j = 5;
			for (int i = 0; i < Global.globalAccess.weaponIDs.Count; i++)
			{
				j += 2;
				int[] createdWeaponPos = { Camera.displayWidth - Global.WEAPON_AVATAR(i.ToString()).Length - 3, j };
				weaponPos.Add(createdWeaponPos);
			}

			hud = new HUD(player.Name());
			background = new string[height, width];

			stockItems = new int[(int)ITEM.TOTALITEMS];
			for (ITEM i = 0; i < ITEM.TOTALITEMS; i++)
			{
				stockItems[(int)i] = 0;
			}

			stockWeapons = new bool[(int)WEAPON.TOTALWEAPONS];
			stockWeapons[0] = true;

			maxObjects = 27 - 1; // -1 to account for starting count at 0 / used in navigation
								 // 27 is the number of inventory spaces available to cycle through

			CreateBackground();
		}

		// ----- gets/ sets
		public int[] ItemStock()// get
		{
			return stockItems;
		}

		public bool ItemIsAvailable(string ID)
        {
			if (stockItems[Int32.Parse(ID)] <= 0) { return false; }
            else { return true; }
        }
		public void IncreaseStock(string ID)// FindItem Child
		{
			stockItems[Int32.Parse(ID)]++;
		}

		// ----- private methods

		#region Display Methods
		private void SetPosition(ITEM item) //DrawObj Child / HOLDS ITEM DISPLAY POSITIONS /// fill index from least to greatest, if a number is skipped the cursor wont find it however it will still display
		{
			//second value is always static, use pos to chose position /
			int pos;
			int fsv = 0; // first static value
			int ssv = 1; // second static value, for easier reading
			switch (item)
			{
				case ITEM.POTHEAL:
					pos = 0;
					Console.SetCursorPosition(itemPos[pos, fsv], itemPos[pos, ssv]);
					break;
				case ITEM.POTSHELL:
					pos = 1;
					Console.SetCursorPosition(itemPos[pos, fsv], itemPos[pos, ssv]);
					break;
				case ITEM.KEYBIG:
					pos = 4;
					Console.SetCursorPosition(itemPos[pos, fsv], itemPos[pos, ssv]);
					break;
				case ITEM.KEYSMALL:
					pos = 5;
					Console.SetCursorPosition(itemPos[pos, fsv], itemPos[pos, ssv]);
					break;
				default: break;
			}
			
		}
		private void SetPosition(string ID) //DrawObj Child / holds weapon display positions // dont touch unless editing weapons or inventory big time
		{
			/*int pos; // height of weapons on list, Heirarchy: 0 = Top
			int fsv = 0; // first static value
			int ssv = 1; */// second static value, for easier reading

			int[] setPos = weaponPos[Global.globalAccess.weaponIDs[ID]];
			Console.SetCursorPosition(setPos[(int)AXIS.X], setPos[(int)AXIS.Y]);
			/*switch (weapon)
			{
				case WEAPON.FISTS:
					pos = 0;
					Console.SetCursorPosition(weaponPos[pos, fsv], weaponPos[pos, ssv]);
					break;
				case WEAPON.DAGGER:
					pos = 1;
					Console.SetCursorPosition(weaponPos[pos, fsv], weaponPos[pos, ssv]);
					break;
				case WEAPON.SHORTSWORD:
					pos = 2;
					Console.SetCursorPosition(weaponPos[pos, fsv], weaponPos[pos, ssv]);
					break;
				case WEAPON.BROADSWORD:
					pos = 3;
					Console.SetCursorPosition(weaponPos[pos, fsv], weaponPos[pos, ssv]);
					break;
				case WEAPON.LONGSWORD:
					pos = 4;
					Console.SetCursorPosition(weaponPos[pos, fsv], weaponPos[pos, ssv]);
					break;
				case WEAPON.CLAYMORE:
					pos = 5;
					Console.SetCursorPosition(weaponPos[pos, fsv], weaponPos[pos, ssv]);
					break;
				case WEAPON.KALIBURN:
					pos = 6;
					Console.SetCursorPosition(weaponPos[pos, fsv], weaponPos[pos, ssv]);
					break;
				default: break;
			}*/
		}
		private void DrawObjects() //Draw Child / holds display for both items and weapons
		{
			///items / INT
			for (int i = 0; i < Global.globalAccess.itemIDs.Count; i++)
			{
				string ID = i.ToString();
				SetPosition(ID);
				Console.Write($"{Global.ITEM_AVATAR(ID)} x {stockItems[i]}");
			}
			/*// Healht Pot
			SetPosition(ITEM.POTHEAL);
			Console.Write($"{Global.ITEM_AVATAR(ITEM.POTHEAL)} x {stockItems[(int)ITEM.POTHEAL]}");
			// Shell Pot
			SetPosition(ITEM.POTSHELL);
			Console.Write($"{Global.ITEM_AVATAR(ITEM.POTSHELL)} x {stockItems[(int)ITEM.POTSHELL]}");
			// Big Key
			SetPosition(ITEM.KEYBIG);
			Console.Write($"{Global.ITEM_AVATAR(ITEM.KEYBIG)} x {stockItems[(int)ITEM.KEYBIG]}");
			// Small Key
			SetPosition(ITEM.KEYSMALL);
			Console.Write($"{Global.ITEM_AVATAR(ITEM.KEYSMALL)} x {stockItems[(int)ITEM.KEYSMALL]}");*/

			///weapons / BOOL
			for (int i = 0; i < Global.globalAccess.weaponIDs.Count; i++)
			{
				if (stockWeapons[i]) { SetPosition(i.ToString()); Console.Write(Global.WEAPON_AVATAR(i.ToString())); }
			}
			/*if (_stockWeapons[(int)WEAPON.FISTS]) { SetPosition(WEAPON.FISTS); Console.Write(Global.WEAPON_AVATAR(WEAPON.FISTS)); }
			if (_stockWeapons[(int)WEAPON.DAGGER]) { SetPosition(WEAPON.DAGGER); if (daggerOwned == false) { } else { Console.Write(Global.WEAPON_AVATAR(WEAPON.DAGGER)); } }
			if (_stockWeapons[(int)WEAPON.SHORTSWORD]) { SetPosition(WEAPON.SHORTSWORD); if (shortswordOwned == false) { } else { Console.Write(Global.WEAPON_AVATAR(WEAPON.SHORTSWORD)); } }
			if (_stockWeapons[(int)WEAPON.BROADSWORD]) { SetPosition(WEAPON.BROADSWORD); if (broadswordOwned == false) { } else { Console.Write(Global.WEAPON_AVATAR(WEAPON.BROADSWORD)); } }
			if (_stockWeapons[(int)WEAPON.LONGSWORD]) { SetPosition(WEAPON.LONGSWORD); if (longswordOwned == false) { } else { Console.Write(Global.WEAPON_AVATAR(WEAPON.LONGSWORD)); } }
			if (_stockWeapons[(int)WEAPON.CLAYMORE]) { SetPosition(WEAPON.CLAYMORE); if (claymoreOwned == false) { } else { Console.Write(Global.WEAPON_AVATAR(WEAPON.CLAYMORE)); } }
			if (_stockWeapons[(int)WEAPON.KALIBURN]) { SetPosition(WEAPON.KALIBURN); if (kaliburnOwned == false) { } else { Console.Write(Global.WEAPON_AVATAR(WEAPON.KALIBURN)); } }*/
		}
		#endregion

        #region Input/Output
        private void Input()
		{
			menuInput = Console.ReadKey(true);
			if (Console.KeyAvailable)
			{
				menuInput = Console.ReadKey(true);
				Console.Clear();
			}
		}
		private GAMESTATE NavigateOutput(GAMESTATE gameState)//cycles through inventory options / holds close inventory
		{
			if (menuInput.Key == ConsoleKey.W) // move cursor up
			{
				cursorPos--;
			}
			else if (menuInput.Key == ConsoleKey.A) // move cursor left
			{
				if (cursorPos < 20) // theres 20 items
				{ cursorPos -= 4; }
				else { cursorPos = 19; } // -1 because array starts at 0
			}
			else if (menuInput.Key == ConsoleKey.S) // move cursor down
			{
				cursorPos++;
			}
			else if (menuInput.Key == ConsoleKey.D) // move cursor right
			{
				if (cursorPos < 20) // theres 20 items
				{ cursorPos += 4; }
				else { cursorPos = 0; }
			}
			if (menuInput.Key == ConsoleKey.E) // return to Gameplay set to previous state
			{
				gameState = ReturnLastGameState();
			}

			// cycles to the start/back of selection
			if (cursorPos < 0) { cursorPos = maxObjects; }
			else if (cursorPos > maxObjects) { cursorPos = 0; };

			return gameState;
		}  
		private void UseInvetoryOutput(Player player, Item item, TradeMenu tradeMenu) // SpaceBar Input / HOLDS OBJECT USE SLOTS / 
		{
			Weapon weapon = player.EquipedWeapon();

			if (menuInput.Key == ConsoleKey.Spacebar)
			{
				//item slots // max 20
				for (int i = 0; i < this.itemPos.GetLength(0); i++)
				{
					if (cursorPos == i)
					{
						for (int j = 0; j < Global.globalAccess.itemIDs.Count; j++)
						{
							string ID = j.ToString();
							if (item.Avatar() == Global.globalAccess.ItemValues.avatars[j])
							{
								if (Global.globalAccess.ItemValues.effects[i] == "none" || Global.globalAccess.ItemValues.effects[i] == null) // should be enum
								{
									hud.DisplayText($"< this item is insignificant >", false);
								}
								else if (Regex.IsMatch(Global.globalAccess.ItemValues.desc[i], @"^[a-zA-Z]+$"))
									#region regex explaination
									/// ^ start of the string
									/// [] character set
									/// \ one time 
									/// + continious
									/// $ end of the string
								#endregion
								{
									if (cursorPos == i) { hud.DisplayText($" \"{Global.globalAccess.ItemValues.desc[i]}\"", false); }
									else { hud.DisplayText($"< i can't seem to remember what this is >", false); }

									if (Global.globalAccess.ItemValues.powers[i] > 0)
									{
										UsePot(player, item, hud, Global.globalAccess.ItemValues.effects[i]);
									}
									else { hud.DisplayText($"< it has no effect >", false); }
								}
								else 
								{  }
							}
						}
					}
				}

				/*if (navigator == 1) { UseOldPot(player, item, hud, ); }
				else if (navigator == 2) { hud.DisplayText($"< empty >", false); } 
				else if (navigator == 3) { hud.DisplayText($"< empty >", false); } 
				else if (navigator == 4) { hud.DisplayText(" \"A Big Key, for Big Problems, I mean Big Doors\"", false); }
				else if (navigator == 5) { hud.DisplayText(" \"A Small Key, it looks generic, I bet it would fit most locks\"", false); }
				else if (navigator == 6) { hud.DisplayText($"< empty >", false); }
				else if (navigator == 7) { hud.DisplayText($"< empty >", false); }
				else if (navigator == 8) { hud.DisplayText($"< empty >", false); }
				else if (navigator == 9) { hud.DisplayText($"< empty >", false); }
				else if (navigator == 10) { hud.DisplayText($"< empty >", false); }
				else if (navigator == 11) { hud.DisplayText($"< empty >", false); }
				else if (navigator == 12) { hud.DisplayText($"< empty >", false); }
				else if (navigator == 13) { hud.DisplayText($"< empty >", false); }
				else if (navigator == 14) { hud.DisplayText($"< empty >", false); }
				else if (navigator == 15) { hud.DisplayText($"< empty >", false); }
				else if (navigator == 16) { hud.DisplayText($"< empty >", false); }
				else if (navigator == 17) { hud.DisplayText($"< empty >", false); }
				else if (navigator == 18) { hud.DisplayText($"< empty >", false); }
				else { hud.DisplayText($"< empty >", false); }*/

				//weapon slots
				for (int i = 0; i < Global.globalAccess.weaponIDs.Count; i++)
				{
					string ID = i.ToString();
					if (cursorPos == 20+i && stockWeapons[Global.globalAccess.weaponIDs[ID]]) { player.damageMultiplier = tradeMenu.kaliburnDamageMultiplier; player.EquipWeapon(ID); hud.DisplayText($"< {player.Name()} Equiped {weapon.Name()} >", false); }
					else { hud.DisplayText($"< {player.Name()} Equiped nothing... >", false); }
				}
				/*else if (navigator == 20 && _stockWeapons[(int)WEAPON.FISTS]) { player.EquipWeapon(WEAPON.FISTS); hud.DisplayText($"< {player.Name()} Equiped {weapon.Name()} >", false); }
				else if (navigator == 21 && _stockWeapons[(int)WEAPON.DAGGER]) { hud.DisplayText($"< {player.Name()} Equiped nothing... >", false); } else { player.damageMultiplier = tradeMenu.daggerDamageMultiplier; player.EquipWeapon(WEAPON.DAGGER); hud.DisplayText($"< {player.Name()} Equiped {weapon.Name()} >", false); } 
				else if (navigator == 22 && _stockWeapons[(int)WEAPON.SHORTSWORD]) { hud.DisplayText($"< {player.Name()} Equiped nothing... >", false); } else { player.damageMultiplier = tradeMenu.shortswordDamageMultiplier; player.EquipWeapon(WEAPON.SHORTSWORD); hud.DisplayText($"< {player.Name()} Equiped {weapon.Name()} >", false); } 
				else if (navigator == 23 && _stockWeapons[(int)WEAPON.BROADSWORD]) { hud.DisplayText($"< {player.Name()} Equiped nothing... >", false); } else { player.damageMultiplier = tradeMenu.broadswordDamageMultplier; player.EquipWeapon(WEAPON.BROADSWORD); hud.DisplayText($"< {player.Name()} Equiped {weapon.Name()} >", false); } 
				else if (navigator == 24 && _stockWeapons[(int)WEAPON.LONGSWORD]) { hud.DisplayText($"< {player.Name()} Equiped nothing... >", false); } else { player.damageMultiplier = tradeMenu.longswordDamageMultplier; player.EquipWeapon(WEAPON.LONGSWORD); hud.DisplayText($"< {player.Name()} Equiped {weapon.Name()} >", false); } 
				else if (navigator == 25 && _stockWeapons[(int)WEAPON.CLAYMORE]) { hud.DisplayText($"< {player.Name()} Equiped nothing... >", false); } else { player.damageMultiplier = tradeMenu.claymoreDamageMultiplier; player.EquipWeapon(WEAPON.CLAYMORE); hud.DisplayText($"< {player.Name()} Equiped {weapon.Name()} >", false); }
				if (navigator == 26 && _stockWeapons[(int)WEAPON.KALIBURN]) { player.damageMultiplier = tradeMenu.kaliburnDamageMultiplier; player.EquipWeapon(WEAPON.KALIBURN); hud.DisplayText($"< {player.Name()} Equiped {weapon.Name()} >", false); }
				else { hud.DisplayText($"< {player.Name()} Equiped nothing... >", false); }*/

				hud.UpdateHotBar(player, this); // update hotbar on inventory use
			}
		}
        #endregion

        private void CreateBackground() // slightly differnt version of createBorder that fills in a blank background
		{
			//reset borderstring
			string borderString = "";

			// write to borderstring
			borderString += "╔,"; //Console.Write("╔"); 
			for (int i = 0; i < width - 2; i++)
			{
				borderString += "═,"; //Console.Write("═"); 
			}
			borderString += "╗;"; //Console.Write("╗"); 
			for (int i = 0; i < height - 2; i++)
			{
				borderString += "║,"; //Console.Write("║"); 
				for (int j = 0; j < width- 2; j++)
				{
					borderString += " ,"; // LEGACY : Console.Write(" ,"); 
				}
				borderString += "║;"; //Console.Write("║"); 
			}
			borderString += "╚,"; //Console.Write("╚"); 
			for (int i = 0; i < width - 2; i++)
			{
				borderString += "═,"; //Console.Write("═"); 
			}
			borderString += "╝"; //Console.Write("╝");

			// create array from string
			string[] borderY = borderString.Split(';');
			for (int i = 0; i < borderY.Length; i++)
			{
				string[] borderX = borderY[i].Split(','); // used for .Length and to Map the Xcoordinates on 
				for (int j = 0; j < borderX.Length; j++)
				{
					background[i, j] = borderX[j];
				}
			}
		}
		private GAMESTATE ReturnLastGameState() // NavigateOutput Child / returns to game screen 
		{
			return returnto;
		}

		// ----- public methods
		public void Draw() // displays inventory / draws cursor
		{
			hud.Draw();

			// init positioning for display
			int moveUIX = (Console.WindowWidth / 2) - (Camera.displayWidth / 2);
			if (moveUIX < 0) { moveUIX = 0; }
			int moveUIY = (Console.WindowHeight / 3) - (Camera.displayHeight / 2);
			if (moveUIY < 0) { moveUIY = 0; }

			// set position
			if (Console.WindowHeight != Camera.minConsoleSizeHeight || Console.WindowWidth != Camera.minConsoleSizeWidth)
			{ Console.SetCursorPosition(moveUIX + 1, moveUIY + 2); }
			else
			{ Console.SetCursorPosition(1, 2); }

			//change display according to position
			int line = 2;
			for (int y = 0; y < Camera.displayHeight - 1; y++)
			{
				for (int x = 0; x < Camera.displayWidth - 1; x++)
				{
					//write the display
					Console.Write(background[y, x]);
				}

				//move down a line
				line++;
				if (Console.WindowHeight != Camera.minConsoleSizeHeight || Console.WindowWidth != Camera.minConsoleSizeWidth)
				{ Console.SetCursorPosition(moveUIX + 1, moveUIY + line); }
				else
				{
					Console.SetCursorPosition(1, line);
				}
			}


			// set position of cursor  / -2 is cursor offset from selected item
			int[] menuSelect = { 0, 0 };
			if (cursorPos < 20) // there's 20 item slots but this doesnt reach 20 however it starts at 0 making 20 slots
			{
				menuSelect[0] = itemPos[cursorPos, 0] - 2;
				menuSelect[1] = itemPos[cursorPos, 1];
			}
			else // is weapon
			{
				int[] positioning = weaponPos[cursorPos - 20];
				menuSelect[0] = positioning[0] - 2;
				menuSelect[1] = positioning[1]; // -19 is because there's 20 item slots -1 for '0 start'
			}
			Console.SetCursorPosition(menuSelect[0], menuSelect[1]);
			Console.Write(cursor);

			DrawObjects();
		}
		public void InitInventory(Player player, GAMESTATE returnState) // could be changed into an input method to open inventory, currently player opens inventory: OpenInventoryOutput()
		{
			returnto = returnState; // sets return state
			CreateBackground(); // creates a new background for inventory upon opening it
			hud.Update(player, this); // init stats for inventory hud
			hud.ReloadText();
		}

        #region Item Handlers
		public void PickupFoundItems(Player player, List<Item> items, HUD hud)
		{
			for (int i = 0; i < items.Count; i++)
			{
				if (items[i].PickedUpByPlayer())
				{
					//switch statement changed to if else to accept Global parameter
					for (int j = 0; j < Global.globalAccess.itemIDs.Count; j++)
					{
						string ID = j.ToString();
						if (items[i].Avatar() == Global.ITEM_AVATAR(ID))
						{
							IncreaseStock(ID);
						}
					}

					/*if (items[i].Avatar() == Global.ITEM_AVATAR(ITEM.POTHEAL))
					{
						IncreaseStock(ITEM.POTHEAL);
					}
					else if (items[i].Avatar() == Global.ITEM_AVATAR(ITEM.POTSHELL))
					{
						IncreaseStock(ITEM.POTSHELL);
					}
					else if (items[i].Avatar() == Global.ITEM_AVATAR(ITEM.KEYBIG))
					{
						IncreaseStock(ITEM.KEYBIG);
					}
					else if (items[i].Avatar() == Global.ITEM_AVATAR(ITEM.KEYSMALL))
					{
						IncreaseStock(ITEM.KEYSMALL);
					}*/
					
					hud.UpdateHotBar(player, this);
					hud.Draw();
					hud.DisplayText($"< {player.Name()} picked up {items[i].Name()} [{items[i].Avatar()}] >", false);
					items[i].PickedUpByPlayer(false); // removes the items ability to be picked up /this completes the process of putting the item in the invetory, otherwise pick up every item set to pickedup every time player picks up new item
				}
			}
		}
		public void UsePot(Player player, Item item, HUD hud, string effect)
		{
			for (int i = 0; i < Global.globalAccess.itemIDs.Count; i++)
			{
				if (effect == Global.globalAccess.ItemValues.effects[i])
				{
					string ID = i.ToString();
					if (stockItems[i] > 0)
					{
						ActivateEffect(player, item, effect, ID);
						DecreaseStock(i);

						//update HUD bar and display to HUD text box
						hud.setHudHealthAndShield(player.Health(), player.Shield());
						hud.Draw();// updates visible inventory
						hud.UpdateHotBar(player, this);
					}
					else
					{
						hud.DisplayText($"< {player.Name()} {Global.ITEM_TEXTFAILURE(ID)} >", false);
					}
				}
			}
		}
		public void ActivateEffect(Player player, Item item, string effect, string ID )
		{
            if (effect == "health")
            {
                player.HealHealth(item.Power(Global.ITEM_AVATAR(ID)), this, hud);
            }
			else if (effect == "shield")
			{
				player.HealShell(item.Power(Global.ITEM_AVATAR(ID)), this, hud);
			}


		}
		/*public void UseHealthPot(Player player, Item item, HUD hud)
        {
			if (stockItems[(int)ITEM.POTHEAL] > 0)
			{
				player.HealHealth(item.Power(Global.ITEM_AVATAR(ITEM.POTHEAL)), this, hud);
				DecreaseStock(ITEM.POTHEAL);

				//update HUD bar and display to HUD text box
				hud.setHudHealthAndShield(player.Health(), player.Shield());
				hud.Draw();// updates visible inventory
				hud.UpdateHotBar(player, this);
			}
			else
			{
				hud.DisplayText($"< {player.Name()} {Global.MESSAGE_POTHEALTHMISSING} >", false);
			}
		}
		public void UseShieldPot(Player player, Item item, HUD hud)
		{
			if (stockItems[(int)ITEM.POTSHELL] > 0)
			{
				DecreaseStock(ITEM.POTSHELL);
				player.HealShell(item.Power(Global.ITEM_AVATAR(ITEM.POTSHELL)), this, hud);

				//update HUD bar and display to HUD text box
				hud.setHudHealthAndShield(player.Health(), player.Shield());
				hud.Draw();// updates visible inventory
				hud.UpdateHotBar(player, this);
			}
			else
			{
				hud.DisplayText($"< {player.Name()} {Global.MESSAGE_POTSHIELDMISSING} >", false);
			}
		}*/
        public void DecreaseStock(int ID) // called when item is used
		{
			stockItems[ID] -= 1;
		}
        #endregion

        public GAMESTATE Update(GAMESTATE gameState, Player player, Item item, Inventory inventory, TradeMenu tradeMenu) // used to interact with the inventory / remove normal input / control console cursor
		{
			hud.Update(player, inventory);
			Input();

			//Output
			gameState = NavigateOutput(gameState);
			UseInvetoryOutput(player, item, tradeMenu);

			return gameState;
		}	
	}
}
