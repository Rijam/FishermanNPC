using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Chat;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using FishermanNPC.NPCs.TownNPCs;
using Terraria.Localization;

namespace FishermanNPC.NPCs
{
	/// <summary>
	/// NPCHelper is a small class that "automates" many repeated things for the Town NPCs.
	/// </summary>
	public class NPCHelper
	{
		/// Mod name
		private static readonly string mod = ModContent.GetInstance<FishermanNPC>().Name;

		/// <summary>
		/// Automatically gets the localized Loved text for the Bestiary.
		/// </summary>
		/// <param name="npc">The NPC's (class) name. In most cases, just pass Name</param>
		/// <returns>string</returns>
		public static string LoveText(string npc)
		{
			return "[c/b3f2b3:" + Language.GetTextValue("RandomWorldName_Noun.Love") + "]: " + Language.GetTextValue("Mods." + mod + ".Bestiary.Happiness." + npc + ".Love") + "\n";
		}

		/// <summary>
		/// Automatically gets the localized Liked text for the Bestiary.
		/// </summary>
		/// <param name="npc">The NPC's (class) name. In most cases, just pass Name</param>
		/// <returns>string</returns>
		public static string LikeText(string npc)
		{
			return "[c/ddf2b3:" + Language.GetTextValue("Mods.FishermanNPC.UI.Like") + "]: " + Language.GetTextValue("Mods." + mod + ".Bestiary.Happiness." + npc + ".Like") + "\n";
		}

		/// <summary>
		/// Automatically gets the localized Disliked text for the Bestiary.
		/// </summary>
		/// <param name="npc">The NPC's (class) name. In most cases, just pass Name</param>
		/// <returns>string</returns>
		public static string DislikeText(string npc)
		{
			return "[c/f2e0b3:" + Language.GetTextValue("Mods.FishermanNPC.UI.Dislike") + "]: " + Language.GetTextValue("Mods." + mod + ".Bestiary.Happiness." + npc + ".Dislike") + "\n";
		}

		/// <summary>
		/// Automatically gets the localized Hated text for the Bestiary.
		/// </summary>
		/// <param name="npc">The NPC's (class) name. In most cases, just pass Name</param>
		/// <returns>string</returns>
		public static string HateText(string npc)
		{
			return "[c/f2b5b3:" + Language.GetTextValue("RandomWorldName_Noun.Hate") + "]: " + Language.GetTextValue("Mods." + mod + ".Bestiary.Happiness." + npc + ".Hate");
		}

		/// <summary>
		/// Automatically gets the path to the localized Bestiary Description.
		/// </summary>
		/// <param name="npc">The NPC's (class) name. In most cases, just pass Name</param>
		/// <returns>string</returns>
		public static string BestiaryPath(string npc)
		{
			return "Mods." + mod + ".Bestiary.Description." + npc;
		}

		/// <summary>
		/// Automatically gets the base path to the localized dialog. Add `+ "Key"` to get the dialog.
		/// </summary>
		/// <param name="npc">The NPC's (class) name. In most cases, just pass Name</param>
		/// <returns>string</returns>
		public static string DialogPath(string npc)
		{
			return "Mods." + mod + ".NPCDialog." + npc + ".";
		}

		/// <summary>
		/// Gets if the player has unlocked the Otherworldly music. Doesn't actually check for the player, but the shop runs client side so it doesn't matter.
		/// </summary>
		/// <returns>bool</returns>
		public static bool UnlockOWMusic()
		{
			return Main.Configuration.Get("UnlockMusicSwap", false);
		}

		private static int shopCycler = 1;
		// 1 = Bait
		// 2 = Fish
		// 3 = Rods
		// 4 = Extra

		/// <summary>
		/// Sets the shopCycler int.
		/// </summary>
		public static void SetShopCycle(int type)
		{
			shopCycler = type;
		}

		/// <summary>
		/// Increments the shopCycler int. If it exceeds 4 (>= 5), it will be set to 1 again.
		/// Will not select the shop if the config for that shop is disabled.
		/// </summary>
		public static void IncrementShopCycle()
		{
			bool sellBait = ModContent.GetInstance<FishermanNPCConfigServer>().SellBait;
			bool sellFish = ModContent.GetInstance<FishermanNPCConfigServer>().SellFish;
			bool sellFishingRods = ModContent.GetInstance<FishermanNPCConfigServer>().SellFishingRods;
			bool sellExtraItems = ModContent.GetInstance<FishermanNPCConfigServer>().SellExtraItems;

			shopCycler++;
			if (!sellBait && shopCycler == 1) // If disabled, go to the next shop.
			{
				shopCycler++;
			}
			if (!sellFish && shopCycler == 2)
			{
				shopCycler++;
			}
			if (!sellFishingRods && shopCycler == 3)
			{
				shopCycler++;
			}
			if (!sellExtraItems && shopCycler == 4)
			{
				shopCycler++;
			}

			if (shopCycler >= 5)
			{
				shopCycler = 0;
				if (sellBait || sellFish || sellFishingRods || sellExtraItems) // Only call if at least one of the shops are enabled.
				{
					IncrementShopCycle();
				}
			}
		}

