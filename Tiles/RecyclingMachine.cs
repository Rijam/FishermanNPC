using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace FishermanNPC.Tiles
{
	public class RecyclingMachine : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Origin = new Point16(1, 2);
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
			TileObjectData.addTile(Type);
			AddMapEntry(new Color(171, 87, 0), Language.GetText("Mods.FishermanNPC.MapObject.RecyclingMachine"));
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 48, ModContent.ItemType<Items.Placeable.RecyclingMachine>());
		}

		public override bool RightClick(int i, int j)
		{
			Player player = Main.LocalPlayer;

			// Main.mouseItem.type ==
			// player.inventory[58].type ==


			if ((player.HeldItem.type == ItemID.OldShoe && player.HasItem(ItemID.OldShoe)) || Main.mouseItem.type == ItemID.OldShoe)
			{
				SoundEngine.PlaySound(SoundID.Item65, new Vector2(i * 16, j * 16));
				
				if (Main.mouseItem.type == ItemID.OldShoe)
				{
					Main.mouseItem.stack--; //:grimacing:
				}
				else if (player.HeldItem.type == ItemID.OldShoe)
                {
					player.ConsumeItem(ItemID.OldShoe);
				}
				 
				if (Main.rand.Next(4) <= 2) //75% chance: 0, 1, 2, but not 3
				{
					player.QuickSpawnItemDirect(player.GetSource_TileInteraction(i, j), ItemID.Silk, Main.rand.Next(1, 3));
				}
				else if (Main.rand.Next(2) == 0) //50% chance after the 75% (25%) chance
				{
					player.QuickSpawnItemDirect(player.GetSource_TileInteraction(i, j), ItemID.Leather, Main.rand.Next(1, 3));
				}
				else
				{
					player.QuickSpawnItemDirect(player.GetSource_TileInteraction(i, j), ItemID.GreenThread, Main.rand.Next(1, 3));
				}
			}
			if ((player.HeldItem.type == ItemID.FishingSeaweed && player.HasItem(ItemID.FishingSeaweed)) || Main.mouseItem.type == ItemID.FishingSeaweed)
			{
				SoundEngine.PlaySound(SoundID.Item171, new Vector2(i * 16, j * 16));

				if (Main.mouseItem.type == ItemID.FishingSeaweed)
				{
					Main.mouseItem.stack--; //:grimacing:
				}
				else if (player.HeldItem.type == ItemID.FishingSeaweed)
				{
					player.ConsumeItem(ItemID.FishingSeaweed);
				}

				if (Main.rand.Next(4) <= 2) //75% chance
				{
					player.QuickSpawnItemDirect(player.GetSource_TileInteraction(i, j), ItemID.Acorn, Main.rand.Next(1, 2));
				}
				else if (Main.rand.Next(2) == 0) //50% chance after the 75% (25%) chance
				{
					player.QuickSpawnItemDirect(player.GetSource_TileInteraction(i, j), ItemID.DaybloomSeeds, Main.rand.Next(1, 2));
				}
				else if (Main.rand.Next(2) == 0) //50% chance after
				{
					player.QuickSpawnItemDirect(player.GetSource_TileInteraction(i, j), ItemID.BlinkrootSeeds, Main.rand.Next(1, 2));
				}
				else if (Main.rand.Next(2) == 0) //50% chance after
				{
					player.QuickSpawnItemDirect(player.GetSource_TileInteraction(i, j), ItemID.ShiverthornSeeds, Main.rand.Next(1, 2));
				}
				else
				{
					player.QuickSpawnItemDirect(player.GetSource_TileInteraction(i, j), ItemID.WaterleafSeeds, Main.rand.Next(1, 2));
				}
			}
			if ((player.HeldItem.type == ItemID.TinCan && player.HasItem(ItemID.TinCan)) || Main.mouseItem.type == ItemID.TinCan)
			{
				SoundEngine.PlaySound(SoundID.Item52, new Vector2(i * 16, j * 16));

				if (Main.mouseItem.type == ItemID.TinCan)
				{
					Main.mouseItem.stack--; //:grimacing:
				}
				else if (player.HeldItem.type == ItemID.TinCan)
				{
					player.ConsumeItem(ItemID.TinCan);
				}

				if (Main.rand.Next(2) == 0) //50% chance
				{
					player.QuickSpawnItemDirect(player.GetSource_TileInteraction(i, j), ItemID.TinOre, Main.rand.Next(1, 2));
				}
				else if (Main.rand.Next(2) == 0) //50% chance after the 50% chance
				{
					player.QuickSpawnItemDirect(player.GetSource_TileInteraction(i, j), ItemID.CopperOre, Main.rand.Next(1, 2));
				}
				else
				{
					player.QuickSpawnItemDirect(player.GetSource_TileInteraction(i, j), ItemID.Bottle, Main.rand.Next(1, 2));
				}
			}
			if ((player.HeldItem.type == ItemID.Coal && player.HasItem(ItemID.Coal)) || Main.mouseItem.type == ItemID.Coal)
			{
				SoundEngine.PlaySound(SoundID.Item23, new Vector2(i * 16, j * 16));

				if (Main.mouseItem.type == ItemID.Coal)
				{
					Main.mouseItem.stack--; //:grimacing:
				}
				else if (player.HeldItem.type == ItemID.Coal)
				{
					player.ConsumeItem(ItemID.Coal);
				}

				if (Main.rand.Next(4) <= 2) //75% chance
				{
					player.QuickSpawnItemDirect(player.GetSource_TileInteraction(i, j), ItemID.StoneBlock, Main.rand.Next(1, 3));
				}
				else if (Main.rand.Next(2) == 0) //50% chance after the 75% (25%) chance
				{
					player.QuickSpawnItemDirect(player.GetSource_TileInteraction(i, j), ItemID.Hook, 1);
				}
				else
				{
					player.QuickSpawnItemDirect(player.GetSource_TileInteraction(i, j), ItemID.Geode, 1);
				}
			}
			return true;
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			if (player.HeldItem.type == ItemID.OldShoe && player.HasItem(ItemID.OldShoe)) 
			{
				player.cursorItemIconID = ItemID.OldShoe;
			}
			else if (player.HeldItem.type == ItemID.FishingSeaweed && player.HasItem(ItemID.FishingSeaweed))
			{
				player.cursorItemIconID = ItemID.FishingSeaweed;
			}
			else if (player.HeldItem.type == ItemID.TinCan && player.HasItem(ItemID.TinCan))
			{
				player.cursorItemIconID = ItemID.TinCan;
			}
			else if (player.HeldItem.type == ItemID.Coal && player.HasItem(ItemID.Coal))
			{
				player.cursorItemIconID = ItemID.Coal;
			}
			else
			{
				player.cursorItemIconID = ModContent.ItemType<Items.Placeable.RecyclingMachine>();
			}
		}
	}
}