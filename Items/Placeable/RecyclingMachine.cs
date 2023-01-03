using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace FishermanNPC.Items.Placeable
{
	public class RecyclingMachine : ModItem
	{
		public override void SetDefaults()
		{

			Item.maxStack = 9999;
			Item.consumable = true;
			Item.width = 28;
			Item.height = 40;
			Item.value = 5000;
			Item.rare = ItemRarityID.Blue;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.createTile = ModContent.TileType<Tiles.RecyclingMachine>();
		}

		public override void SetStaticDefaults()
		{
			SacrificeTotal = 1;
		}
	}
}