		/// <summary>
		/// Gets the current shop selected.
		/// </summary>
		public static int StatusShopCycle()
		{
			return shopCycler;
		}

		/// <summary>
		/// Gets if the player is in the Underground, Caverns, or Underworld layers.
		/// </summary>
		public static bool ZoneAnyUnderground(Player player)
		{
			return player.ZoneDirtLayerHeight || player.ZoneRockLayerHeight || player.ZoneUnderworldHeight;
		}

		/// <summary>
		/// Safely returns the integer of the ModItem from the given mod.
		/// </summary>
		/// <param name="mod">The mod that the item is from.</param>
		/// <param name="itemString">The class name of the item.</param>
		/// <returns>int if found, 0 if not found.</returns>
		public static int SafelyGetCrossModItem(Mod mod, string itemString)
		{
			mod.TryFind<ModItem>(itemString, out ModItem outItem);
			if (outItem != null)
			{
				return outItem.Type;
			}
			ModContent.GetInstance<FishermanNPC>().Logger.WarnFormat("SafelyGetCrossModItem(): ModItem type \"{0}\" from \"{1}\" was not found.", itemString, mod);
			return 0;
		}

		/// <summary>
		/// Safely sets the shop item of the ModItem from the given slot in the given slot.
		/// Will not set the shop item if the ModItem is not found.
		/// The price of the item will be the customPrice.
		/// </summary>
		/// <param name="mod">The mod that the item is from.</param>
		/// <param name="itemString">The class name of the item.</param>
		/// <param name="shop">The Chest shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="nextSlot">The ref nextSlot. Pass ref nextSlot in most cases.</param>
		/// <param name="customPrice">The custom price of the item.</param>
		public static void SafelySetCrossModItem(Mod mod, string itemString, Chest shop, ref int nextSlot, int customPrice)
		{
			mod.TryFind<ModItem>(itemString, out ModItem outItem);
			if (outItem != null)
			{
				shop.item[nextSlot].SetDefaults(outItem.Type);
				shop.item[nextSlot].shopCustomPrice = customPrice;
				nextSlot++;
			}
			else
			{
				ModContent.GetInstance<FishermanNPC>().Logger.WarnFormat("SafelySetCrossModItem(): ModItem type \"{0}\" from \"{1}\" was not found.", itemString, mod);
			}
		}

		/// <summary>
		/// Safely sets the shop item of the ModItem from the given slot in the given slot.
		/// Will not set the shop item if the ModItem is not found.
		/// The price of the item will be the item's value / 5 / priceDiv.
		/// </summary>
		/// <param name="mod">The mod that the item is from.</param>
		/// <param name="itemString">The class name of the item.</param>
		/// <param name="shop">The Chest shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="nextSlot">The ref nextSlot. Pass ref nextSlot in most cases.</param>
		/// <param name="priceDiv">The price will be divided by this amount.</param>
		public static void SafelySetCrossModItem(Mod mod, string itemString, Chest shop, ref int nextSlot, float priceDiv)
		{
			mod.TryFind<ModItem>(itemString, out ModItem outItem);
			if (outItem != null)
			{
				shop.item[nextSlot].SetDefaults(outItem.Type);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(shop.item[nextSlot].value / 5 / priceDiv);
				nextSlot++;
			}
			else
			{
				ModContent.GetInstance<FishermanNPC>().Logger.WarnFormat("SafelySetCrossModItem(): ModItem type \"{0}\" from \"{1}\" was not found.", itemString, mod);
			}
		}

		/// <summary>
		/// Safely sets the shop item of the ModItem from the given slot in the given slot.
		/// Will not set the shop item if the ModItem is not found.
		/// The price of the item will be the item's (value / priceDiv) * priceMulti.
		/// </summary>
		/// <param name="mod">The mod that the item is from.</param>
		/// <param name="itemString">The class name of the item.</param>
		/// <param name="shop">The Chest shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="nextSlot">The ref nextSlot. Pass ref nextSlot in most cases.</param>
		/// <param name="priceDiv">The price will be divided by this amount.</param>
		/// <param name="priceMulti">The price will be multiplied by this amount after the priceDiv.</param>
		public static void SafelySetCrossModItem(Mod mod, string itemString, Chest shop, ref int nextSlot, float priceDiv = 1f, float priceMulti = 1f)
		{
			mod.TryFind<ModItem>(itemString, out ModItem outItem);
			if (outItem != null)
			{
				shop.item[nextSlot].SetDefaults(outItem.Type);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(shop.item[nextSlot].value / priceDiv * priceMulti);
				nextSlot++;
			}
			else
			{
				ModContent.GetInstance<FishermanNPC>().Logger.WarnFormat("SafelySetCrossModItem(): ModItem type \"{0}\" from \"{1}\" was not found.", itemString, mod);
			}
		}

