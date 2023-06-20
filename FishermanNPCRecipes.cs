using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FishermanNPC
{
	public class FishermanNPCRecipes : ModSystem
	{
		public override void AddRecipes()
		{
			if (ModContent.GetInstance<FishermanNPCConfigServer>().CatchNPCs)
			{
				Recipe.Create(ItemID.FleshBlock, 25)
					.AddIngredient<Items.CaughtFisherman>()
					.AddTile(TileID.MeatGrinder)
					.Register();
			}
		}
	}
}