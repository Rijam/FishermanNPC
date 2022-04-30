using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

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

		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Bait Box"); //set in lang file
			//	Tried to use the vanilla localization for the "Right click to open" in addition to the custom tooltip. I couldn't get it to work.
			//Tooltip.SetDefault(Language.GetTextValue("Mods.FishermanNPC.ItemTooltip.BaitBox") + "\n" + Language.GetTextValue("CommonItemTooltip.RightClickToOpen"));
			//Tooltip.SetDefault(Language.GetTextValue("CommonItemTooltip.RightClickToOpen"));
		}

		//public override void ModifyTooltips(List<TooltipLine> tooltips)
        //{
			//tooltips.Add(new TooltipLine(Mod, "RightClickToOpen", Language.GetTextValue("CommonItemTooltip.RightClickToOpen")));
			//string tooltip = Language.GetTextValue("Mods.FishermanNPC.ItemTooltip.BaitBox");
			//tooltips.Add(new TooltipLine(Mod, "Tooltip", tooltip));
		//}

		public override bool CanRightClick()
		{
			return true;
		}

		public virtual void CrateLoot(Player player)
		{

			int random;
			for (int i = 0; i < 10; i++) //roll 10 times
			{
				random = Main.rand.Next(9);
				if (random == 8)
				{
					player.QuickSpawnItemDirect(player.GetSource_OpenItem(Type), ModContent.ItemType<RedWorm>(), Main.rand.Next(0, 3));
				}
				if (random == 7)
				{
					player.QuickSpawnItemDirect(player.GetSource_OpenItem(Type), ModContent.ItemType<Mealworm>(), Main.rand.Next(0, 3));
				}
				if (random == 6)
                {
					player.QuickSpawnItemDirect(player.GetSource_OpenItem(Type), ModContent.ItemType<PlasticWormLure>(), Main.rand.Next(0, 3));
				}
				if (random == 5)
				{
					player.QuickSpawnItemDirect(player.GetSource_OpenItem(Type), ItemID.Snail, Main.rand.Next(0, 3));
				}
				if (random == 4)
				{
					player.QuickSpawnItemDirect(player.GetSource_OpenItem(Type), ItemID.EnchantedNightcrawler, Main.rand.Next(0, 3));
				}
				if (random == 3)
				{
					player.QuickSpawnItemDirect(player.GetSource_OpenItem(Type), ItemID.Worm, Main.rand.Next(0, 3));
				}
				if (random == 2)
				{
					player.QuickSpawnItemDirect(player.GetSource_OpenItem(Type), ItemID.MasterBait, Main.rand.Next(0, 3));
				}
				if (random == 1)
				{
					player.QuickSpawnItemDirect(player.GetSource_OpenItem(Type), ItemID.JourneymanBait, Main.rand.Next(0, 3));
				}
				if (random == 0)
				{
					player.QuickSpawnItemDirect(player.GetSource_OpenItem(Type), ItemID.ApprenticeBait, Main.rand.Next(0, 3));
				}
			}
		}
		public override void RightClick(Player player)
		{
			CrateLoot(player);
		}
	}
}