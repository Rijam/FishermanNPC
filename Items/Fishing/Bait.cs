using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using FishermanNPC.NPCs.TownNPCs;
using FishermanNPC.NPCs;

namespace FishermanNPC.Items.Fishing
{
	public class PlasticWormLure : ModItem
	{
		public override void SetDefaults()
		{
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.width = 10;
			Item.height = 22;
			Item.bait = 5;
			Item.value = 50;
			Item.rare = ItemRarityID.White;
		}

		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 5;
		}
	}

	public class Mealworm : ModItem
	{
		public override void SetDefaults()
		{
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.width = 14;
			Item.height = 10;
			Item.bait = 18;
			Item.value = 250;
			Item.rare = ItemRarityID.Blue;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.autoReuse = true;
			Item.makeNPC = ModContent.NPCType<MealwormCritter>();
		}

		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 5;
		}
	}
	public class RedWorm : ModItem
	{
		public override void SetDefaults()
		{
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.width = 18;
			Item.height = 24;
			Item.bait = 32;
			Item.value = 500;
			Item.rare = ItemRarityID.Green;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.autoReuse = true;
			Item.makeNPC = ModContent.NPCType<RedWormCritter>();
		}

		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 5;
		}
	}
	public class GlowingMushroomChunk : ModItem
	{
		public override void SetDefaults()
		{
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.width = 16;
			Item.height = 14;
			Item.bait = 66;
			Item.value = 1000;
			Item.rare = ItemRarityID.LightRed;
		}

		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 5;
		}
	}
}