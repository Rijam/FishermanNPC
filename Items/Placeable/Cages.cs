using FishermanNPC.Items.Fishing;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FishermanNPC.Items.Placeable
{
	public class MealwormCage : ModItem
	{
		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.MealwormCage>();
			Item.width = 12;
			Item.height = 12;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.Terrarium)
				.AddIngredient<Mealworm>()
				.Register();
		}
	}

	public class RedWormCage : ModItem
	{
		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.RedWormCage>();
			Item.width = 12;
			Item.height = 12;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.Terrarium)
				.AddIngredient<RedWorm>()
				.Register();
		}
	}

	public class PlasticWormLureCage : ModItem
	{
		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.PlasticWormLureCage>();
			Item.width = 12;
			Item.height = 12;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.Terrarium)
				.AddIngredient<PlasticWormLure>()
				.Register();
		}
	}
}