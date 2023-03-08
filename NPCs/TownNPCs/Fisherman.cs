using FishermanNPC.Items.Fishing;
using FishermanNPC.Items.Armor.Vanity;
using FishermanNPC.Items.Placeable;
using FishermanNPC.Projectiles;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using System.Collections.Generic;
using Terraria.GameContent;

namespace FishermanNPC.NPCs.TownNPCs
{
	[AutoloadHead]
	public class Fisherman : ModNPC
	{
		private static int ShimmerHeadIndex;
		private static Profiles.StackedNPCProfile NPCProfile;

		public override void Load()
		{
			// Adds our Shimmer Head to the NPCHeadLoader.
			ShimmerHeadIndex = Mod.AddNPCHeadTexture(Type, GetType().Namespace.Replace('.', '/') + "/Shimmered/" + Name + "_Head");
		}

		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = 25;
			NPCID.Sets.ExtraFramesCount[Type] = 9;
			NPCID.Sets.AttackFrameCount[Type] = 4;
			NPCID.Sets.DangerDetectRange[Type] = 700;
			NPCID.Sets.AttackType[Type] = 1;
			NPCID.Sets.AttackTime[Type] = 90;
			NPCID.Sets.AttackAverageChance[Type] = 30;
			NPCID.Sets.HatOffsetY[Type] = 3;
			NPCID.Sets.ShimmerTownTransform[NPC.type] = true;

			// Influences how the NPC looks in the Bestiary
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new (0)
			{
				Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
				Direction = -1
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

			NPC.Happiness
				.SetBiomeAffection<OceanBiome>(AffectionLevel.Love)
				.SetBiomeAffection<ForestBiome>(AffectionLevel.Like)
				.SetBiomeAffection<DesertBiome>(AffectionLevel.Dislike)
				.SetNPCAffection(NPCID.Angler, AffectionLevel.Love)
				.SetNPCAffection(NPCID.Truffle, AffectionLevel.Like)
				.SetNPCAffection(NPCID.Mechanic, AffectionLevel.Like)
				.SetNPCAffection(NPCID.Pirate, AffectionLevel.Dislike)
				.SetNPCAffection(NPCID.Nurse, AffectionLevel.Dislike)
				//Princess is automatically set
			; // < Mind the semicolon!

			NPCProfile = new Profiles.StackedNPCProfile(
				new Profiles.DefaultNPCProfile(Texture, NPCHeadLoader.GetHeadSlot(HeadTexture), Texture + "_Alt"),
				new Profiles.DefaultNPCProfile(GetType().Namespace.Replace('.', '/') + "/Shimmered/" + Name, ShimmerHeadIndex, GetType().Namespace.Replace('.', '/') + "/Shimmered/" + Name + "_Alt")
			);
		}

