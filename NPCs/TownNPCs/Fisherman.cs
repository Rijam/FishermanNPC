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
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent.UI;

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
			NPCID.Sets.FaceEmote[Type] = ModContent.EmoteBubbleType<FishermanEmote>();

			// Influences how the NPC looks in the Bestiary
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new()
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

			// Specify the debuffs it is immune to
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
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

		public override void HitEffect(NPC.HitInfo hitInfo)
		{
			// Create gore when the NPC is killed.
			if (Main.netMode != NetmodeID.Server && NPC.life <= 0)
			{
				// Retrieve the gore types.
				string altVariant = NPC.altTexture == 1 ? "_Alt" : "";
				string shimmerVariant = NPC.IsShimmerVariant ? "_Shimmered" : "";
				int hatGore = NPC.GetPartyHatGore();
				int headGore = Mod.Find<ModGore>($"{Name}_Head{altVariant}{shimmerVariant}").Type;
				int armGore = Mod.Find<ModGore>($"{Name}_Arm{shimmerVariant}").Type;
				int legGore = Mod.Find<ModGore>($"{Name}_Leg{shimmerVariant}").Type;

				// Spawn the gores. The positions of the arms and legs are lowered for a more natural look.
				if (hatGore > 0)
				{
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, hatGore);
				}
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, headGore, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 20), NPC.velocity, armGore);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 20), NPC.velocity, armGore);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 34), NPC.velocity, legGore);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 34), NPC.velocity, legGore);
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
			button = NPCHelper.StatusShopCycle() switch
			{
				0 => Language.GetTextValue($"Mods.{Mod.Name}.UI.{Name}.NoShop"), //Shops are disabled!
				1 => Language.GetTextValue($"Mods.{Mod.Name}.UI.{Name}.Shop1"), //shop1 Bait
				2 => Language.GetTextValue($"Mods.{Mod.Name}.UI.{Name}.Shop2"), //shop2 Fish
				3 => Language.GetTextValue($"Mods.{Mod.Name}.UI.{Name}.Shop3"), //shop3 Rods
				4 => Language.GetTextValue($"Mods.{Mod.Name}.UI.{Name}.Shop4"), //shop4 Extra
				_ => Language.GetTextValue($"Mods.{Mod.Name}.UI.{Name}.Shop1"), //shop1 Bait
			};
			button2 = Language.GetTextValue("Mods." + Mod.Name + ".UI." + Name + ".CycleShop"); // Cycle Shop
		}

		public override void OnChatButtonClicked(bool firstButton, ref string shop)
		{
			if (firstButton)
			{
				shop = NPCHelper.StatusShopCycle() switch
				{
					1 => FishermanShops.shop1Bait,
					2 => FishermanShops.shop2Fish,
					3 => FishermanShops.shop3Rods,
					4 => FishermanShops.shop4Extra,
					_ => ""
				};
			}
			if (!firstButton)
			{
				// shop = false;
				NPCHelper.IncrementShopCycle();
			}
		}

		public override void AddShops()
		{
			//
			// Bait
			//
			var npcShop1Bait = new NPCShop(Type, FishermanShops.shop1Bait);
			FishermanShops.BaitShop(npcShop1Bait);
			npcShop1Bait.Register();

			//
			// Fish
			//
			var npcShop2Fish = new NPCShop(Type, FishermanShops.shop2Fish);
			FishermanShops.FishShop(npcShop2Fish);
			npcShop2Fish.Register();

			//
			// Fishing Rods
			// 
			var npcShop3Rods = new NPCShop(Type, FishermanShops.shop3Rods);
			FishermanShops.RodsShop(npcShop3Rods);
			npcShop3Rods.Register();

			//
			// Extra Items
			// 
			var npcShop4Extra = new NPCShop(Type, FishermanShops.shop4Extra);
			FishermanShops.ExtraShop(npcShop4Extra);
			npcShop4Extra.Register();
		}

		public override int? PickEmote(Player closestPlayer, List<int> emoteList, WorldUIAnchor otherAnchor)
		{
			emoteList.Add(EmoteID.ItemFishingRod);
			emoteList.Add(EmoteID.ItemCookedFish);
			return base.PickEmote(closestPlayer, emoteList, otherAnchor);
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
		public override void DrawTownAttackGun(ref Texture2D item, ref Rectangle itemFrame, ref float scale, ref int horizontalHoldoutOffset)
		{
			Main.GetItemDrawFrame(ItemID.Harpoon, out Texture2D itemTexture, out Rectangle itemRectangle);
			item = itemTexture;
			itemFrame = itemRectangle;
			scale = 1f;
			horizontalHoldoutOffset = -23;
		}
	}
}