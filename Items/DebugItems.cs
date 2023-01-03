using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using FishermanNPC;

namespace FishermanNPC.Items
{
	public class DebugAnglerQuestChanger : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod)
		{
			return ModContent.GetInstance<FishermanNPCConfigServer>().LoadDebugItems;
		}
		public override string Texture => "Terraria/Images/Item_" + ItemID.BlueSolution;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("[c/ff0000:Debug] - Angler Quest Changer");
			Tooltip.SetDefault("Changes the number of Angler quests you have completed\nLeft click to add one\nRight click to remove one");
		}

		public override void SetDefaults()
		{
			Item.color = Color.RoyalBlue;
			Item.width = 14;
			Item.height = 14;
			Item.maxStack = 9999;
			Item.rare = ItemRarityID.White;
			Item.value = 0;
			Item.useStyle = ItemUseStyleID.EatFood;
			Item.useAnimation = 10;
			Item.useTime = 10;
			Item.useTurn = true;
			Item.UseSound = SoundID.Item9;
			Item.consumable = true;
		}
		public override bool? UseItem(Player player)
		{
			Main.player[Main.myPlayer].anglerQuestsFinished++;
			return true;
		}
		public override bool CanRightClick()
		{
			return true;
		}
		public override void RightClick(Player player)
		{
			//Don't allow the number to become negative.
			if (Main.player[Main.myPlayer].anglerQuestsFinished > 0)
            {
				Main.player[Main.myPlayer].anglerQuestsFinished--;
			}
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
			string anglerQuestsCompleted = Main.player[Main.myPlayer].anglerQuestsFinished.ToString();
			tooltips.Add(new TooltipLine(Mod, "AnglerQuestsCompleted", "You have completed " + anglerQuestsCompleted + " Angler quests"));
		}
	}
}