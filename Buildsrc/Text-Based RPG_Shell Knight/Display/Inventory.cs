using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Based_RPG_Shell_Knight
{
	
	class Inventory
	{
		//stretch goals: scrolling inventory for larger implementation

		//init fields

		readonly int height = Camera.displayHeight;
		readonly int width = Camera.displayWidth;

		string[,] backround;  // holds the inventory border

		//moving the cursor
		ConsoleKeyInfo menuInput;
		private GAMESTATE returnto;
		int navigator = 0;
		int[] menuSelect = { 0, 0 };
		readonly char cursor = '>';
		readonly int[,] itemPos =
			//items
		{ {5, 5 }, {5, 10 }, {15, 5 }, {15, 10 },
			//weapons
		{ Camera.displayWidth - Global.WEAPON_AVATAR(WEAPON.FISTS).Length - 3, 5 },
		{ Camera.displayWidth - Global.WEAPON_AVATAR(WEAPON.DAGGER).Length - 3, 7 },
		{ Camera.displayWidth - Global.WEAPON_AVATAR(WEAPON.SHORTSWORD).Length - 3, 9 },
		{ Camera.displayWidth - Global.WEAPON_AVATAR(WEAPON.BROADSWORD).Length - 3, 11 },
		{ Camera.displayWidth - Global.WEAPON_AVATAR(WEAPON.LONGSWORD).Length - 3, 13 },
		{ Camera.displayWidth - Global.WEAPON_AVATAR(WEAPON.CLAYMORE).Length - 3, 15 },
		{ Camera.displayWidth - Global.WEAPON_AVATAR(WEAPON.KALIBURN).Length - 3, 17 } };

		char[] _avatarItems;
		string[] _avatarWeapons;
		int[] _stockItems; // holds the amount of Items aquired
		bool[] _stockWeapons; // holds what weapons have been aquired

		public Inventory() // creates the size of the inventory
		{
			backround = new string[height, width];

			_avatarItems = new char[(int)ITEM.TOTALITEMS];
			_stockItems = new int[(int)ITEM.TOTALITEMS];
			for (ITEM i = 0; i < ITEM.TOTALITEMS; i++)
			{
				_avatarItems[(int)i] = Global.ITEM_AVATAR(i);
				_stockItems[(int)i] = 3;
			}

			_avatarWeapons = new string[(int)WEAPON.TOTALWEAPONS];
			_stockWeapons = new bool[(int)WEAPON.TOTALWEAPONS];
			for (WEAPON i = 0; i < WEAPON.TOTALWEAPONS; i++)
			{
				_avatarWeapons[(int)i] = Global.WEAPON_AVATAR(i);
				_stockWeapons[(int)i] = true;
			}

			UpdateBorder();
		}


		// ----- gets/ sets
		public char[] ItemAvatars()
		{
			return _avatarItems;
		}
		public int[] ItemStock()
		{
			return _stockItems;
		}

		// ----- private methods
		private void UpdateBorder()
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
					backround[i, j] = borderX[j];
				}
			}
		}
		private GAMESTATE Close(GAMESTATE gameState) // returns to game screen /
		{
			return returnto;
		}

		// ----- public methods
		public void SetReturnState(GAMESTATE returnState) // could be changed into an input method to open inventory
		{
			returnto = returnState;
		}
		public void Draw() // displays inventory and all the aquired/available items
		{
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

			//write the display
			int line = 2;
			for (int y = 0; y < Camera.displayHeight - 1; y++)
			{
				for (int x = 0; x < Camera.displayWidth - 1; x++)
				{
					Console.Write(backround[y, x]);
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
			Console.SetCursorPosition(menuSelect[0], menuSelect[1]);
			Console.Write(cursor);
		}
		public void UseItem(ITEM item) // used to distinguish function
		{
			_stockItems[(int)item] -= 1;
		}
		public void PickupIfFound(Player player, List<Item> items, HUD hud)
		{
			for (int i = 0; i < items.Count; i++)
			{
				if (items[i].PickedUpByPlayer())
				{
					switch (items[i].Avatar())
					{
						case 'ö':
							IncreaseStock(ITEM.POTHEAL);
							break;

						case 'ï':
							IncreaseStock(ITEM.POTSHELL);
							break;

						case 'K':
							IncreaseStock(ITEM.KEYBIG);
							break;

						case 'k':
							IncreaseStock(ITEM.KEYSMALL);
							break;
						default:
							break;
					}
					hud.UpdateHotBar(player, this);
					hud.Draw();
					hud.DisplayText($"< {player.Name()} picked up {items[i].Name()} [{items[i].Avatar()}] >", false);
					items[i].PickedUpByPlayer(false); // removes the items ability to be picked up /this completes the process of putting the item in the invetory
				}
			}
		}
		public void IncreaseStock(ITEM item)
		{
			_stockItems[(int)item] ++;
		} // FindItem Child
		public GAMESTATE Navigate(GAMESTATE gameState, Player player, Item item) // used to interact with the inventory / remove normal input / control console cursor
		{
			Weapon weapon = player.EquipedWeapon();


			menuInput = Console.ReadKey(true);

			if (Console.KeyAvailable)
			{
				menuInput = Console.ReadKey(true);
				Console.Clear();
			}

			if (menuInput.Key == ConsoleKey.W)
			{
				navigator--;
			}
			else if (menuInput.Key == ConsoleKey.S)
			{
				navigator++;
			}

			if (navigator < 0) { navigator = 10; }
			else if (navigator > 10) { navigator = 0; };

			menuSelect[0] = itemPos[navigator, 0] - 2;
			menuSelect[1] = itemPos[navigator, 1];


			if (menuInput.Key == ConsoleKey.Spacebar)
			{
				if (navigator >= 0 && navigator <= 3)
				{
					if (navigator == 0 && _stockItems[(int)ITEM.POTHEAL] > 0) { player.HealHealth(item.Power(_avatarItems[(int)ITEM.POTHEAL])); UseItem(ITEM.POTHEAL); }
					else if (navigator == 1 && _stockItems[(int)ITEM.POTSHELL] > 0) { player.HealShell(item.Power(_avatarItems[(int)ITEM.POTSHELL])); UseItem(ITEM.POTSHELL); }
					else if (navigator == 2) { } // do nothing
					else if (navigator == 3) { } // do nothing
				}
				else if (navigator >= 4 && navigator <= 10)
				{
					if (navigator == 4 && _stockWeapons[(int)WEAPON.FISTS]) { weapon.IdentifyAndEquip(WEAPON.FISTS); }
					else if (navigator == 5 && _stockWeapons[(int)WEAPON.DAGGER]) { weapon.IdentifyAndEquip(WEAPON.DAGGER); }
					else if (navigator == 6 && _stockWeapons[(int)WEAPON.SHORTSWORD]) { weapon.IdentifyAndEquip(WEAPON.SHORTSWORD); }
					else if (navigator == 7 && _stockWeapons[(int)WEAPON.BROADSWORD]) { weapon.IdentifyAndEquip(WEAPON.BROADSWORD); }
					else if (navigator == 8 && _stockWeapons[(int)WEAPON.LONGSWORD]) { weapon.IdentifyAndEquip(WEAPON.LONGSWORD); }
					else if (navigator == 9 && _stockWeapons[(int)WEAPON.CLAYMORE]) { weapon.IdentifyAndEquip(WEAPON.CLAYMORE); }
					else if (navigator == 10 && _stockWeapons[(int)WEAPON.KALIBURN]) { weapon.IdentifyAndEquip(WEAPON.KALIBURN); }
					player.EquipedWeaponDamageRange(weapon.DamageRange());
				}
			}

			if (menuInput.Key == ConsoleKey.I) // return to Gameplay set to Map
			{
				gameState = Close(gameState);
			}
			return gameState;
		}	
		public void Update(Player player, HUD hud) // update while accessing inventory, only changes equip and items use
		{
			//items
			// HP up
			if (_stockItems[(int)ITEM.POTHEAL] > 0)
			{
				Console.SetCursorPosition(itemPos[0, 0], itemPos[0, 1]);
				if (_stockItems[(int)ITEM.POTHEAL] < 2) { Console.Write($" ö "); }
				else if (_stockItems[(int)ITEM.POTHEAL] > 1) { Console.Write($" ö x {_stockItems[(int)ITEM.POTHEAL]} "); }
			}
			// SP up
			if (_stockItems[(int)ITEM.POTSHELL] > 0)
			{
				Console.SetCursorPosition(itemPos[1, 0], itemPos[1, 1]);
				if (_stockItems[(int)ITEM.POTSHELL] < 2) { Console.Write($" ï "); }
				else if (_stockItems[(int)ITEM.POTSHELL] > 1) { Console.Write($" ï x {_stockItems[(int)ITEM.POTSHELL]} "); }
			}
			// Big Key
			if (_stockItems[(int)ITEM.KEYBIG] > 0)
			{
				Console.SetCursorPosition(itemPos[2, 0], itemPos[2, 1]);
				if (_stockItems[(int)ITEM.KEYBIG] < 2) { Console.Write($" K "); }
				else if (_stockItems[(int)ITEM.KEYBIG] > 1) { Console.Write($" K x {_stockItems[(int)ITEM.KEYBIG]} "); }
			}
			// Small Key
			if (_stockItems[(int)ITEM.KEYSMALL] > 0)
			{
				Console.SetCursorPosition(itemPos[3, 0], itemPos[3, 1]);
				if (_stockItems[(int)ITEM.KEYSMALL] < 2) { Console.Write($" k "); }
				else if (_stockItems[(int)ITEM.KEYSMALL] > 1) { Console.Write($" k x {_stockItems[(int)ITEM.KEYSMALL]} "); }
			}

			//weapons
			if (_stockWeapons[(int)WEAPON.FISTS])		{ Console.SetCursorPosition(itemPos[4, 0], itemPos[4, 1]); Console.Write(_avatarWeapons[(int)WEAPON.FISTS]); }
			if (_stockWeapons[(int)WEAPON.DAGGER])		{ Console.SetCursorPosition(itemPos[5, 0], itemPos[5, 1]); Console.Write(_avatarWeapons[(int)WEAPON.DAGGER]); }
			if (_stockWeapons[(int)WEAPON.SHORTSWORD])	{ Console.SetCursorPosition(itemPos[6, 0], itemPos[6, 1]); Console.Write(_avatarWeapons[(int)WEAPON.SHORTSWORD]); }
			if (_stockWeapons[(int)WEAPON.BROADSWORD])	{ Console.SetCursorPosition(itemPos[7, 0], itemPos[7, 1]); Console.Write(_avatarWeapons[(int)WEAPON.BROADSWORD]); }
			if (_stockWeapons[(int)WEAPON.LONGSWORD])	{ Console.SetCursorPosition(itemPos[8, 0], itemPos[8, 1]); Console.Write(_avatarWeapons[(int)WEAPON.LONGSWORD]); }
			if (_stockWeapons[(int)WEAPON.CLAYMORE])	{ Console.SetCursorPosition(itemPos[9, 0], itemPos[9, 1]); Console.Write(_avatarWeapons[(int)WEAPON.CLAYMORE]); }
			if (_stockWeapons[(int)WEAPON.KALIBURN])	{ Console.SetCursorPosition(itemPos[10, 0], itemPos[10, 1]); Console.Write(_avatarWeapons[(int)WEAPON.KALIBURN]); }

			hud.UpdateHotBar(player, this);
		}
	}
}
