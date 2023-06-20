using FishermanNPC.NPCs;
using FishermanNPC.NPCs.TownNPCs;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace FishermanNPC
{
	public class FishermanNPC : Mod
	{
		internal static FishermanNPCConfigServer ConfigServer;
		internal static FishermanNPC Instance;

		public override void Load()
		{
			if (ModLoader.TryGetMod("Wikithis", out Mod wikithis) && !Main.dedServ)
			{
				// Special thanks to Wikithis for having an outdated mod calls description -_-
				// Actual special thanks to Confection Rebaked for having the correct format.
				wikithis.Call("AddModURL", this, "terrariamods.wiki.gg$User:Rijam/Fisherman_NPC");
				wikithis.Call("AddWikiTexture", this, ModContent.Request<Texture2D>("FishermanNPC/icon_small"));
			}

			if (ModLoader.TryGetMod("ItemCheckBlacklist", out Mod itemCheckBlacklist))
			{
				itemCheckBlacklist.Call("ItemCheckBlacklist", new List<int>() { ModContent.ItemType<Items.DebugAnglerQuestChanger>(), ModContent.ItemType<Items.CaughtFisherman>() });
			}
		}


		public override void Unload()
		{
			ConfigServer = null;
			Instance = null;
		}
		public override void PostSetupContent()
		{
			/*if (ModLoader.TryGetMod("Census", out Mod censusMod))
			{
				censusMod.Call("TownNPCCondition", ModContent.NPCType<Fisherman>(), Language.GetTextValue("Mods.FishermanNPC.Crossmod.Census.Fisherman"));
			}*/
		}
		//Adapted from absoluteAquarian's GraphicsLib
		public override object Call(params object[] args)
		{
			if (args is null)
				throw new ArgumentNullException(nameof(args));

			if (args[0] is not string function)
				throw new ArgumentException("Expected a function name for the first argument");

			void CheckArgsLength(int expected, params string[] argNames)
			{
				if (args.Length != expected)
					throw new ArgumentOutOfRangeException($"Expected {expected} arguments for Mod.Call(\"{function}\", {string.Join(",", argNames)}), got {args.Length} arguments instead");
			}

			switch (function)
			{
				case "SellModdedItems":
					return ModContent.GetInstance<FishermanNPCConfigServer>().SellModdedItems;
				case "SellBait":
					return ModContent.GetInstance<FishermanNPCConfigServer>().SellBait;
				case "SellFish":
					return ModContent.GetInstance<FishermanNPCConfigServer>().SellFish;
				case "SellFishingRods":
					return ModContent.GetInstance<FishermanNPCConfigServer>().SellFishingRods;
				case "SellExtraItems":
					return ModContent.GetInstance<FishermanNPCConfigServer>().SellExtraItems;
				case "shopMulti":
					return (ModContent.GetInstance<FishermanNPCConfigServer>().ShopPriceScaling / 100f);
				case "TownNPCsCrossModSupport":
					return ModContent.GetInstance<FishermanNPCConfigServer>().TownNPCsCrossModSupport;
				case "CatchNPCs":
					return ModContent.GetInstance<FishermanNPCConfigServer>().CatchNPCs;
				case "GetStatusShopCycle": // Legacy
					Logger.Warn($"Function \"{function}\" has been removed by FishermanNPC. Please use \"AddToShop\"");
					return -1;
				case "GetStatusShop1": // Legacy
					Logger.Warn($"Function \"{function}\" has been removed by FishermanNPC. Please use \"AddToShop\"");
					return false;
				case "GetStatusShop2": // Legacy
					Logger.Warn($"Function \"{function}\" has been removed by FishermanNPC. Please use \"AddToShop\"");
					return false;
				case "GetCondition":
					CheckArgsLength(2, new string[] { args[0].ToString(), args[1].ToString() });
					return args[1].ToString() switch
					{
						"SellModdedItems" => ShopConditions.SellModdedItems,
						"SellBait" => ShopConditions.SellBait,
						"SellFish" => ShopConditions.SellFish,
						"SellFishingRods" => ShopConditions.SellFishingRods,
						"SellExtraItems" => ShopConditions.SellExtraItems,
						"TownNPCsCrossModSupport" => ShopConditions.TownNPCsCrossModSupport,
						"AnyUnderground" => ShopConditions.AnyUnderground,
						"AnyUndergroundOrHardmode" => ShopConditions.AnyUndergroundOrHardmode,
						"AnyUndergroundNotDesert" => ShopConditions.AnyUndergroundNotDesert,
						"InCavernsOrUnderworld" => ShopConditions.InCavernsOrUnderworld,
						"DownedBocOrEoWCrimsonOrHardmode" => ShopConditions.DownedBocOrEoWCrimsonOrHardmode,
						"DownedBocOrEoWCorruptionOrHardmode" => ShopConditions.DownedBocOrEoWCorruptionOrHardmode,
						"InJungleOrHardmode" => ShopConditions.InJungleOrHardmode,
						"InUnderworldOrHardmode" => ShopConditions.InUnderworldOrHardmode,
						_ => throw new ArgumentException($"Argument \"{args[1]}\" of Function \"{function}\" is not defined by Fisherman NPC"),
					};
				case "AddToShop":
					switch (args[1].ToString())
					{
						case "DefaultPrice":
							CheckArgsLength(5, new string[] { args[0].ToString(), args[1].ToString(), args[2].ToString(), args[3].ToString(), args[4].ToString() });
							// string shop, int item, List<Condition> condition
							return FishermanShops.SetShopItem(args[2].ToString(), (int)args[3], (List<Condition>)args[4]);
						case "CustomPrice":
							CheckArgsLength(6, new string[] { args[0].ToString(), args[1].ToString(), args[2].ToString(), args[3].ToString(), args[4].ToString(), args[5].ToString() });
							// string shop, int item, List<Condition> condition, int customPrice
							return FishermanShops.SetShopItem(args[2].ToString(), (int)args[3], (List<Condition>)args[4], (int)args[5]);
						case "WithMulti":
							CheckArgsLength(6, new string[] { args[0].ToString(), args[1].ToString(), args[2].ToString(), args[3].ToString(), args[4].ToString(), args[5].ToString() });
							// string shop, int item, List<Condition> condition, float priceMulti
							return FishermanShops.SetShopItem(args[2].ToString(), (int)args[3], (List<Condition>)args[4], (float)args[5]);
						default:
							throw new ArgumentException($"Argument \"{args[1]}\" of Function \"{function}\" is not defined by Fisherman NPC");
					}
				case "DisableInternalCrossModSupport":
					CheckArgsLength(2, new string[] { args[0].ToString(), args[1].ToString() });
					Logger.DebugFormat("Internal cross mod support for {0} has been disabled.", args[1].ToString());
					return args[1].ToString() switch
					{
						"CalamityMod" => FishermanShops.CalamityMod = false,
						"ThoriumMod" => FishermanShops.ThoriumMod = false,
						_ => throw new ArgumentException($"Argument \"{args[1]}\" of Function \"{function}\" is not defined by Bosses As NPCs"),
					};
				default:
					throw new ArgumentException($"Function \"{function}\" is not defined by Fisherman NPC");
			}
		}
	}
}