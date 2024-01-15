using System;
using System.ComponentModel;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace FishermanNPC
{
	/// <summary>
	/// FishermanNPCConfigServer has Server-wide effects. Things that happen on the server, on the world, or influence autoload go here
	/// ConfigScope.ServerSide ModConfigs are SHARED from the server to all clients connecting in MP.
	/// </summary>
	//Fisherman NPC Server Options
	public class FishermanNPCConfigServer : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

		[Header("ServerHeader")]
		//[c/00FF00:Server Options]

		//Sell Fisherman NPC Modded Items
		//When Enabled: The Fisherman sells the custom items added by this mod. Disable to remove the modded items.
		[DefaultValue(true)]
		public bool SellModdedItems { get; set; }

		//Sell Bait
		//When Enabled: The Fisherman will sell bait. Disable to remove bait from the shop.
		[DefaultValue(true)]

		public bool SellBait { get; set; }
		//Sell Fish
		//When Enabled: The Fisherman will sell fish. Disable to remove fish from the shop.
		[DefaultValue(true)]
		public bool SellFish { get; set; }

		//Sell Fishing Rods
		//When Enabled: The Fisherman will sell fishing rods. Disable to remove fishing rods from the shop.
		[DefaultValue(true)]
		public bool SellFishingRods { get; set; }

		//Sell Extra Items
		//When Enabled: The Fisherman will sell extra fishing related items. Disable to remove the extra items from the shop.
		[DefaultValue(true)]
		public bool SellExtraItems { get; set; }

		//Shop Price Scaling
		//Sets the scaling for the prices in the Fisherman's shop. 50 means half price, 200 means double price.
		[Increment(1)]
		[Range(50, 200)]
		[DefaultValue(100)]
		[Slider]
		public int ShopPriceScaling { get; set; }

		//Fisherman Cross Mod Support
		//This option toggles if the Fisherman will sell items from other mods, if he will have cross mod dialog, and cross mod happiness.
		[DefaultValue(true)]
		public bool TownNPCsCrossModSupport { get; set; }

		//Load Debug Items
		//When Disabled: The Fisherman CAN NOT be caught. Enable to catch the Fisherman. Requires a Reload.
		[ReloadRequired]
		[DefaultValue(false)]
		public bool CatchNPCs { get; set; }

		/*
		//Load Debug Items
		//When Disabled: The debug items WILL NOT be loaded. Enable to load the debug items. Requires a Reload.
		[ReloadRequired]
		[DefaultValue(false)]
		public bool LoadDebugItems { get; set; }
		*/

		/* Not written by Rijam*/
		public static bool IsPlayerLocalServerOwner(int whoAmI)
		{
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				return Netplay.Connection.Socket.GetRemoteAddress().IsLocalHost();
			}

			for (int i = 0; i < Main.maxPlayers; i++)
			{
				RemoteClient client = Netplay.Clients[i];
				if (client.State == 10 && i == whoAmI && client.Socket.GetRemoteAddress().IsLocalHost())
				{
					return true;
				}
			}
			return false;
		}

		public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref NetworkText message)
		{
			if (Main.netMode == NetmodeID.SinglePlayer)
			{
				return true;
			}

			if (!IsPlayerLocalServerOwner(whoAmI))
			{
				message = NetworkText.FromKey("Mods.FishermanNPC.Configs.FishermanNPCConfigServer.MultiplayerMessage");
				// message = Language.GetTextValue("Mods.FishermanNPC.Configs.FishermanNPCConfigServer.MultiplayerMessage");
				return false;
			}
			return base.AcceptClientChanges(pendingConfig, whoAmI, ref message);
		}
		/* */
	}
}