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
		/*public override bool IsLoadingEnabled(Mod mod)
		{
			return ModContent.GetInstance<FishermanNPCConfigServer>().LoadDebugItems;
		}*/
		public override string Texture => "FishermanNPC/NPCs/TownNPCs/Fisherman";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault((string)Language.GetText("Mods.FishermanNPC.NPCName.Fisherman"));
			Tooltip.SetDefault("'" + Language.GetText("Mods.FishermanNPC.NPCDialog.Fisherman.DefaultChat1") + "'");
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 25));
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
			Item.makeNPC = (short)ModContent.NPCType<Fisherman>();
			Item.tileBoost += 20;
		}

		public override bool CanUseItem(Player player)
		{
			Vector2 mousePos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			return (NPC.CountNPCS(ModContent.NPCType<Fisherman>()) < 1 && !Collision.SolidCollision(mousePos, player.width, player.height));
		}

		public override void OnConsumeItem(Player player)
		{

			/*if (!ModContent.GetInstance<FishermanNPCConfigServer>().CatchNPCs && Main.netMode == NetmodeID.SinglePlayer) //So it still spawns the Town NPC if the config is off. If it is on, it automatically does this.
			{
				Vector2 mousePos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
				NPC.NewNPC((int)mousePos.X, (int)mousePos.Y, ModContent.NPCType<Fisherman>());
				//NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, ModContent.NPCType<Fisherman>(), 0f, 0f, 0f, 0, 0, 0);
			}*/
			SpawnText();
		}
		private static void SpawnText()
        {
			string chatmessage = Language.GetTextValue("Mods.FishermanNPC.UI.Fisherman.CapturedSpawnText");
			if (Main.netMode != NetmodeID.Server)
			{
				Main.NewText(chatmessage, 50, 125, 255);
			}
			else
			{
				NetworkText text = NetworkText.FromLiteral(chatmessage);
				ChatHelper.BroadcastChatMessage(text, new Color(50, 125, 255));
			}
		}
	}
}