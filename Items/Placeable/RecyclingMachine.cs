using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace FishermanNPC.Items.Placeable
{
	public class RecyclingMachine : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults()
		{

			Item.maxStack = Item.CommonMaxStack;
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
	}
}