		public override void SetDefaults()
		{
			NPC.townNPC = true;
			NPC.friendly = true;
			NPC.width = 18;
			NPC.height = 40;
			NPC.aiStyle = 7;
			NPC.damage = 10;
			NPC.defense = 15;
			NPC.lifeMax = 250;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.5f;
			AnimationType = NPCID.Guide;
			Main.npcCatchable[NPC.type] = ModContent.GetInstance<FishermanNPCConfigServer>().CatchNPCs;
			NPC.catchItem = ModContent.GetInstance<FishermanNPCConfigServer>().CatchNPCs ? ModContent.ItemType<Items.CaughtFisherman>() : -1;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
				new FlavorTextBestiaryInfoElement(NPCHelper.BestiaryPath(Name)),
				new FlavorTextBestiaryInfoElement(NPCHelper.LoveText(Name) + NPCHelper.LikeText(Name) + NPCHelper.DislikeText(Name) + NPCHelper.HateText(Name))
			});
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode != NetmodeID.Server && NPC.life < 0)
			{
				if (NPC.IsShimmerVariant)
				{
					if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
					{
						Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Head_Alt_Shimmered").Type, 1f);
					}
					else
					{
						Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Head_Shimmered").Type, 1f);
					}
					for (int k = 0; k < 2; k++)
					{
						Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Arm_Shimmered").Type, 1f);
						Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Leg_Shimmered").Type, 1f);
					}
				}
				else
				{
					if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
					{
						Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Head_Alt").Type, 1f);
					}
					else
					{
						Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Head").Type, 1f);
					}
					for (int k = 0; k < 2; k++)
					{
						Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Arm").Type, 1f);
						Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Leg").Type, 1f);
					}
				}
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs)
		{
			if (NPC.savedAngler && numTownNPCs >= 5 && NPC.CountNPCS(ModContent.NPCType<Fisherman>()) < 1)
			{
				return true;
			}
			return false;
		}

		public override ITownNPCProfile TownNPCProfile()
		{
			return NPCProfile;
		}

		public override List<string> SetNPCNameList()
		{
			List<string> maleNames =  new()
			{
				"Willy", "Mark", "Charles", "Richard", "Michael", "Davis", "Ray", "Max", "Sherman", "Ike", "Pete", "Hermann", "Colin", "Paul", "Nathaniel", "Malcolm", "Keith", "Jarrell", "Isaac", "Spencer"
			};
			List<string> femaleNames = new()
			{
				"Catherine", "Connie", "Elizabeth", "Vicky", "Wilma", "Cindy", "Rosalind", "Michelle", "Colleen", "Elsie", "Josephine", "Sharlene", "Rose", "Carolyn", "Nancy", "Lana", "Jody", "Claire", "Christine", "Deborah"
			};

			return NPC.townNpcVariationIndex switch
			{
				0 => maleNames,
				1 => femaleNames, // Shimmered. Doesn't seem to work because NPC.townNpcVariationIndex isn't set when choosing a name?
				_ => maleNames
			};
		}

		public override string GetChat()
		{
			string path = NPCHelper.DialogPath(Name);
			WeightedRandom<string> chat = new ();

			bool townNPCsCrossModSupport = ModContent.GetInstance<FishermanNPCConfigServer>().TownNPCsCrossModSupport;
			int fisherman = NPC.FindFirstNPC(ModContent.NPCType<Fisherman>());

			for (int i = 1; i <= 5; i++)
			{
				chat.Add(Language.GetTextValue(path + "Default" + i));
			}
			//Greetings. Care to do some fishin'?
			//I got all the supplies you'd need if you want to do some fishin'.
			//Fishing rods? Bait? Hooks? You want it?
			//Make sure you have bait on your hook.
			//There are all sorts of strange fish in these waters; see what you can catch!

			chat.Add(Language.GetTextValue(path + "Default6").Replace("{0}", Main.npc[fisherman].GivenName));
			//Ahoy there. My name is {Name}

			chat.Add(Language.GetTextValue(path + "Rare1"), 0.1);
			//Hm... I seem to have misplaced my rod...

			chat.Add(Language.GetTextValue(path + "Rare2"), 0.2);
			//You were paying way too much for worms. I'm your new worm guy.

			if (NPC.life < NPC.lifeMax * 0.5)
			{
				chat.Add(Language.GetTextValue(path + "Hurt"), 10.0);
				//That's gonna leave a mark.
			}
			if (Main.dayTime)
			{
				chat.Add(Main.raining ? Language.GetTextValue(path + "DayTimeRaining") : Language.GetTextValue(path + "DayTime"));
				//Rain is the perfect time to do some fishin'!
				//A nice sunny day today.
			}
			else
			{
				chat.Add(Main.raining ? Language.GetTextValue(path + "NightTimeRaining") : Language.GetTextValue(path + "NightTime"));
				//Careful out there. There's a nasty storm out tonight!
				//Time to get some shut-eye.
			}
			if (NPC.homeless)
			{
				chat.Add(Language.GetTextValue(path + "Homeless"));
				//Could you provide me with a vessel of some sort? A man needs a place to sleep!
			}
			else
			{
				chat.Add(Language.GetTextValue(path + "HasHome"));
				//Now I can really set up shop! Make sure you purchase some goods!
			}
			if (Main.LocalPlayer.wingTimeMax > 0)
			{
				chat.Add(Language.GetTextValue(path + "PlayerHasWings"));
				//Aye, I saw a large fish with wings once...
			}
			if (Main.LocalPlayer.HasItem(ItemID.LadyBug))
			{
				chat.Add(Language.GetTextValue(path + "PlayerHasLadyBug"));
				//I wouldn't use that lady bug as bait if I were you. There are tales of an evil curse being put on you if you do...
			}
			if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
			{
				chat.Add(Language.GetTextValue(path + "Party1"), 2.0);
				//That's the biggest catch I've seen yet! Time to celebrate.
				chat.Add(Language.GetTextValue(path + "Party2"), 2.0);
				//I cooked me up some Bumblebee Tuna. Those things are tasty!
			}
			if (Main.bloodMoon)
			{
				chat.Add(Language.GetTextValue(path + "BloodMoon"), 2.0);
				//I hear that there are some crazy creatures swimming in the water tonight. Fish them up!
			}
			if (!NPC.savedAngler) //spawn in the Fisherman before meeting the requirements
			{
				chat.Add(Language.GetTextValue(path + "HowDidIGetHere"), 2.0);
				//Some sort of storm must've pushed me off course. I know I didn't intent on being here at this time.
			}
			if (!ModContent.GetInstance<FishermanNPCConfigServer>().SellBait
				&& !ModContent.GetInstance<FishermanNPCConfigServer>().SellFish
				&& !ModContent.GetInstance<FishermanNPCConfigServer>().SellFishingRods
				&& !ModContent.GetInstance<FishermanNPCConfigServer>().SellExtraItems) //if the config disables everything that he can sell
			{
				chat.Add(Language.GetTextValue(path + "SellNothing"), 20.0);
				//A mysterious force told me not to sell anything. They said to check the "config", whatever that is.
			}
			
			int angler = NPC.FindFirstNPC(NPCID.Angler);
			if (angler >= 0)
			{
				int questsCompleted = Main.LocalPlayer.anglerQuestsFinished;
				string questsCompletedString = questsCompleted.ToString();
				chat.Add(Language.GetTextValue(path + "AnglerInfo", Main.npc[angler].GivenName, questsCompletedString), 0.25);
				//{Name} is after all sorts of exotic fish. You should see what he wants today. Currently, you have completed # for him.
				if (questsCompleted == 0)
				{
					chat.Add(Language.GetTextValue(path + "AnglerQuest1", Main.npc[angler].GivenName, questsCompletedString) + Language.GetTextValue(path + "AnglerQuest2"), 0.5);
					//So far you have completed # quests for {Name} the Angler. I think it is time for you to start!
				}
				if (questsCompleted > 0)
				{
					chat.Add(Language.GetTextValue(path + "AnglerQuest1", Main.npc[angler].GivenName, questsCompletedString) + Language.GetTextValue(path + "AnglerQuest3"), 0.5);
					//So far you have completed # quests for {Name} the Angler. Keep at it!
				}
				if (questsCompleted >= 10)
				{
					chat.Add(Language.GetTextValue(path + "AnglerQuest1", Main.npc[angler].GivenName, questsCompletedString) + Language.GetTextValue(path + "AnglerQuest4"), 0.5);
					//So far you have completed # quests for {Name} the Angler. Great job! Keep going!
				}
				if (Main.LocalPlayer.Male && questsCompleted >= 30)
				{
					chat.Add(Language.GetTextValue(path + "AnglerQuest5", Main.npc[angler].GivenName, questsCompletedString), 0.5);
					//Nice job, son! You have completed # quests for {Name} the Angler. Why not keep going?
				}
				else if (!Main.LocalPlayer.Male && questsCompleted >= 30)
				{
					chat.Add(Language.GetTextValue(path + "AnglerQuest6", Main.npc[angler].GivenName, questsCompletedString), 0.5);
					//Nice job, lass! You have completed # quests for {Name} the Angler. Why not keep going?
				}
				//happiness quote
				chat.Add(Language.GetTextValue(path + "AnglerHappiness", Main.npc[angler].GivenName), 0.25);
				//No I'm not {Name} the Angler's father, but I'll gladly watch over him!
			}
			int pirate = NPC.FindFirstNPC(NPCID.Pirate);
			if (pirate >= 0)
			{
				//happiness quote
				chat.Add(Language.GetTextValue(path + "PirateHappiness", Main.npc[pirate].GivenName), 0.25);
				//I wouldn't trust {Name} the Pirate. I've had my fair share of run-ins with pirates.
			}
			int princess = NPC.FindFirstNPC(NPCID.Princess);
			if (princess >= 0)
			{
				//happiness quote
				chat.Add(Language.GetTextValue(path + "PrincessHappiness", Main.npc[princess].GivenName), 0.25);
				//{Name} the Princess has offered to get me a fancy new vessel. How kind of her!
			}
			int truffle = NPC.FindFirstNPC(NPCID.Truffle);
			if (truffle >= 0)
			{
				chat.Add(Language.GetTextValue(path + "TruffleNormal", Main.npc[truffle].GivenName), 0.25);
				//I occasionally take Glowing Mushrooms to use as bait. I don't think {Name} knows that I do that, so don't tell 'em!

				//happiness quote
				chat.Add(Language.GetTextValue(path + "TruffleHappiness", Main.npc[truffle].GivenName), 0.25);
				//I'm friends with {Name} the Truffle. Glowing Mushrooms make good bait!
			}
			int nurse = NPC.FindFirstNPC(NPCID.Nurse);
			if (nurse >= 0)
			{
				//happiness quote
				chat.Add(Language.GetTextValue(path + "NurseHappiness", Main.npc[nurse].GivenName), 0.25);
				//I dislike {Name} the Nurse. She always bugs me for check-ups that I don't need.
			}
			int mechanic = NPC.FindFirstNPC(NPCID.Mechanic);
			if (mechanic >= 0)
			{
				//happiness quote
				chat.Add(Language.GetTextValue(path + "MechanicHappiness", Main.npc[mechanic].GivenName), 0.25);
				//{Name} the Mechanic is always here to help me when me ship needs repairs.
			}
			if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && townNPCsCrossModSupport)
			{
				if (thorium.TryFind<ModNPC>("Diverman", out ModNPC divermanModNPC))
				{
					int diverman = NPC.FindFirstNPC(divermanModNPC.Type);
					if (diverman >= 0)
					{
						chat.Add(Language.GetTextValue(path + "ThoriumMod.Diverman", Main.npc[diverman].FullName), 0.25);
						//Now {Name} is the kind of guy I respect!
					}
				}
			}
			if (ModLoader.TryGetMod("CalamityMod", out Mod calamity) && townNPCsCrossModSupport)
			{
				if (calamity.TryFind<ModNPC>("SEAHOE", out ModNPC seaKingModNPC))
				{
					int seaKing = NPC.FindFirstNPC(seaKingModNPC.Type); //Sea King
					if (seaKing >= 0)
					{
						chat.Add(Language.GetTextValue(path + "CalamityMod.SeaKing"), 0.25);
						//Amidias is such an interesting person. I hope he doesn't mind me fishin'.
					}
					if (Main.LocalPlayer.Male)
					{
						chat.Add(Language.GetTextValue(path + "CalamityMod.Warn1"), 0.25);
						//Careful with mermaids, son, not all of them are friendly.
					}
					else
					{
						chat.Add(Language.GetTextValue(path + "CalamityMod.Warn2"), 0.25);
						//Careful with mermaids, lass, not all of them are friendly.
					}
				}
			}
			if (ModLoader.TryGetMod("SGAmod", out Mod _) && townNPCsCrossModSupport) //SGAmod
			{
				if (Main.hardMode)
				{
					chat.Add(Language.GetTextValue(path + "SGAmod.Sharkvern"), 0.25);
					//A Shark-what? That is truly a freak of nature.
				}
			}
			if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && townNPCsCrossModSupport)
			{
				if (fargosMutant.TryFind<ModNPC>("Mutant", out ModNPC mutantModNPC))
				{
					int mutant = NPC.FindFirstNPC(mutantModNPC.Type);
					if (mutant >= 0)
					{
						chat.Add(Language.GetTextValue(path + "FargosMutant.Mutant", Main.npc[mutant].GivenName), 0.25);
						//The wings that {Name} have remind me of something else...
					}
				}
			}
			if (ModLoader.TryGetMod("NoFishingQuests", out Mod _) && townNPCsCrossModSupport)
			{
				if (angler >= 0)
				{
					chat.Add(Language.GetTextValue(path + "NoFishingQuests.AnglerShop", Main.npc[angler].GivenName), 0.25);
					//It seems like {Name} has started to sell his own items. Don't forget about me; I still have plenty of goods available in my shop!
				}
			}
			if (ModLoader.TryGetMod("PboneUtils", out Mod _) && townNPCsCrossModSupport)
			{
				if (Main.dayTime)
				{
					chat.Add(Language.GetTextValue(path + "PbonesUtilities.MysteriousTrader"), 0.25);
					//I heard rumors of a blue hooded man who sells valuables...
				}
			}
			if (ModLoader.TryGetMod("TorchMerchant", out Mod torchSeller) && townNPCsCrossModSupport)
			{
				if (torchSeller.TryFind<ModNPC>("TorchSellerNPC", out ModNPC torchSellerModNPC))
				{
					int torchMan = NPC.FindFirstNPC(torchSellerModNPC.Type);
					if (torchMan >= 0)
					{
						chat.Add(Language.GetTextValue(path + "TorchSeller.TorchMan", Main.npc[torchMan].GivenName), 0.25);
						//{Name} really lightens my spirits when I'm around them.
					}
				}

			}
			if (ModLoader.TryGetMod("BossesAsNPCs", out Mod bossesAsNPCs) && townNPCsCrossModSupport)
			{
				if (bossesAsNPCs.TryFind<ModNPC>("DukeFishron", out ModNPC dukeFishron))
				{
					int dukeFishronType = NPC.FindFirstNPC(dukeFishron.Type);
					if (dukeFishronType >= 0)
					{
						chat.Add(Language.GetTextValue(path + "BossesAsNPCs.DukeFishron"), 0.25);
						//Duke Fishron is quite the fellow. Never thought I'd get to actually get to talk to someone like him!
					}
				}
			}
			if (ModLoader.TryGetMod("RijamsMod", out Mod rijamsMod) && townNPCsCrossModSupport)
			{
				if (rijamsMod.TryFind<ModNPC>("InterstellarTraveler", out ModNPC intTravModNPC))
				{
					int interTravel = NPC.FindFirstNPC(intTravModNPC.Type);
					if (interTravel >= 0)
					{
						chat.Add(Language.GetTextValue(path + "RijamsMod.InterTravel", Main.npc[interTravel].GivenName), 0.25);
						//{Name} is nice and all, but I don't trust her around my stash of fish!
					}
				}
				if (rijamsMod.TryFind<ModNPC>("Harpy", out ModNPC harpyModNPC))
				{
					int harpy = NPC.FindFirstNPC(harpyModNPC.Type);
					if (harpy >= 0)
					{
						chat.Add(Language.GetTextValue(path + "RijamsMod.Harpy", Main.npc[harpy].GivenName), 0.25);
						//{Name} sometimes helps me scout ahead on my fishing journeys. Very helpful!
					}
				}
				if (rijamsMod.TryFind<ModNPC>("HellTrader", out ModNPC hellTraderModNPC))
				{
					int hellTrader = NPC.FindFirstNPC(hellTraderModNPC.Type);
					if (hellTrader >= 0)
					{
						chat.Add(Language.GetTextValue(path + "RijamsMod.HellTrader", Main.npc[hellTrader].GivenName), 0.25);
						//I let {0} try some different kinds of fish. She seemed to enjoy them!
					}
				}
			}
			if (ModLoader.TryGetMod("HelpfulNPCs", out Mod helpfulNPCs) && townNPCsCrossModSupport)
			{
				if (helpfulNPCs.TryFind<ModNPC>("FishermanNPC", out ModNPC fisherman2ModNPC))
				{
					int fisherman2 = NPC.FindFirstNPC(fisherman2ModNPC.Type);
					if (fisherman2 >= 0)
					{
						chat.Add(Language.GetTextValue(path + "HelpfulNPCs.Fisherman2", Main.npc[fisherman2].GivenName), 0.25);
						//{0} may be my competitor, but I can't stay mad at a fellow Fisherman.
					}
				}
			}
			return chat;
		}

		public override void SetChatButtons(ref string button, ref string button2)
		{
			if (NPCHelper.StatusShopCycle() == 1)
				button = Language.GetTextValue("Mods." + Mod.Name + ".UI." + Name + ".Shop1"); //shop1 Bait
			else if (NPCHelper.StatusShopCycle() == 2)
				button = Language.GetTextValue("Mods." + Mod.Name + ".UI." + Name + ".Shop2"); //shop2 Fish
			else if (NPCHelper.StatusShopCycle() == 3)
				button = Language.GetTextValue("Mods." + Mod.Name + ".UI." + Name + ".Shop3"); //shop3 Rods
			else if (NPCHelper.StatusShopCycle() == 4)
				button = Language.GetTextValue("Mods." + Mod.Name + ".UI." + Name + ".Shop4"); //shop4 Extra
			else if (NPCHelper.StatusShopCycle() == 0)
				button = Language.GetTextValue("Mods." + Mod.Name + ".UI." + Name + ".NoShop"); //Shops are disabled!
			else
				button = Language.GetTextValue("Mods." + Mod.Name + ".UI." + Name + ".Shop1"); //shop1 Bait

			button2 = Language.GetTextValue("Mods." + Mod.Name + ".UI." + Name + ".CycleShop"); // Cycle Shop
		}

		public override void OnChatButtonClicked(bool firstButton, ref bool shop)
		{
			if (firstButton)
			{
				shop = true;
				NPCHelper.StatusShopCycle();
			}
			if (!firstButton)
			{
				shop = false;
				NPCHelper.IncrementShopCycle();
			}
		}
		
		public override void SetupShop(Chest shop, ref int nextSlot)
		{
			bool sellModdedItems = ModContent.GetInstance<FishermanNPCConfigServer>().SellModdedItems;
			bool sellBait = ModContent.GetInstance<FishermanNPCConfigServer>().SellBait;
			bool sellFish = ModContent.GetInstance<FishermanNPCConfigServer>().SellFish;
			bool sellFishingRods = ModContent.GetInstance<FishermanNPCConfigServer>().SellFishingRods;
			bool sellExtraItems = ModContent.GetInstance<FishermanNPCConfigServer>().SellExtraItems;
			bool townNPCsCrossModSupport = ModContent.GetInstance<FishermanNPCConfigServer>().TownNPCsCrossModSupport;
			Player player = Main.LocalPlayer;

			//
			// Bait
			//
			if (sellBait && NPCHelper.StatusShopCycle() == 1)
			{
				if (sellModdedItems)
				{
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<PlasticWormLure>());//5% bait power
					shop.item[nextSlot].shopCustomPrice = 50;
					nextSlot++;
				}
				shop.item[nextSlot].SetDefaults(ItemID.Snail);//10%
				shop.item[nextSlot].shopCustomPrice = 2000;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.ApprenticeBait);//15%
				shop.item[nextSlot].shopCustomPrice = 2500;
				nextSlot++;
				if (NPC.downedBoss1)//EoC
				{
					shop.item[nextSlot].SetDefaults(ItemID.HellButterfly);//15%
					shop.item[nextSlot].shopCustomPrice = 3000;
					nextSlot++;
				}
				if (sellModdedItems)
				{
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Mealworm>());//18%
					shop.item[nextSlot].shopCustomPrice = 2800;
					nextSlot++;
				}
				shop.item[nextSlot].SetDefaults(ItemID.Firefly);//20%
				shop.item[nextSlot].shopCustomPrice = 3000;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.Worm);//25%
				shop.item[nextSlot].shopCustomPrice = 3500;
				nextSlot++;
				if (NPC.downedBoss2)//EoW or BoC
				{
					shop.item[nextSlot].SetDefaults(ItemID.Lavafly);//25%
					shop.item[nextSlot].shopCustomPrice = 5000;
					nextSlot++;
				}
				if (NPC.downedBoss1)//EoC
				{
					shop.item[nextSlot].SetDefaults(ItemID.JourneymanBait);//30%
					shop.item[nextSlot].shopCustomPrice = 4000;
					nextSlot++;
					if (sellModdedItems)
					{
						shop.item[nextSlot].SetDefaults(ModContent.ItemType<RedWorm>());//32%
						shop.item[nextSlot].shopCustomPrice = 4200;
						nextSlot++;
					}
				}
				if (NPC.downedBoss2)//EoW or BoC
				{
					shop.item[nextSlot].SetDefaults(ItemID.EnchantedNightcrawler);//35%
					shop.item[nextSlot].shopCustomPrice = 4500;
					nextSlot++;
				}
				if (NPC.downedBoss3)//Skeletron
				{
					shop.item[nextSlot].SetDefaults(ItemID.MagmaSnail);//35%
					shop.item[nextSlot].shopCustomPrice = 7000;
					nextSlot++;
				}
				if (NPC.downedQueenBee)
				{
					shop.item[nextSlot].SetDefaults(ItemID.Buggy);//40%
					shop.item[nextSlot].shopCustomPrice = 5000;
					nextSlot++;
				}
				if (Main.hardMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.MasterBait);//50%
					shop.item[nextSlot].shopCustomPrice = 6000;
					nextSlot++;
				}
				if (Main.hardMode && NPC.CountNPCS(NPCID.Truffle) > 0)
				{
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<GlowingMushroomChunk>());//66%
					shop.item[nextSlot].shopCustomPrice = 8000;
					nextSlot++;
				}
				if (NPC.downedFishron)
				{
					shop.item[nextSlot].SetDefaults(ItemID.TruffleWorm);
					shop.item[nextSlot].shopCustomPrice = 150000;
					nextSlot++;
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamity) && townNPCsCrossModSupport)
				{
					if (Main.hardMode && ((bool)calamity.Call("GetInZone", player, "crags") || (bool)calamity.Call("GetInZone", player, "astral") || (bool)calamity.Call("GetInZone", player, "sulphursea")))
					{
						NPCHelper.SafelySetCrossModItem(calamity, "ArcturusAstroidean", shop, ref nextSlot);
					}
					if ((bool)calamity.Call("GetBossDowned", "oldduke"))
					{
						NPCHelper.SafelySetCrossModItem(calamity, "BloodwormItem", shop, ref nextSlot, 500000);
					}
				}
				shop.item[nextSlot].SetDefaults(ItemID.CanOfWorms);
				shop.item[nextSlot].shopCustomPrice = 25000;
				nextSlot++;
				if (sellModdedItems)
				{
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<BaitBox>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
				}
			}
			//
			// Fish
			//
			if (sellFish && NPCHelper.StatusShopCycle() == 2)
			{
				if (NPCHelper.ZoneAnyUnderground(player) || Main.hardMode) //if Underground, Caverns, Underworld, or Hardmode
				{
					shop.item[nextSlot].SetDefaults(ItemID.ArmoredCavefish);
					shop.item[nextSlot].shopCustomPrice = 7500; //Fish's prices are based off of their vanilla values. 
					nextSlot++;
				}
				if (player.ZoneSnow) //if Snow biome
				{
					shop.item[nextSlot].SetDefaults(ItemID.AtlanticCod);
					shop.item[nextSlot].shopCustomPrice = 3750;
					nextSlot++;
				}
				if (!player.ZoneDesert) //if not Desert
				{
					shop.item[nextSlot].SetDefaults(ItemID.Bass);
					shop.item[nextSlot].shopCustomPrice = 2500;
					nextSlot++;
				}
				if (NPCHelper.ZoneAnyUnderground(player)) //if Underground, Caverns, Underworld
				{
					if (player.ZoneHallow) //if Hallow
					{
						shop.item[nextSlot].SetDefaults(ItemID.ChaosFish);
						shop.item[nextSlot].shopCustomPrice = 150000;
						nextSlot++;
					}
				}
				if ((NPC.downedBoss2 && WorldGen.crimson) || Main.hardMode) //if EoW/BoC defeated and Crimson world, or Hardmode
				{
					shop.item[nextSlot].SetDefaults(ItemID.CrimsonTigerfish);
					shop.item[nextSlot].shopCustomPrice = 3750;
					nextSlot++;
				}
				if (player.ZoneSkyHeight) //if Space layer
				{
					shop.item[nextSlot].SetDefaults(ItemID.Damselfish);
					shop.item[nextSlot].shopCustomPrice = 15000;
					nextSlot++;
				}
				if (player.ZoneJungle || Main.hardMode) //if Jungle
				{
					shop.item[nextSlot].SetDefaults(ItemID.DoubleCod);
					shop.item[nextSlot].shopCustomPrice = 7500;
					nextSlot++;
				}
				if ((NPC.downedBoss2 && !WorldGen.crimson) || Main.hardMode) //if EoW/BoC defeated and Corruption world, or Hardmode
				{
					shop.item[nextSlot].SetDefaults(ItemID.Ebonkoi);
					shop.item[nextSlot].shopCustomPrice = 7500;
					nextSlot++;
				}
				if (player.ZoneUnderworldHeight || Main.hardMode) //if Underworld or Hardmode
				{
					shop.item[nextSlot].SetDefaults(ItemID.FlarefinKoi);
					shop.item[nextSlot].shopCustomPrice = 25000;
					nextSlot++;
				}
				if (player.ZoneDesert) //if Desert
				{
					shop.item[nextSlot].SetDefaults(ItemID.Flounder);
					shop.item[nextSlot].shopCustomPrice = 750;
					nextSlot++;
				}
				if (player.ZoneSnow) //if Snow biome
				{
					shop.item[nextSlot].SetDefaults(ItemID.FrostMinnow);
					shop.item[nextSlot].shopCustomPrice = 7500;
					nextSlot++;
				}
				if ((NPC.downedBoss2 && WorldGen.crimson) || Main.hardMode) //if EoW/BoC defeated and Crimson world, or Hardmode
				{
					shop.item[nextSlot].SetDefaults(ItemID.Hemopiranha);
					shop.item[nextSlot].shopCustomPrice = 7500;
					nextSlot++;
				}
				if (player.ZoneJungle) //if Jungle
				{
					shop.item[nextSlot].SetDefaults(ItemID.Honeyfin);
					shop.item[nextSlot].shopCustomPrice = 7500;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.NeonTetra);
					shop.item[nextSlot].shopCustomPrice = 7500;
					nextSlot++;
				}
				if (player.ZoneUnderworldHeight || Main.hardMode) //if Underworld or Hardmode
				{
					shop.item[nextSlot].SetDefaults(ItemID.Obsidifish);
					shop.item[nextSlot].shopCustomPrice = 7500;
					nextSlot++;
				}
				if (player.ZoneHallow) //if Hallow
				{
					shop.item[nextSlot].SetDefaults(ItemID.PrincessFish);
					shop.item[nextSlot].shopCustomPrice = 12500;
					nextSlot++;
				}
				if (Main.hardMode) //if Hardmode
				{
					shop.item[nextSlot].SetDefaults(ItemID.Prismite);
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
				}
				if (player.ZoneBeach) //if Ocean
				{
					shop.item[nextSlot].SetDefaults(ItemID.RedSnapper);
					shop.item[nextSlot].shopCustomPrice = 3750;
					nextSlot++;
				}
				if (player.ZoneDesert) //if Desert
				{
					shop.item[nextSlot].SetDefaults(ItemID.RockLobster);
					shop.item[nextSlot].shopCustomPrice = 5000;
					nextSlot++;
				}
				if (player.ZoneSkyHeight || player.ZoneOverworldHeight) //if Sky or Surface
				{
					shop.item[nextSlot].SetDefaults(ItemID.Salmon);
					shop.item[nextSlot].shopCustomPrice = 3750;
					nextSlot++;
				}
				if (player.ZoneBeach) //if Ocean
				{
					shop.item[nextSlot].SetDefaults(ItemID.Shrimp);
					shop.item[nextSlot].shopCustomPrice = 7500;
					nextSlot++;
				}
				if ((NPCHelper.ZoneAnyUnderground(player)) && !player.ZoneDesert) //if Underground or Caverns or Underworld, and not Desert
				{
					shop.item[nextSlot].SetDefaults(ItemID.SpecularFish);
					shop.item[nextSlot].shopCustomPrice = 3750;
					nextSlot++;
				}
				if (NPCHelper.ZoneAnyUnderground(player)) //if Underground or Caverns or Underworld
				{
					shop.item[nextSlot].SetDefaults(ItemID.Stinkfish);
					shop.item[nextSlot].shopCustomPrice = 12500;
					nextSlot++;
				}
				if (player.ZoneBeach) //if Ocean
				{
					shop.item[nextSlot].SetDefaults(ItemID.Trout);
					shop.item[nextSlot].shopCustomPrice = 2500;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.Tuna);
					shop.item[nextSlot].shopCustomPrice = 3750;
					nextSlot++;
				}
				if (player.ZoneJungle || Main.hardMode) //if Jungle or Hardmode
				{
					shop.item[nextSlot].SetDefaults(ItemID.VariegatedLardfish);
					shop.item[nextSlot].shopCustomPrice = 7500;
					nextSlot++;
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamity2) && townNPCsCrossModSupport)
				{
					if (Main.hardMode && (bool)calamity2.Call("GetInZone", player, "astral"))
					{
						NPCHelper.SafelySetCrossModItem(calamity2, "AldebaranAlewife", shop, ref nextSlot);
						NPCHelper.SafelySetCrossModItem(calamity2, "ProcyonidPrawn", shop, ref nextSlot);
						NPCHelper.SafelySetCrossModItem(calamity2, "TwinklingPollox", shop, ref nextSlot);
					}
					if ((bool)calamity2.Call("GetInZone", player, "crags"))
					{
						NPCHelper.SafelySetCrossModItem(calamity2, "BrimstoneFish", shop, ref nextSlot);
						if ((bool)calamity2.Call("GetBossDowned", "providence"))
						{
							NPCHelper.SafelySetCrossModItem(calamity2, "Bloodfin", shop, ref nextSlot);
						}
						if (Main.hardMode)
						{
							NPCHelper.SafelySetCrossModItem(calamity2, "ChaoticFish", shop, ref nextSlot);
						}
						NPCHelper.SafelySetCrossModItem(calamity2, "CoastalDemonfish", shop, ref nextSlot);
						NPCHelper.SafelySetCrossModItem(calamity2, "CragBullhead", shop, ref nextSlot);
						NPCHelper.SafelySetCrossModItem(calamity2, "Shadowfish", shop, ref nextSlot);
					}
					if ((bool)calamity2.Call("GetInZone", player, "sunkensea"))
					{
						NPCHelper.SafelySetCrossModItem(calamity2, "CoralskinFoolfish", shop, ref nextSlot);
						NPCHelper.SafelySetCrossModItem(calamity2, "PrismaticGuppy", shop, ref nextSlot);
						NPCHelper.SafelySetCrossModItem(calamity2, "SunkenSailfish", shop, ref nextSlot);
					}
					if (Main.hardMode)
					{
						if (player.ZoneSnow)
						{
							NPCHelper.SafelySetCrossModItem(calamity2, "FishofEleum", shop, ref nextSlot, 1, 10f);
						}
						if (player.ZoneSkyHeight)
						{
							NPCHelper.SafelySetCrossModItem(calamity2, "FishofFlight", shop, ref nextSlot);
							NPCHelper.SafelySetCrossModItem(calamity2, "SunbeamFish", shop, ref nextSlot, 1, 10f);
						}
						if (NPCHelper.ZoneAnyUnderground(player) && player.ZoneHallow)
						{
							NPCHelper.SafelySetCrossModItem(calamity2, "FishofLight", shop, ref nextSlot);
						}
						if (NPCHelper.ZoneAnyUnderground(player) && (player.ZoneCorrupt || player.ZoneCrimson))
						{
							NPCHelper.SafelySetCrossModItem(calamity2, "FishofNight", shop, ref nextSlot);
						}
					}
					if (player.ZoneRockLayerHeight)
					{
						NPCHelper.SafelySetCrossModItem(calamity2, "GlimmeringGemfish", shop, ref nextSlot);
					}
					if (player.ZoneOverworldHeight)
					{
						NPCHelper.SafelySetCrossModItem(calamity2, "StuffedFish", shop, ref nextSlot);
						if (Main.bloodMoon)
						{
							NPCHelper.SafelySetCrossModItem(calamity2, "Xerocodile", shop, ref nextSlot, 1, 2f);
						}
					}
				}
				if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && townNPCsCrossModSupport)
				{
					if (player.ZoneUnderworldHeight || player.ZoneRockLayerHeight || true)
					{
						NPCHelper.SafelySetCrossModItem(thorium, "MagmaGill", shop, ref nextSlot);
						NPCHelper.SafelySetCrossModItem(thorium, "FlamingCrackGut", shop, ref nextSlot);
					}
				}
			}
			//
			// Fishing rods
			//
			if (sellFishingRods && NPCHelper.StatusShopCycle() == 3)
			{
				shop.item[nextSlot].SetDefaults(ItemID.ReinforcedFishingPole);
				shop.item[nextSlot].shopCustomPrice = 50000;
				nextSlot++;
				if ((NPC.downedBoss2 && WorldGen.crimson) || NPC.downedMoonlord)//EoW or BoC and Crimson world. Or defeated Moon Lord
				{
					shop.item[nextSlot].SetDefaults(ItemID.Fleshcatcher);
					shop.item[nextSlot].shopCustomPrice = 150000;
					nextSlot++;
				}
				if ((NPC.downedBoss2 && !WorldGen.crimson) || NPC.downedMoonlord)//EoW or BoC and Corruption world. Or defeated Moon Lord
				{
					shop.item[nextSlot].SetDefaults(ItemID.FisherofSouls);
					shop.item[nextSlot].shopCustomPrice = 150000;
					nextSlot++;
				}
				if (Main.bloodMoon)
				{
					shop.item[nextSlot].SetDefaults(ItemID.BloodFishingRod);//Chum Caster
					shop.item[nextSlot].shopCustomPrice = 250000;
					nextSlot++;
				}
				if (NPC.downedBoss1 && player.ZoneDesert)//EoC and in desert
				{
					shop.item[nextSlot].SetDefaults(ItemID.ScarabFishingRod);
					shop.item[nextSlot].shopCustomPrice = 250000;
					nextSlot++;
				}
				if (NPC.downedQueenBee)
				{
					shop.item[nextSlot].SetDefaults(ItemID.FiberglassFishingPole);
					shop.item[nextSlot].shopCustomPrice = 250000;
					nextSlot++;
				}
				if (NPC.downedBoss3)//Skeletron
				{
					shop.item[nextSlot].SetDefaults(ItemID.MechanicsRod);
					shop.item[nextSlot].shopCustomPrice = 300000;
					nextSlot++;
				}
				if (NPC.downedBoss3 && NPCHelper.CountTownNPCs() > 8)//Skeletron and more than 8 NPCs
				{
					shop.item[nextSlot].SetDefaults(ItemID.SittingDucksFishingRod);
					shop.item[nextSlot].shopCustomPrice = 400000;
					nextSlot++;
				}
				if (Main.hardMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.HotlineFishingHook);
					shop.item[nextSlot].shopCustomPrice = 450000;
					nextSlot++;
				}
				if (player.anglerQuestsFinished >= 30)
				{
					shop.item[nextSlot].SetDefaults(ItemID.GoldenFishingRod);
					shop.item[nextSlot].shopCustomPrice = 500000;
					nextSlot++;
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamity3) && townNPCsCrossModSupport)
				{
					NPCHelper.SafelySetCrossModItem(calamity3, "WulfrumRod", shop, ref nextSlot);
					if ((bool)calamity3.Call("GetBossDowned", "desertscourge"))
					{
						NPCHelper.SafelySetCrossModItem(calamity3, "NavyFishingRod", shop, ref nextSlot);
					}
					if ((bool)calamity3.Call("GetBossDowned", "hivemind") || (bool)calamity3.Call("GetBossDowned", "perforator"))
					{
						NPCHelper.SafelySetCrossModItem(calamity3, "HeronRod", shop, ref nextSlot);
					}
					if ((bool)calamity3.Call("GetInZone", player, "crags"))
					{
						NPCHelper.SafelySetCrossModItem(calamity3, "SlurperPole", shop, ref nextSlot);
					}
					if (NPC.downedPlantBoss)
					{
						NPCHelper.SafelySetCrossModItem(calamity3, "FeralDoubleRod", shop, ref nextSlot);
					}
					if (NPC.downedGolemBoss)
					{
						NPCHelper.SafelySetCrossModItem(calamity3, "RiftReeler", shop, ref nextSlot);
					}
					if ((bool)calamity3.Call("GetBossDowned", "providence"))
					{
						NPCHelper.SafelySetCrossModItem(calamity3, "EarlyBloomRod", shop, ref nextSlot);
					}
					if ((bool)calamity3.Call("GetBossDowned", "devourerofgods"))
					{
						NPCHelper.SafelySetCrossModItem(calamity3, "TheDevourerofCods", shop, ref nextSlot);
					}
				}
				if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && townNPCsCrossModSupport)
				{
					int diverman = NPC.FindFirstNPC(thorium.Find<ModNPC>("Diverman").Type);
					if (diverman >= 0)
					{
						NPCHelper.SafelySetCrossModItem(thorium, "MarineCatcher", shop, ref nextSlot);
					}
					if (player.ZoneBeach)
					{
						NPCHelper.SafelySetCrossModItem(thorium, "CartlidgedCatcher", shop, ref nextSlot);
					}
					if (NPC.downedBoss3)
					{
						NPCHelper.SafelySetCrossModItem(thorium, "GraniteControlRod", shop, ref nextSlot);
						NPCHelper.SafelySetCrossModItem(thorium, "ChampionCatcher", shop, ref nextSlot);
					}
					if (Main.hardMode)
					{
						NPCHelper.SafelySetCrossModItem(thorium, "GeodeGatherer", shop, ref nextSlot);
					}
					// Bruh, Thorium doesn't have downed boss mod calls.
					if (diverman >= 0 && NPC.downedPlantBoss)
					{
						NPCHelper.SafelySetCrossModItem(thorium, "RlyehLostRod", shop, ref nextSlot);
					}
					if (player.anglerQuestsFinished >= 30 && NPC.downedPlantBoss)
					{
						NPCHelper.SafelySetCrossModItem(thorium, "TerrariumFisher", shop, ref nextSlot);
					}
				}
			}
			//
			// Other
			//
			if (sellExtraItems && NPCHelper.StatusShopCycle() == 4)
			{
				if (player.anglerQuestsFinished >= 1)
				{
					shop.item[nextSlot].SetDefaults(ItemID.FishingPotion);
					shop.item[nextSlot].shopCustomPrice = 7500;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.CratePotion);
					shop.item[nextSlot].shopCustomPrice = 6000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.SonarPotion);
					shop.item[nextSlot].shopCustomPrice = 5000;
					nextSlot++;
				}
				if (player.anglerQuestsFinished >= 2)
				{
					shop.item[nextSlot].SetDefaults(ItemID.FishingBobber);
					shop.item[nextSlot].shopCustomPrice = 25000;
					nextSlot++;
				}
				if (player.anglerQuestsFinished >= 5) 
				{
					// 0 Full Moon
					// 1 Waning Gibbous
					// 2 Third Quarter
					// 3 Waning Crescent
					// 4 New Moon
					// 5 Waxing Crescent
					// 6 First Quarter
					// 7 Waxing Gibbous
					if (Main.moonPhase == 0)
					{
						shop.item[nextSlot].SetDefaults(ItemID.ChumBucket);
						shop.item[nextSlot].shopCustomPrice = 2500;
						nextSlot++;
					}
					if (Main.moonPhase == 1)
					{
						shop.item[nextSlot].SetDefaults(ItemID.Sextant);
						shop.item[nextSlot].shopCustomPrice = 50000;
						nextSlot++;
					}
					if (Main.moonPhase == 2)
					{
						shop.item[nextSlot].SetDefaults(ItemID.AnglerEarring);
						shop.item[nextSlot].shopCustomPrice = 50000;
						nextSlot++;
					}
					if (Main.moonPhase == 3)
					{
						shop.item[nextSlot].SetDefaults(ItemID.FishermansGuide);
						shop.item[nextSlot].shopCustomPrice = 50000;
						nextSlot++;
					}
					if (Main.moonPhase == 4)
					{
						shop.item[nextSlot].SetDefaults(ItemID.LavaFishingHook);
						shop.item[nextSlot].shopCustomPrice = 100000;
						nextSlot++;
					}
					if (Main.moonPhase == 5)
					{
						shop.item[nextSlot].SetDefaults(ItemID.HighTestFishingLine);
						shop.item[nextSlot].shopCustomPrice = 50000;
						nextSlot++;
					}
					if (Main.moonPhase == 6)
					{
						shop.item[nextSlot].SetDefaults(ItemID.WeatherRadio);
						shop.item[nextSlot].shopCustomPrice = 50000;
						nextSlot++;
					}
					if (Main.moonPhase == 7)
					{
						shop.item[nextSlot].SetDefaults(ItemID.TackleBox);
						shop.item[nextSlot].shopCustomPrice = 50000;
						nextSlot++;
					}
				}
				if (player.anglerQuestsFinished >= 10)
				{
					if (Main.moonPhase == 4)
					{
						shop.item[nextSlot].SetDefaults(ItemID.ChumBucket);
						shop.item[nextSlot].shopCustomPrice = 2500;
						nextSlot++;
					}
					if (Main.moonPhase == 5)
					{
						shop.item[nextSlot].SetDefaults(ItemID.Sextant);
						shop.item[nextSlot].shopCustomPrice = 50000;
						nextSlot++;
					}
					if (Main.moonPhase == 6)
					{
						shop.item[nextSlot].SetDefaults(ItemID.AnglerEarring);
						shop.item[nextSlot].shopCustomPrice = 50000;
						nextSlot++;
					}
					if (Main.moonPhase == 7)
					{
						shop.item[nextSlot].SetDefaults(ItemID.FishermansGuide);
						shop.item[nextSlot].shopCustomPrice = 50000;
						nextSlot++;
					}
					if (Main.moonPhase == 0)
					{
						shop.item[nextSlot].SetDefaults(ItemID.LavaFishingHook);
						shop.item[nextSlot].shopCustomPrice = 100000;
						nextSlot++;
					}
					if (Main.moonPhase == 1)
					{
						shop.item[nextSlot].SetDefaults(ItemID.HighTestFishingLine);
						shop.item[nextSlot].shopCustomPrice = 50000;
						nextSlot++;
					}
					if (Main.moonPhase == 2)
					{
						shop.item[nextSlot].SetDefaults(ItemID.WeatherRadio);
						shop.item[nextSlot].shopCustomPrice = 50000;
						nextSlot++;
					}
					if (Main.moonPhase == 3)
					{
						shop.item[nextSlot].SetDefaults(ItemID.TackleBox);
						shop.item[nextSlot].shopCustomPrice = 50000;
						nextSlot++;
					}
				}
				if (player.anglerQuestsFinished >= 15)
				{
					if (Main.moonPhase == 2)
					{
						shop.item[nextSlot].SetDefaults(ItemID.ChumBucket);
						shop.item[nextSlot].shopCustomPrice = 2500;
						nextSlot++;
					}
					if (Main.moonPhase == 3)
					{
						shop.item[nextSlot].SetDefaults(ItemID.Sextant);
						shop.item[nextSlot].shopCustomPrice = 50000;
						nextSlot++;
					}
					if (Main.moonPhase == 4)
					{
						shop.item[nextSlot].SetDefaults(ItemID.AnglerEarring);
						shop.item[nextSlot].shopCustomPrice = 50000;
						nextSlot++;
					}
					if (Main.moonPhase == 5)
					{
						shop.item[nextSlot].SetDefaults(ItemID.FishermansGuide);
						shop.item[nextSlot].shopCustomPrice = 50000;
						nextSlot++;
					}
					if (Main.moonPhase == 6)
					{
						shop.item[nextSlot].SetDefaults(ItemID.LavaFishingHook);
						shop.item[nextSlot].shopCustomPrice = 100000;
						nextSlot++;
					}
					if (Main.moonPhase == 7)
					{
						shop.item[nextSlot].SetDefaults(ItemID.HighTestFishingLine);
						shop.item[nextSlot].shopCustomPrice = 50000;
						nextSlot++;
					}
					if (Main.moonPhase == 0)
					{
						shop.item[nextSlot].SetDefaults(ItemID.WeatherRadio);
						shop.item[nextSlot].shopCustomPrice = 50000;
						nextSlot++;
					}
					if (Main.moonPhase == 1)
					{
						shop.item[nextSlot].SetDefaults(ItemID.TackleBox);
						shop.item[nextSlot].shopCustomPrice = 50000;
						nextSlot++;
					}
				}
				if (player.anglerQuestsFinished >= 20)
				{
					if (Main.moonPhase == 6)
					{
						shop.item[nextSlot].SetDefaults(ItemID.ChumBucket);
						shop.item[nextSlot].shopCustomPrice = 2500;
						nextSlot++;
					}
					if (Main.moonPhase == 7)
					{
						shop.item[nextSlot].SetDefaults(ItemID.Sextant);
						shop.item[nextSlot].shopCustomPrice = 50000;
						nextSlot++;
					}
					if (Main.moonPhase == 0)
					{
						shop.item[nextSlot].SetDefaults(ItemID.AnglerEarring);
						shop.item[nextSlot].shopCustomPrice = 50000;
						nextSlot++;
					}
					if (Main.moonPhase == 1)
					{
						shop.item[nextSlot].SetDefaults(ItemID.FishermansGuide);
						shop.item[nextSlot].shopCustomPrice = 50000;
						nextSlot++;
					}
					if (Main.moonPhase == 2)
					{
						shop.item[nextSlot].SetDefaults(ItemID.LavaFishingHook);
						shop.item[nextSlot].shopCustomPrice = 100000;
						nextSlot++;
					}
					if (Main.moonPhase == 3)
					{
						shop.item[nextSlot].SetDefaults(ItemID.HighTestFishingLine);
						shop.item[nextSlot].shopCustomPrice = 50000;
						nextSlot++;
					}
					if (Main.moonPhase == 4)
					{
						shop.item[nextSlot].SetDefaults(ItemID.WeatherRadio);
						shop.item[nextSlot].shopCustomPrice = 50000;
						nextSlot++;
					}
					if (Main.moonPhase == 5)
					{
						shop.item[nextSlot].SetDefaults(ItemID.TackleBox);
						shop.item[nextSlot].shopCustomPrice = 50000;
						nextSlot++;
					}
				}
				if (player.anglerQuestsFinished >= 10)
				{
					shop.item[nextSlot].SetDefaults(ItemID.AnglerHat);
					shop.item[nextSlot].shopCustomPrice = 10000;
					nextSlot++;
				}
				if (player.anglerQuestsFinished >= 15)
				{
					shop.item[nextSlot].SetDefaults(ItemID.AnglerVest);
					shop.item[nextSlot].shopCustomPrice = 10000;
					nextSlot++;
				}
				if (player.anglerQuestsFinished >= 20)
				{
					shop.item[nextSlot].SetDefaults(ItemID.AnglerPants);
					shop.item[nextSlot].shopCustomPrice = 10000;
					nextSlot++;
				}
				if (sellModdedItems)
				{
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<RecyclingMachine>());
					shop.item[nextSlot].shopCustomPrice = 5000;
					nextSlot++;
				}
				if (sellModdedItems && NPC.CountNPCS(NPCID.Clothier) > 0)
				{
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Fisherman_Vanity_Shirt>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Fisherman_Vanity_Pants>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
				}
			}
		}

		public override bool CanGoToStatue(bool toKingStatue)
		{
			return NPC.townNpcVariationIndex switch
			{
				0 => toKingStatue,
				1 => !toKingStatue, // Shimmered
				_ => toKingStatue
			};
		}

		public override void TownNPCAttackStrength(ref int damage, ref float knockback)
		{
			if (!Main.hardMode)
			{
				damage = 25;
			}
			if (Main.hardMode && !NPC.downedMoonlord)
			{
				damage = 30;
			}
			if (NPC.downedMoonlord)
			{
				damage = 35;
			}
			knockback = 6f;
		}

		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
		{
			cooldown = 5;
			randExtraCooldown = 30;
		}

		public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
		{
			projType = ModContent.ProjectileType<HarpoonSpear>();
			attackDelay = 1;
		}
		
		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 16f;
		}
		public override void DrawTownAttackGun(ref float scale, ref int item, ref int closeness)
		{
			item = ItemID.Harpoon;
			scale = 1f;
			closeness = 7;
		}
	}
}