using FishermanNPC.NPCs;
using FishermanNPC.NPCs.TownNPCs;
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
			AddMapEntry(new Color(171, 87, 0), CreateMapEntryName());
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
					player.QuickSpawnItemDirect(player.GetSource_TileInteraction(i, j), ItemID.Silk, Main.rand.Next(3, 8));
				}
				else if (Main.rand.NextBool(2)) //50% chance after the 75% (25%) chance
				{
					player.QuickSpawnItemDirect(player.GetSource_TileInteraction(i, j), ItemID.Leather, Main.rand.Next(5, 13));
				}
				else
				{
					player.QuickSpawnItemDirect(player.GetSource_TileInteraction(i, j), ItemID.GreenThread, Main.rand.Next(6, 10));
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

				if (Main.rand.NextBool(2)) //50% chance
				{
					player.QuickSpawnItemDirect(player.GetSource_TileInteraction(i, j), ItemID.Acorn, Main.rand.Next(5, 8));
				}
				else if (Main.rand.NextBool(2)) //50% chance after the 50% chance
				{
					player.QuickSpawnItemDirect(player.GetSource_TileInteraction(i, j), ItemID.DaybloomSeeds, Main.rand.Next(5, 10));
				}
				else if (Main.rand.NextBool(2)) //50% chance after
				{
					player.QuickSpawnItemDirect(player.GetSource_TileInteraction(i, j), ItemID.BlinkrootSeeds, Main.rand.Next(5, 10));
				}
				else if (Main.rand.NextBool(2)) //50% chance after
				{
					player.QuickSpawnItemDirect(player.GetSource_TileInteraction(i, j), ItemID.ShiverthornSeeds, Main.rand.Next(5, 10));
				}
				else
				{
					player.QuickSpawnItemDirect(player.GetSource_TileInteraction(i, j), ItemID.WaterleafSeeds, Main.rand.Next(5, 10));
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

				if (Main.rand.NextBool(2)) //50% chance
				{
					player.QuickSpawnItemDirect(player.GetSource_TileInteraction(i, j), ItemID.TinOre, Main.rand.Next(8, 16));
				}
				else if (Main.rand.NextBool(2)) //50% chance after the 50% chance
				{
					player.QuickSpawnItemDirect(player.GetSource_TileInteraction(i, j), ItemID.CopperOre, Main.rand.Next(8, 16));
				}
				else
				{
					player.QuickSpawnItemDirect(player.GetSource_TileInteraction(i, j), ItemID.Bottle, Main.rand.Next(4, 8));
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
					player.QuickSpawnItemDirect(player.GetSource_TileInteraction(i, j), ItemID.StoneBlock, Main.rand.Next(10, 30));
				}
				else if (Main.rand.NextBool(2)) //50% chance after the 75% (25%) chance
				{
					player.QuickSpawnItemDirect(player.GetSource_TileInteraction(i, j), ItemID.Hook, Main.rand.Next(1, 5));
				}
				else
				{
					player.QuickSpawnItemDirect(player.GetSource_TileInteraction(i, j), ItemID.Geode, Main.rand.Next(1, 5));
				}
			}


			if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && ModContent.GetInstance<FishermanNPCConfigServer>().TownNPCsCrossModSupport && FishermanShops.ThoriumMod)
			{
				int igneousRock = NPCHelper.SafelyGetCrossModItem(thorium, "IgneousRock");
				int scorchedBone = NPCHelper.SafelyGetCrossModItem(thorium, "ScorchedBone");
				if ((player.HeldItem.type == igneousRock && player.HasItem(igneousRock) || Main.mouseItem.type == igneousRock) && igneousRock > 0)
				{
					SoundEngine.PlaySound(SoundID.Item22, new Vector2(i * 16, j * 16));

					if (Main.mouseItem.type == igneousRock)
					{
						Main.mouseItem.stack--;
					}
					else if (player.HeldItem.type == igneousRock)
					{
						player.ConsumeItem(igneousRock);
					}

					if (Main.rand.Next(4) <= 2)
					{
						player.QuickSpawnItemDirect(player.GetSource_TileInteraction(i, j), ItemID.Obsidian, Main.rand.Next(5, 21));
					}
					else if (Main.rand.NextBool(2))
					{
						player.QuickSpawnItemDirect(player.GetSource_TileInteraction(i, j), ItemID.HellfireArrow, Main.rand.Next(5, 16));
					}
					else
					{
						player.QuickSpawnItemDirect(player.GetSource_TileInteraction(i, j), ItemID.MagmaStone);
					}
				}
				if ((player.HeldItem.type == scorchedBone && player.HasItem(scorchedBone) || Main.mouseItem.type == scorchedBone) && scorchedBone > 0)
				{
					SoundEngine.PlaySound(SoundID.Item127, new Vector2(i * 16, j * 16));

					if (Main.mouseItem.type == scorchedBone)
					{
						Main.mouseItem.stack--;
					}
					else if (player.HeldItem.type == scorchedBone)
					{
						player.ConsumeItem(scorchedBone);
					}

					if (Main.rand.Next(4) <= 2)
					{
						player.QuickSpawnItemDirect(player.GetSource_TileInteraction(i, j), ItemID.AshBlock, Main.rand.Next(10, 30));
					}
					else if (Main.rand.NextBool(2))
					{
						player.QuickSpawnItemDirect(player.GetSource_TileInteraction(i, j), ItemID.BoneSword);
					}
					else
					{
						player.QuickSpawnItemDirect(player.GetSource_TileInteraction(i, j), ItemID.Fireblossom, Main.rand.Next(1, 5));
					}
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
			else if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && ModContent.GetInstance<FishermanNPCConfigServer>().TownNPCsCrossModSupport && FishermanShops.ThoriumMod)
			{
				int igneousRock = NPCHelper.SafelyGetCrossModItem(thorium, "IgneousRock");
				int scorchedBone = NPCHelper.SafelyGetCrossModItem(thorium, "ScorchedBone");
				if (player.HeldItem.type == igneousRock && player.HasItem(igneousRock) && igneousRock > 0)
				{
					player.cursorItemIconID = igneousRock;
				}
				else if (player.HeldItem.type == scorchedBone && player.HasItem(scorchedBone) && igneousRock > 0)
				{
					player.cursorItemIconID = scorchedBone;
				}
				else
				{
					player.cursorItemIconID = ModContent.ItemType<Items.Placeable.RecyclingMachine>();
				}
			}
			else
			{
				player.cursorItemIconID = ModContent.ItemType<Items.Placeable.RecyclingMachine>();
			}
		}
	}
}