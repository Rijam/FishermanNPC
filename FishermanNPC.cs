using FishermanNPC.NPCs.TownNPCs;
using Microsoft.Xna.Framework.Graphics;
using System;
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
		}

		public override void Unload()
		{
			ConfigServer = null;
			Instance = null;
		}
        public override void PostSetupContent()
        {
            if (ModLoader.TryGetMod("Census", out Mod censusMod))
            {
                censusMod.Call("TownNPCCondition", ModContent.NPCType<Fisherman>(), Language.GetTextValue("Mods.FishermanNPC.Crossmod.Census.Fisherman"));
            }
        }
		//Adapted from absoluteAquarian's GraphicsLib
		public override object Call(params object[] args)
		{
			if (args is null)
				throw new ArgumentNullException(nameof(args));

			if (args[0] is not string function)
				throw new ArgumentException("Expected a function name for the first argument");

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
				case "GetStatusShopCycle":
					return NPCs.NPCHelper.StatusShopCycle();
				case "GetStatusShop1": // Legacy
					Logger.Warn($"Function \"{function}\" has been removed by FishermanNPC. Please use \"GetStatusShopCycle\"");
					return false;
				case "GetStatusShop2": // Legacy
					Logger.Warn($"Function \"{function}\" has been removed by FishermanNPC. Please use \"GetStatusShopCycle\"");
					return false;
				default:
					throw new ArgumentException($"Function \"{function}\" is not defined by FishermanNPC");
			}
		}
	}
}