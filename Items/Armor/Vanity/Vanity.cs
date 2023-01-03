using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace FishermanNPC.Items.Armor.Vanity
{
	[AutoloadEquip(EquipType.Body)]
	public class Fisherman_Vanity_Shirt : ModItem
	{
		public override void SetStaticDefaults()
		{
			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 20;
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.Blue;
			Item.vanity = true;
			Item.defense = 0;
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class Fisherman_Vanity_Pants : ModItem
	{
		public override void SetStaticDefaults()
		{
			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 20;
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.Blue;
			Item.vanity = true;
			Item.defense = 0;
		}
	}
}