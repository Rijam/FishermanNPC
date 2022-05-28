using System;
using Terraria;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace FishermanNPC.NPCs.TownNPCs
{
	public partial class GlobalNPCs : GlobalNPC
	{
		public override void GetChat(NPC npc, ref string chat)
		{
			int fisherman = NPC.FindFirstNPC(ModContent.NPCType<Fisherman>());
			switch (npc.type)
			{		
				case NPCID.Pirate:
					if (Main.rand.Next(0, 6) == 0 && NPC.CountNPCS(ModContent.NPCType<Fisherman>()) > 0)
					{
						chat = Language.GetTextValue("Mods.FishermanNPC.NPCDialog.Pirate.ExtraChat1") + " " + Main.npc[fisherman].GivenName + " " + Language.GetTextValue("Mods.FishermanNPC.NPCDialog.Pirate.ExtraChat2");
						//Arr, that {Name} the Fisherman is the type to steal me crew's waters and then get me crew into trouble with the authorities. Do not trust such a man.
					}
					break;
				case NPCID.BestiaryGirl: //Zoologist
					if (Main.rand.Next(0, 6) == 0 && NPC.CountNPCS(ModContent.NPCType<Fisherman>()) > 0)
					{
						chat = Language.GetTextValue("Mods.FishermanNPC.NPCDialog.Zoologist.ExtraChat1") + " " + Main.npc[fisherman].GivenName + " " + Language.GetTextValue("Mods.FishermanNPC.NPCDialog.Zoologist.ExtraChat2");
						//I keep telling {Name} the Fisherman the dangers of over-fishing and he just shrugs me off! What nerve!
					}
					break;
				case NPCID.Angler:
					if (Main.rand.Next(0, 6) == 0 && NPC.CountNPCS(ModContent.NPCType<Fisherman>()) > 0)
					{
						chat = Language.GetTextValue("Mods.FishermanNPC.NPCDialog.Angler.ExtraChat1") + " " + Main.npc[fisherman].GivenName + " " + Language.GetTextValue("Mods.FishermanNPC.NPCDialog.Angler.ExtraChat2");
						//Yeah? What about {Name} the Fisherman? He's alright I guess. Back to work with you!
					}
					break;
				case NPCID.Truffle:
					if (Main.rand.Next(0, 6) == 0 && NPC.CountNPCS(ModContent.NPCType<Fisherman>()) > 0)
					{
						chat = Main.npc[fisherman].GivenName + " " + Language.GetTextValue("Mods.FishermanNPC.NPCDialog.Truffle.ExtraChat1");
						//{Name} the Fisherman is nice to me. He doesn't seem to want to eat me.
					}
					break;
				case NPCID.Princess:
					if (Main.rand.Next(0, 6) == 0 && NPC.CountNPCS(ModContent.NPCType<Fisherman>()) > 0)
					{
						chat = Main.npc[fisherman].GivenName + " " + Language.GetTextValue("Mods.FishermanNPC.NPCDialog.Princess.ExtraChat1");
						//{Name} the Fisherman catches all sorts of tasty fish for the town!
					}
					break;
			}
		}
		public override void SetupShop(int type, Chest shop, ref int nextSlot)
		{
			int shopPriceScaling = ModContent.GetInstance<FishermanNPCConfigServer>().ShopPriceScaling;
			float shopMulti = (shopPriceScaling / 100f);
			if (type == ModContent.NPCType<Fisherman>())
			{
				foreach (Item item in shop.item)
				{
					int shopPrice = item.shopCustomPrice ?? 0; //Some hackery with changing the int? type into int
					item.shopCustomPrice = (int?)Math.Round(shopPrice * shopMulti);
				}
			}
		}
		public override void SetStaticDefaults()
		{
			int fishermanType = ModContent.NPCType<NPCs.TownNPCs.Fisherman>(); // Get Fisherman's type
			var anglerHappiness = NPCHappiness.Get(NPCID.Angler); // Get the key into The Angler's happiness
			var truffleHappiness = NPCHappiness.Get(NPCID.Truffle);
			var zoologistHappiness = NPCHappiness.Get(NPCID.BestiaryGirl);
			var pirateHappiness = NPCHappiness.Get(NPCID.Pirate);

			anglerHappiness.SetNPCAffection(fishermanType, AffectionLevel.Love); // Make the Angler love the Fisherman!
			truffleHappiness.SetNPCAffection(fishermanType, AffectionLevel.Like);
			zoologistHappiness.SetNPCAffection(fishermanType, AffectionLevel.Dislike);
			pirateHappiness.SetNPCAffection(fishermanType, AffectionLevel.Dislike);
			//Princess automatically loves the Fisherman
		}
	}
}