using System;
using System.ComponentModel;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace FishermanNPC
{
	/// <summary>
	/// ExampleConfigServer has Server-wide effects. Things that happen on the server, on the world, or influence autoload go here
	/// ConfigScope.ServerSide ModConfigs are SHARED from the server to all clients connecting in MP.
	/// </summary>
	[Label("$Mods.FishermanNPC.Config.Server.LabelBig")]
	//Fisherman NPC Server Options
	public class FishermanNPCConfigServer : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

		[Header("$Mods.FishermanNPC.Config.Server.Header")]
		//[c/00FF00:Server Options]

		[Label("$Mods.FishermanNPC.Config.Server.Label1")]
		//Sell Fisherman NPC Modded Items
		[Tooltip("$Mods.FishermanNPC.Config.Server.Tooltip1")]
		//When Enabled: The Fisherman sells the custom items added by this mod. Disable to remove the modded items.
		[DefaultValue(true)]
		public bool SellModdedItems { get; set; }

		[Label("$Mods.FishermanNPC.Config.Server.Label2")]
		//Sell Bait
		[Tooltip("$Mods.FishermanNPC.Config.Server.Tooltip2")]
		//When Enabled: The Fisherman will sell bait. Disable to remove bait from the shop.
		[DefaultValue(true)]
		public bool SellBait { get; set; }

		[Label("$Mods.FishermanNPC.Config.Server.Label3")]
		//Sell Fish
		[Tooltip("$Mods.FishermanNPC.Config.Server.Tooltip3")]
		//When Enabled: The Fisherman will sell fish. Disable to remove fish from the shop.
		[DefaultValue(true)]
		public bool SellFish { get; set; }

		[Label("$Mods.FishermanNPC.Config.Server.Label4")]
		//Sell Fishing Rods
		[Tooltip("$Mods.FishermanNPC.Config.Server.Tooltip4")]
		//When Enabled: The Fisherman will sell fishing rods. Disable to remove fishing rods from the shop.
		[DefaultValue(true)]
		public bool SellFishingRods { get; set; }

		[Label("$Mods.FishermanNPC.Config.Server.Label5")]
		//Sell Extra Items
		[Tooltip("$Mods.FishermanNPC.Config.Server.Tooltip5")]
		//When Enabled: The Fisherman will sell extra fishing related items. Disable to remove the extra items from the shop.
		[DefaultValue(true)]
		public bool SellExtraItems { get; set; }

		[Label("$Mods.FishermanNPC.Config.Server.Label6")]
		//Shop Price Scaling
		[Tooltip("$Mods.FishermanNPC.Config.Server.Tooltip6")]
		//Sets the scaling for the prices in the Fisherman's shop. 50 means half price, 200 means double price.
		[Increment(1)]
		[Range(50, 200)]
		[DefaultValue(100)]
		[Slider]
		public int ShopPriceScaling { get; set; }

		[Label("$Mods.FishermanNPC.Config.Server.Label9")]
		//Fisherman Cross Mod Support
		[Tooltip("$Mods.FishermanNPC.Config.Server.Tooltip9")]
		//This option toggles if the Fisherman will sell items from other mods, if he will have cross mod dialog, and cross mod happiness.
		[DefaultValue(true)]
		public bool TownNPCsCrossModSupport { get; set; }

		[Label("$Mods.FishermanNPC.Config.Server.Label8")]
		//Load Debug Items
		[Tooltip("$Mods.FishermanNPC.Config.Server.Tooltip8")]
		//When Disabled: The Fisherman CAN NOT be caught. Enable to catch the Fisherman. Requires a Reload.
		[ReloadRequired]
		[DefaultValue(false)]
		public bool CatchNPCs { get; set; }

		[Label("$Mods.FishermanNPC.Config.Server.Label7")]
		//Load Debug Items
		[Tooltip("$Mods.FishermanNPC.Config.Server.Tooltip7")]
		//When Disabled: The debug items WILL NOT be loaded. Enable to load the debug items. Requires a Reload.
		[ReloadRequired]
		[DefaultValue(false)]
		public bool LoadDebugItems { get; set; }
	}
}