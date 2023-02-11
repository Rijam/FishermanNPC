using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using FishermanNPC.NPCs.TownNPCs;

namespace FishermanNPC.Items
{
	//Code adapted from Fargo's Mutant Mod (CaughtNPCItem.cs)
	//and code adapted from Alchemist NPC (BrewerHorcrux.cs and similar)
	public class CaughtFisherman : ModItem
	{
		public override string Texture => "FishermanNPC/NPCs/TownNPCs/Fisherman";
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault((string)Language.GetText("Mods.FishermanNPC.NPCName.Fisherman"));
			// Tooltip.SetDefault("'" + Language.GetText(NPCs.NPCHelper.DialogPath("Fisherman") + "Default1") + "'");
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 25));
			Item.ResearchUnlockCount = 3;
		}

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = 10;
			Item.value = 0;
			Item.rare = ItemRarityID.Blue;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.noUseGraphic = true;
			Item.consumable = true;
			Item.UseSound = SoundID.Item44;
			Item.makeNPC = ModContent.NPCType<Fisherman>();
			Item.tileBoost += 20;
		}

		public override bool CanUseItem(Player player)
		{
			Vector2 mousePos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			return (NPC.CountNPCS(ModContent.NPCType<Fisherman>()) < 1 && !Collision.SolidCollision(mousePos, player.width, player.height));
		}

		public override void OnConsumeItem(Player player)
		{
			SpawnText();
		}
		private static void SpawnText()
        {
			string chatMessage = Language.GetTextValue("Mods.FishermanNPC.UI.Fisherman.CapturedSpawnText");
			if (Main.netMode != NetmodeID.Server)
			{
				Main.NewText(chatMessage, 50, 125, 255);
			}
			else
			{
				NetworkText text = NetworkText.FromLiteral(chatMessage);
				ChatHelper.BroadcastChatMessage(text, new Color(50, 125, 255));
			}
		}
	}
}