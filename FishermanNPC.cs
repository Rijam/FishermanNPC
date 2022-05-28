using FishermanNPC.NPCs.TownNPCs;
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
				case "CatchNPCs":
					return ModContent.GetInstance<FishermanNPCConfigServer>().CatchNPCs;
				case "GetStatusShop1":
					return NPCs.NPCHelper.StatusShop1();
				case "GetStatusShop2":
					return NPCs.NPCHelper.StatusShop2();
				default:
					throw new ArgumentException($"Function \"{function}\" is not defined by FishermanNPC");
			}
		}
	}
}