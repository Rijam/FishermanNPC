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
			int fishermanCount = NPC.CountNPCS(ModContent.NPCType<Fisherman>());
			switch (npc.type)
			{		
				case NPCID.Pirate:
					if (Main.rand.NextBool(6) && fishermanCount > 0)
					{
						chat = Language.GetTextValue(NPCHelper.DialogPath("Pirate") + "ExtraChat", Main.npc[fisherman].GivenName);
						//Arr, that {Name} the Fisherman is the type to steal me crew's waters and then get me crew into trouble with the authorities. Do not trust such a man.
					}
					break;
				case NPCID.BestiaryGirl: //Zoologist
					if (Main.rand.NextBool(6) && fishermanCount > 0)
					{
						if (Main.bloodMoon || Main.moonPhase == 0)
						{
							chat = Language.GetTextValue(NPCHelper.DialogPath("Zoologist") + "ExtraChatTransformed", Main.npc[fisherman].GivenName);
						}
						else
						{
							chat = Language.GetTextValue(NPCHelper.DialogPath("Zoologist") + "ExtraChat", Main.npc[fisherman].GivenName);
							//I keep telling {Name} the Fisherman the dangers of over-fishing and he just shrugs me off! What nerve!
						}
					}
					break;
				case NPCID.Angler:
					if (Main.rand.NextBool(6) && fishermanCount > 0)
					{
						chat = Language.GetTextValue(NPCHelper.DialogPath("Angler") + "ExtraChat", Main.npc[fisherman].GivenName);
						//Yeah? What about {Name} the Fisherman? He's alright I guess. Back to work with you!
					}
					break;
				case NPCID.Truffle:
					if (Main.rand.NextBool(6) && fishermanCount > 0)
					{
						chat = Language.GetTextValue(NPCHelper.DialogPath("Truffle") + "ExtraChat", Main.npc[fisherman].GivenName);
						//{Name} the Fisherman is nice to me. He doesn't seem to want to eat me.
					}
					break;
				case NPCID.Princess:
					if (Main.rand.NextBool(6) && fishermanCount > 0)
					{
						chat = Language.GetTextValue(NPCHelper.DialogPath("Princess") + "ExtraChat", Main.npc[fisherman].GivenName);
						//{Name} the Fisherman catches all sorts of tasty fish for the town!
					}
					break;
			}
		}
		public override void ModifyActiveShop(NPC npc, string shopName, Item[] items)
		{
			int shopPriceScaling = ModContent.GetInstance<FishermanNPCConfigServer>().ShopPriceScaling;
			float shopMulti = (shopPriceScaling / 100f);
			if (npc.type == ModContent.NPCType<Fisherman>())
			{
				foreach (Item item in items)
				{
					if (item is not null)
					{
						int shopPrice = item.shopCustomPrice ?? item.value;
						item.shopCustomPrice = (int?)Math.Round(shopPrice * shopMulti);
					}
				}
			}
		}
		public override void SetStaticDefaults()
		{
			bool townNPCsCrossModSupport = ModContent.GetInstance<FishermanNPCConfigServer>().TownNPCsCrossModSupport;

			int fishermanType = ModContent.NPCType<NPCs.TownNPCs.Fisherman>(); // Get Fisherman's type
			var fishermanHappiness = NPCHappiness.Get(ModContent.NPCType<NPCs.TownNPCs.Fisherman>()); // Get Fisherman's type
			var anglerHappiness = NPCHappiness.Get(NPCID.Angler); // Get the key into The Angler's happiness
			var truffleHappiness = NPCHappiness.Get(NPCID.Truffle);
			var zoologistHappiness = NPCHappiness.Get(NPCID.BestiaryGirl);
			var pirateHappiness = NPCHappiness.Get(NPCID.Pirate);

			anglerHappiness.SetNPCAffection(fishermanType, AffectionLevel.Love); // Make the Angler love the Fisherman!
			truffleHappiness.SetNPCAffection(fishermanType, AffectionLevel.Like);
			zoologistHappiness.SetNPCAffection(fishermanType, AffectionLevel.Dislike);
			pirateHappiness.SetNPCAffection(fishermanType, AffectionLevel.Dislike);
			//Princess automatically loves the Fisherman

			if (ModLoader.TryGetMod("HelpfulNPCs", out Mod helpfulNPCs) && townNPCsCrossModSupport)
			{
				if (helpfulNPCs.TryFind<ModNPC>("FishermanNPC", out ModNPC fisherman2ModNPC))
				{
					var fisherman2Happiness = NPCHappiness.Get(fisherman2ModNPC.Type);

					fisherman2Happiness.SetNPCAffection(fishermanType, AffectionLevel.Like);
					fishermanHappiness.SetNPCAffection(fisherman2ModNPC.Type, AffectionLevel.Like);
				}
			}
		}
	}
}