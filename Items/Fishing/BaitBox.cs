using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;

namespace FishermanNPC.Items.Fishing
{
	public class BaitBox : ModItem
	{
		public override void SetDefaults()
		{

			Item.maxStack = 999;
			Item.consumable = true;
			Item.width = 24;
			Item.height = 24;
			Item.value = 50000;
			Item.rare = ItemRarityID.Orange;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 2;
		}

		public override bool CanRightClick()
		{
			return true;
		}

		public override void ModifyItemLoot(ItemLoot itemLoot)
		{
			IItemDropRule[] baitTypes = new IItemDropRule[] {
				ItemDropRule.Common(ModContent.ItemType<RedWorm>(), 1, 0, 3),
				ItemDropRule.Common(ModContent.ItemType<Mealworm>(), 1, 0, 3),
				ItemDropRule.Common(ModContent.ItemType<PlasticWormLure>(), 1, 0, 3),
				ItemDropRule.Common(ItemID.Snail, 1, 0, 3),
				ItemDropRule.Common(ItemID.EnchantedNightcrawler, 1, 0, 3),
				ItemDropRule.Common(ItemID.Worm, 1, 0, 3),
				ItemDropRule.Common(ItemID.MasterBait, 1, 0, 3),
				ItemDropRule.Common(ItemID.JourneymanBait, 1, 0, 3),
				ItemDropRule.Common(ItemID.ApprenticeBait, 1, 0, 3)
			};
			// Choose one item, get 0-3 of them, roll 10 times total.
			itemLoot.Add(new OneFromRulesRule(1, baitTypes));
			itemLoot.Add(new OneFromRulesRule(1, baitTypes));
			itemLoot.Add(new OneFromRulesRule(1, baitTypes));
			itemLoot.Add(new OneFromRulesRule(1, baitTypes));
			itemLoot.Add(new OneFromRulesRule(1, baitTypes));
			itemLoot.Add(new OneFromRulesRule(1, baitTypes));
			itemLoot.Add(new OneFromRulesRule(1, baitTypes));
			itemLoot.Add(new OneFromRulesRule(1, baitTypes));
			itemLoot.Add(new OneFromRulesRule(1, baitTypes));
			itemLoot.Add(new OneFromRulesRule(1, baitTypes));
		}
	}
}