		/// <summary>
		/// Safely returns the Item of the ModItem from the given mod.
		/// </summary>
		/// <param name="mod">The mod that the item is from.</param>
		/// <param name="itemString">The class name of the item.</param>
		/// <returns>Item if found, null if not found.</returns>
		public static Item SafelyGetCrossModItemWithPrice(Mod mod, string itemString, float priceDiv = 1f, float priceMulti = 1f)
		{
			mod.TryFind<ModItem>(itemString, out ModItem outItem);
			if (outItem != null)
			{
				outItem.Item.shopCustomPrice = (int)Math.Round(outItem.Item.value / priceDiv * priceMulti);
				return outItem.Item;
			}
			ModContent.GetInstance<FishermanNPC>().Logger.WarnFormat("SafelyGetCrossModItem(): ModItem type \"{0}\" from \"{1}\" was not found.", itemString, mod);
			return null;
		}

		/// <summary>
		/// Counts all of the Town NPCs in the world. Town Pets, Old Man, Traveling Merchant, and Skeleton Merchant are not included.
		/// </summary>
		public static int CountTownNPCs()
		{
			int counter = 0;
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC nPC = Main.npc[i];
				if (nPC.active && nPC.townNPC)
				{
					if (nPC.type != NPCID.OldMan || nPC.type != NPCID.TravellingMerchant || nPC.type != NPCID.SkeletonMerchant || !NPCID.Sets.ActsLikeTownNPC[nPC.type] || !NPCID.Sets.IsTownPet[nPC.type])
					{
						counter++;
					}
				}
			}
			return counter;
		}
	}

	public static class ShopConditions
	{
#pragma warning disable CA2211 // Non-constant fields should not be visible

		public static Condition SellModdedItems = new("\'Sell Fisherman NPC Modded Items\' config is enabled", () => ModContent.GetInstance<FishermanNPCConfigServer>().SellModdedItems);
		public static Condition SellBait = new("\'Sell Bait\' config is enabled", () => ModContent.GetInstance<FishermanNPCConfigServer>().SellBait);
		public static Condition SellFish = new("\'Sell Fish\' config is enabled", () => ModContent.GetInstance<FishermanNPCConfigServer>().SellFish);
		public static Condition SellFishingRods = new("\'Sell Fishing Rods\' config is enabled", () => ModContent.GetInstance<FishermanNPCConfigServer>().SellFishingRods);
		public static Condition SellExtraItems = new("\'Sell Extra Items\' config is enabled", () => ModContent.GetInstance<FishermanNPCConfigServer>().SellExtraItems);
		public static Condition TownNPCsCrossModSupport = new("\'Fisherman Cross Mod Support\' config is enabled", () => ModContent.GetInstance<FishermanNPCConfigServer>().TownNPCsCrossModSupport);
		
		public static Condition AnyUnderground = new("In the Underground, Caverns, Underworld", () => NPCHelper.ZoneAnyUnderground(Main.LocalPlayer));
		public static Condition AnyUndergroundOrHardmode = new("In the Underground, Caverns, Underworld, or Hardmode", () => AnyUnderground.IsMet() || Main.hardMode);
		public static Condition AnyUndergroundNotDesert = new("In the Underground, Caverns, Underworld, and not in the Desert", () => AnyUnderground.IsMet() && !Main.LocalPlayer.ZoneDesert);
		public static Condition InCavernsOrUnderworld = new("In the Underground, Caverns, Underworld", () => Condition.InRockLayerHeight.IsMet() || Condition.InUnderworldHeight.IsMet());
		public static Condition DownedBocOrEoWCrimsonOrHardmode = new("After defeating Brain of Cthulhu, in a Crimson World, or in Hardmode", () => Condition.DownedBrainOfCthulhu.IsMet() || Main.hardMode);
		public static Condition DownedBocOrEoWCorruptionOrHardmode = new("After defeating Eater of Worlds, in a Corruption World, or in Hardmode", () => Condition.DownedEaterOfWorlds.IsMet() || Main.hardMode);
		public static Condition InJungleOrHardmode = new("In the Jungle or Hardmode", () => Main.LocalPlayer.ZoneJungle || Main.hardMode);
		public static Condition InUnderworldOrHardmode = new("In the Underworld or Hardmode", () => Main.LocalPlayer.ZoneUnderworldHeight || Main.hardMode);

		public static string CountTownNPCsS(int number) => $"When there are {number} or more Town NPCs in the world";
		public static Func<bool> CountTownNPCsFb(int number) => () => NPCHelper.CountTownNPCs() >= number;

#pragma warning restore CA2211 // Non-constant fields should not be visible
	}
}