using FishermanNPC.Items.Armor.Vanity;
using FishermanNPC.Items.Fishing;
using FishermanNPC.Items.Placeable;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FishermanNPC.NPCs.TownNPCs
{
	public class FishermanShops
	{
		public const string shop1Bait = "Bait";
		public const string shop2Fish = "Fish";
		public const string shop3Rods = "Rods";
		public const string shop4Extra = "Extra";

		// int (key) is the item type
		// int is the price
		// List<Condition> is the conditions
		private static Dictionary<int, Tuple<int, List<Condition>>> customBaitShop = new() { };
		private static Dictionary<int, Tuple<int, List<Condition>>> customFishShop = new() { };
		private static Dictionary<int, Tuple<int, List<Condition>>> customRodsShop = new() { };
		private static Dictionary<int, Tuple<int, List<Condition>>> customExtraShop = new() { };

		public static void UnloadShops()
		{
			customBaitShop = null;
			customFishShop = null;
			customRodsShop = null;
			customExtraShop = null;
		}

		/// <summary>
		/// Adds to the dictionary.
		/// </summary>
		/// <param name="item">The ID for the item</param>
		/// <param name="price">The price for the item</param>
		/// <param name="condition">The availability of the item</param>
		public static void AddToCustomShops(string shop, int item, int price, List<Condition> condition)
		{
			if (shop == shop1Bait)
			{
				customBaitShop.Add(item, new Tuple<int, List<Condition>>(price, condition));
			}
			if (shop == shop2Fish)
			{
				customFishShop.Add(item, new Tuple<int, List<Condition>>(price, condition));
			}
			if (shop == shop3Rods)
			{
				customRodsShop.Add(item, new Tuple<int, List<Condition>>(price, condition));
			}
			if (shop == shop4Extra)
			{
				customExtraShop.Add(item, new Tuple<int, List<Condition>>(price, condition));
			}
		}

		/// <summary>
		/// Attempts to do AddToCustomShops() after some checks.
		/// First it checks that item is within range of all loaded items.
		/// Second, it checks if the shop string matches one of the shops.
		/// Price will be based on the value of the item.
		/// </summary>
		/// <param name="shop">The string for the corresponding NPC</param>
		/// <param name="item">The ID for the item</param>
		/// <param name="condition">The availability of the item</param>
		/// <returns>Returns false if failed.</returns>
		public static bool SetShopItem(string shop, int item, List<Condition> condition)
		{
			if (!CheckIfValid(shop, item))
			{
				return false;
			}

			AdjustConditions(item, ref condition);

			AddToCustomShops(shop, item, CalcItemValue(item), condition);
			return true;
		}

		/// <summary>
		/// Attempts to do AddToCustomShops() after some checks.
		/// First it checks that item is within range of all loaded items.
		/// Second, it checks if the shop string matches one of the shops.
		/// Price will be based on the value of the item.
		/// </summary>
		/// <param name="shop">The string for the corresponding NPC</param>
		/// <param name="item">The ID for the item</param>
		/// <param name="condition">The availability of the item</param>
		/// <param name="customPrice">The custom price to set</param>
		/// <returns>Returns false if failed.</returns>
		public static bool SetShopItem(string shop, int item, List<Condition> condition, int customPrice)
		{
			if (!CheckIfValid(shop, item))
			{
				return false;
			}

			AdjustConditions(item, ref condition);

			AddToCustomShops(shop, item, customPrice, condition);
			return true;
		}

		/// <summary>
		/// Attempts to do AddToCustomShops() after some checks.
		/// First it checks that item is within range of all loaded items.
		/// Second, it checks if the shop string matches one of the shops.
		/// Price will be based on the value of the item.
		/// </summary>
		/// <param name="shop">The string for the corresponding NPC</param>
		/// <param name="item">The ID for the item</param>
		/// <param name="condition">The availability of the item</param>
		/// <param name="priceMulti">The default price will be multiplied by this value</param>
		/// <returns>Returns false if failed.</returns>
		public static bool SetShopItem(string shop, int item, List<Condition> condition, float priceMulti)
		{
			if (!CheckIfValid(shop, item))
			{
				return false;
			}

			AdjustConditions(item, ref condition);

			AddToCustomShops(shop, item, (int)Math.Round(CalcItemValue(item) * priceMulti), condition);
			return true;
		}


		/// <summary>
		/// Checks to see if the string matches one of the shops.
		/// </summary>
		/// <param name="shop">The string for the corresponding shop</param>
		/// <returns>True if a match is found.</returns>
		public static bool CheckIfValid(string shop, int item)
		{
			if (item > ItemLoader.ItemCount)
			{
				ModContent.GetInstance<FishermanNPC>().Logger.WarnFormat("Cross mod SetShopItem(): Item type ID \"{0}\" exceeded the number of loaded items!", item);
				return false;
			}
			if (shop == shop1Bait ||
				shop == shop2Fish ||
				shop == shop3Rods ||
				shop == shop4Extra)
			{
				return true;
			}
			else
			{
				ModContent.GetInstance<FishermanNPC>().Logger.WarnFormat("Cross mod SetShopItem(): Shop string \"{0}\" is not a valid shop type!", shop);
				return false;
			}
		}

		/// <summary>
		/// Also adds cross mod support condition if it wasn't added already.
		/// </summary>
		/// <param name="condition"> Pass the condition list </param>
		public static void AdjustConditions(int item, ref List<Condition> condition)
		{
			if (item >= ItemID.Count && !condition.Contains(ShopConditions.SellModdedItems))
			{
				condition.Add(ShopConditions.SellModdedItems);
			}
			// Add the cross mod support condition.
			if (!condition.Contains(ShopConditions.TownNPCsCrossModSupport))
			{
				condition.Add(ShopConditions.TownNPCsCrossModSupport);
			}
		}

		/// <summary>
		/// Gets the value of the item.
		/// </summary>
		/// <param name="item">The ID for the item</param>
		/// <returns>The value of the item.</returns>
		public static int CalcItemValue(int item)
		{
			Item newItem = new(item);
			return newItem.value;
		}

		// If the internal support for the mod is enabled.

		public static bool CalamityMod = true;
		public static bool ThoriumMod = true;

		/// <summary>
		/// The bait shop.
		/// </summary>
		/// <param name="shop">Pass the NPCShop var</param>
		public static void BaitShop(NPCShop shop)
		{
			shop.Add(new Item(ModContent.ItemType<PlasticWormLure>()) { shopCustomPrice = 50 }, ShopConditions.SellModdedItems);
			shop.Add(new Item(ItemID.Snail) { shopCustomPrice = 2000 });
			shop.Add(new Item(ItemID.ApprenticeBait) { shopCustomPrice = 2500 });
			shop.Add(new Item(ItemID.HellButterfly) { shopCustomPrice = 3000 }, Condition.DownedEowOrBoc);
			shop.Add(new Item(ModContent.ItemType<Mealworm>()) { shopCustomPrice = 2800 }, ShopConditions.SellModdedItems);
			shop.Add(new Item(ItemID.Firefly) { shopCustomPrice = 3000 });
			shop.Add(new Item(ItemID.Worm) { shopCustomPrice = 3500 });
			shop.Add(new Item(ItemID.Lavafly) { shopCustomPrice = 5000 }, Condition.DownedEowOrBoc);
			shop.Add(new Item(ItemID.JourneymanBait) { shopCustomPrice = 4000 }, Condition.DownedEyeOfCthulhu);
			shop.Add(new Item(ModContent.ItemType<RedWorm>()) { shopCustomPrice = 4200 }, Condition.DownedEyeOfCthulhu, ShopConditions.SellModdedItems);
			shop.Add(new Item(ItemID.EnchantedNightcrawler) { shopCustomPrice = 4500 }, Condition.DownedEowOrBoc);
			shop.Add(new Item(ItemID.MagmaSnail) { shopCustomPrice = 7000 }, Condition.DownedSkeletron);
			shop.Add(new Item(ItemID.Buggy) { shopCustomPrice = 5000 }, Condition.DownedQueenBee);
			shop.Add(new Item(ItemID.MasterBait) { shopCustomPrice = 6000 }, Condition.Hardmode);
			shop.Add(new Item(ModContent.ItemType<GlowingMushroomChunk>()) { shopCustomPrice = 8000 },
					Condition.Hardmode,
					Condition.NpcIsPresent(NPCID.Truffle),
					ShopConditions.SellModdedItems);
			shop.Add(new Item(ItemID.TruffleWorm) { shopCustomPrice = 150000 }, Condition.DownedDukeFishron);
			if (ModLoader.TryGetMod("CalamityMod", out Mod calamity1Bait) && CalamityMod)
			{
				NPCHelper.SafelySetCrossModItem(calamity1Bait, "CalamityMod/ArcturusAstroidean", shop,
					new Condition("Mods.FishermanNPC.Conditions.Calamity.CragAstralOrSulphur",
					() => (bool)calamity1Bait.Call("GetInZone", Main.LocalPlayer, "crags")
						|| (bool)calamity1Bait.Call("GetInZone", Main.LocalPlayer, "astral")
						|| (bool)calamity1Bait.Call("GetInZone", Main.LocalPlayer, "sulphursea")));

				NPCHelper.SafelySetCrossModItem(calamity1Bait, "CalamityMod/BloodwormItem", shop,
					new Condition("Mods.FishermanNPC.Conditions.Calamity.DownedOldDuke",
					() => (bool)calamity1Bait.Call("GetBossDowned", "oldduke")));
			}
			shop.Add(new Item(ItemID.CanOfWorms) { shopCustomPrice = 50000 });
			shop.Add(new Item(ModContent.ItemType<BaitBox>()) { shopCustomPrice = 50000 }, ShopConditions.SellModdedItems);

			if (customBaitShop.Count > 0)
			{
				foreach (KeyValuePair<int, Tuple<int, List<Condition>>> set in customBaitShop)
				{
					shop.Add(new Item(set.Key) { shopCustomPrice = set.Value.Item1 }, set.Value.Item2.ToArray());
				}
			}
		}

		/// <summary>
		/// The fish shop.
		/// </summary>
		/// <param name="shop">Pass the NPCShop var</param>
		public static void FishShop(NPCShop shop)
		{
			shop.Add(ItemID.ArmoredCavefish, ShopConditions.AnyUndergroundOrHardmode);
			shop.Add(ItemID.AtlanticCod, Condition.InSnow);
			shop.Add(ItemID.Bass, new Condition("Mods.FishermanNPC.Conditions.NotInDesert", () => !Main.LocalPlayer.ZoneDesert));
			shop.Add(ItemID.ChaosFish, ShopConditions.AnyUnderground, Condition.InHallow);
			shop.Add(ItemID.CrimsonTigerfish, ShopConditions.DownedBocOrEoWCrimsonOrHardmode);
			shop.Add(ItemID.Damselfish, Condition.InSpace);
			shop.Add(ItemID.DoubleCod, ShopConditions.InJungleOrHardmode);
			shop.Add(ItemID.Ebonkoi, ShopConditions.DownedBocOrEoWCrimsonOrHardmode);
			shop.Add(ItemID.FlarefinKoi, ShopConditions.InUnderworldOrHardmode);
			shop.Add(ItemID.Flounder, Condition.InDesert);
			shop.Add(ItemID.FrostMinnow, Condition.InSnow);
			shop.Add(ItemID.Hemopiranha, ShopConditions.DownedBocOrEoWCrimsonOrHardmode);
			shop.Add(ItemID.Honeyfin, Condition.InJungle);
			shop.Add(ItemID.NeonTetra, Condition.InJungle);
			shop.Add(ItemID.Obsidifish, ShopConditions.InUnderworldOrHardmode);
			shop.Add(ItemID.PrincessFish, Condition.InHallow);
			shop.Add(ItemID.Prismite, Condition.Hardmode);
			shop.Add(ItemID.RedSnapper, Condition.InBeach);
			shop.Add(ItemID.RockLobster, Condition.InDesert);
			shop.Add(ItemID.Salmon, Condition.InSpace, Condition.InOverworldHeight);
			shop.Add(ItemID.Shrimp, Condition.InBeach);
			shop.Add(ItemID.SpecularFish, ShopConditions.AnyUndergroundNotDesert);
			shop.Add(ItemID.Stinkfish, ShopConditions.AnyUnderground);
			shop.Add(ItemID.Trout, Condition.InBeach);
			shop.Add(ItemID.Tuna, Condition.InBeach);
			shop.Add(ItemID.VariegatedLardfish, ShopConditions.InJungleOrHardmode);
			if (ModLoader.TryGetMod("CalamityMod", out Mod calamity2Fish) && CalamityMod)
			{
				Condition astral = new("Mods.FishermanNPC.Conditions.Calamity.AstralAndHardmode",
					() => (bool)calamity2Fish.Call("GetInZone", Main.LocalPlayer, "astral"));

				NPCHelper.SafelySetCrossModItem(calamity2Fish, "CalamityMod/AldebaranAlewife", shop, astral, Condition.Hardmode);
				NPCHelper.SafelySetCrossModItem(calamity2Fish, "CalamityMod/ProcyonidPrawn", shop, astral, Condition.Hardmode);
				NPCHelper.SafelySetCrossModItem(calamity2Fish, "CalamityMod/TwinklingPollox", shop, astral, Condition.Hardmode);

				Condition crags = new("Mods.FishermanNPC.Conditions.Calamity.InCrag",
					() => (bool)calamity2Fish.Call("GetInZone", Main.LocalPlayer, "crags"));

				NPCHelper.SafelySetCrossModItem(calamity2Fish, "CalamityMod/BrimstoneFish", shop, crags);
				NPCHelper.SafelySetCrossModItem(calamity2Fish, "CalamityMod/Bloodfin", shop, crags,
					new Condition("Mods.FishermanNPC.Conditions.Calamity.DownedProvidence", () => (bool)calamity2Fish.Call("GetBossDowned", "providence")));
				NPCHelper.SafelySetCrossModItem(calamity2Fish, "CalamityMod/ChaoticFish", shop, crags, Condition.Hardmode);
				NPCHelper.SafelySetCrossModItem(calamity2Fish, "CalamityMod/CoastalDemonfish", shop, crags);
				NPCHelper.SafelySetCrossModItem(calamity2Fish, "CalamityMod/CragBullhead", shop, crags);
				NPCHelper.SafelySetCrossModItem(calamity2Fish, "CalamityMod/Shadowfish", shop, crags);

				Condition sunkensea = new("Mods.FishermanNPC.Conditions.Calamity.InSunkenSea",
					() => (bool)calamity2Fish.Call("GetInZone", Main.LocalPlayer, "sunkensea"));

				NPCHelper.SafelySetCrossModItem(calamity2Fish, "CalamityMod/CoralskinFoolfish", shop, sunkensea);
				NPCHelper.SafelySetCrossModItem(calamity2Fish, "CalamityMod/PrismaticGuppy", shop, sunkensea);
				NPCHelper.SafelySetCrossModItem(calamity2Fish, "CalamityMod/SunkenSailfish", shop, sunkensea);

				NPCHelper.SafelySetCrossModItem(calamity2Fish, "CalamityMod/FishofEleum", shop, 1f, 10f,
					Condition.InSnow, Condition.Hardmode);

				NPCHelper.SafelySetCrossModItem(calamity2Fish, "CalamityMod/FishofFlight", shop, 1f, 10f,
					Condition.InSpace, Condition.Hardmode);
				NPCHelper.SafelySetCrossModItem(calamity2Fish, "CalamityMod/SunbeamFish", shop,
					Condition.InSpace, Condition.Hardmode);

				NPCHelper.SafelySetCrossModItem(calamity2Fish, "CalamityMod/FishofLight", shop,
					Condition.InHallow, ShopConditions.AnyUnderground, Condition.Hardmode);
				NPCHelper.SafelySetCrossModItem(calamity2Fish, "CalamityMod/FishofNight", shop,
					Condition.InEvilBiome, ShopConditions.AnyUnderground, Condition.Hardmode);

				NPCHelper.SafelySetCrossModItem(calamity2Fish, "CalamityMod/GlimmeringGemfish", shop, Condition.InRockLayerHeight);
				NPCHelper.SafelySetCrossModItem(calamity2Fish, "CalamityMod/StuffedFish", shop, Condition.InOverworldHeight);
				NPCHelper.SafelySetCrossModItem(calamity2Fish, "CalamityMod/Xerocodile", shop, 1f, 2f, Condition.InOverworldHeight, Condition.BloodMoon);
			}

			if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium2Fish) && ThoriumMod)
			{
				NPCHelper.SafelySetCrossModItem(thorium2Fish, "ThoriumMod/MagmaGill", shop, ShopConditions.InCavernsOrUnderworld);
				NPCHelper.SafelySetCrossModItem(thorium2Fish, "ThoriumMod/FlamingCrackGut", shop, ShopConditions.InCavernsOrUnderworld);
			}

			if (customFishShop.Count > 0)
			{
				foreach (KeyValuePair<int, Tuple<int, List<Condition>>> set in customFishShop)
				{
					shop.Add(new Item(set.Key) { shopCustomPrice = set.Value.Item1 }, set.Value.Item2.ToArray());
				}
			}
		}

		/// <summary>
		/// The fishing rods shop.
		/// </summary>
		/// <param name="shop">Pass the NPCShop var</param>
		public static void RodsShop(NPCShop shop)
		{
			shop.Add(new Item(ItemID.ReinforcedFishingPole) { shopCustomPrice = 50000 });
			shop.Add(new Item(ItemID.Fleshcatcher) { shopCustomPrice = 150000 },
					new Condition("Mods.FishermanNPC.Conditions.BocOrMoonLord",
					() => Condition.DownedBrainOfCthulhu.IsMet() || Condition.DownedMoonLord.IsMet()));
			shop.Add(new Item(ItemID.FisherofSouls) { shopCustomPrice = 150000 },
					new Condition("Mods.FishermanNPC.Conditions.EowOrMoonLord",
					() => Condition.DownedEaterOfWorlds.IsMet() || Condition.DownedMoonLord.IsMet()));
			shop.Add(new Item(ItemID.BloodFishingRod) { shopCustomPrice = 250000 }, Condition.BloodMoon);
			shop.Add(new Item(ItemID.ScarabFishingRod) { shopCustomPrice = 250000 }, Condition.DownedEyeOfCthulhu, Condition.InDesert);
			shop.Add(new Item(ItemID.FiberglassFishingPole) { shopCustomPrice = 250000 }, Condition.DownedQueenBee);
			shop.Add(new Item(ItemID.MechanicsRod) { shopCustomPrice = 300000 }, Condition.DownedSkeletron);
			shop.Add(new Item(ItemID.SittingDucksFishingRod) { shopCustomPrice = 400000 },
					Condition.DownedSkeletron,
					new Condition(ShopConditions.CountTownNPCsS(9), ShopConditions.CountTownNPCsFb(9)));
			shop.Add(new Item(ItemID.HotlineFishingHook) { shopCustomPrice = 450000 }, Condition.Hardmode);
			shop.Add(new Item(ItemID.GoldenFishingRod) { shopCustomPrice = 500000 }, Condition.AnglerQuestsFinishedOver(30));

			if (ModLoader.TryGetMod("CalamityMod", out Mod calamity3Rods) && CalamityMod)
			{
				NPCHelper.SafelySetCrossModItem(calamity3Rods, "CalamityMod/WulfrumRod", shop);

				NPCHelper.SafelySetCrossModItem(calamity3Rods, "CalamityMod/NavyFishingRod", shop,
					new Condition("Mods.FishermanNPC.Conditions.Calamity.DownedDesertScourge", () => (bool)calamity3Rods.Call("GetBossDowned", "desertscourge")));
				NPCHelper.SafelySetCrossModItem(calamity3Rods, "CalamityMod/HeronRod", shop,
					new Condition("Mods.FishermanNPC.Conditions.Calamity.DownedHiveMindOrPerforators", () => (bool)calamity3Rods.Call("GetBossDowned", "hivemind") || (bool)calamity3Rods.Call("GetBossDowned", "perforator")));

				NPCHelper.SafelySetCrossModItem(calamity3Rods, "CalamityMod/SlurperPole", shop,
					new Condition("Mods.FishermanNPC.Conditions.Calamity.InCrag", () => (bool)calamity3Rods.Call("GetInZone", Main.LocalPlayer, "crags")));

				NPCHelper.SafelySetCrossModItem(calamity3Rods, "CalamityMod/FeralDoubleRod", shop, Condition.DownedPlantera);
				NPCHelper.SafelySetCrossModItem(calamity3Rods, "CalamityMod/RiftReeler", shop, Condition.DownedGolem);

				NPCHelper.SafelySetCrossModItem(calamity3Rods, "CalamityMod/EarlyBloomRod", shop,
					new Condition("Mods.FishermanNPC.Conditions.Calamity.DownedProvidence", () => (bool)calamity3Rods.Call("GetBossDowned", "providence")));
				NPCHelper.SafelySetCrossModItem(calamity3Rods, "CalamityMod/TheDevourerofCods", shop,
					new Condition("Mods.FishermanNPC.Conditions.Calamity.DownedDoG", () => (bool)calamity3Rods.Call("GetBossDowned", "devourerofgods")));
			}

			if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium3Rods) && ThoriumMod)
			{
				if (thorium3Rods.TryFind<ModNPC>("Diverman", out ModNPC diverman))
				{
					NPCHelper.SafelySetCrossModItem(thorium3Rods, "ThoriumMod/MagmaGill", shop,
						Condition.NpcIsPresent(NPC.FindFirstNPC(diverman.Type)));
				}
				NPCHelper.SafelySetCrossModItem(thorium3Rods, "ThoriumMod/CartlidgedCatcher", shop, Condition.InBeach);
				NPCHelper.SafelySetCrossModItem(thorium3Rods, "ThoriumMod/GraniteControlRod", shop, Condition.DownedSkeletron);
				NPCHelper.SafelySetCrossModItem(thorium3Rods, "ThoriumMod/ChampionCatcher", shop, Condition.DownedSkeletron);
				NPCHelper.SafelySetCrossModItem(thorium3Rods, "ThoriumMod/GeodeGatherer", shop, Condition.Hardmode);
				NPCHelper.SafelySetCrossModItem(thorium3Rods, "ThoriumMod/GeodeGatherer", shop,
					new Condition("Mods.FishermanNPC.Conditions.ThoriumMod.DownedForgottenOne", () => (bool)thorium3Rods.Call("GetDownedBoss", "ForgottenOne")));
				NPCHelper.SafelySetCrossModItem(thorium3Rods, "ThoriumMod/TerrariumFisher", shop,
					Condition.AnglerQuestsFinishedOver(30), Condition.DownedCultist);
			}

			if (customRodsShop.Count > 0)
			{
				foreach (KeyValuePair<int, Tuple<int, List<Condition>>> set in customRodsShop)
				{
					shop.Add(new Item(set.Key) { shopCustomPrice = set.Value.Item1 }, set.Value.Item2.ToArray());
				}
			}
		}

		/// <summary>
		/// The extras shop.
		/// </summary>
		/// <param name="shop">Pass the NPCShop var</param>
		public static void ExtraShop(NPCShop shop)
		{
			shop.Add(new Item(ModContent.ItemType<RecyclingMachine>()) { shopCustomPrice = 5000 }, ShopConditions.SellModdedItems);
			shop.Add(new Item(ItemID.FishingPotion) { shopCustomPrice = 15000 }, Condition.AnglerQuestsFinishedOver(1));
			shop.Add(new Item(ItemID.CratePotion) { shopCustomPrice = 12000 }, Condition.AnglerQuestsFinishedOver(1));
			shop.Add(new Item(ItemID.SonarPotion) { shopCustomPrice = 10000 }, Condition.AnglerQuestsFinishedOver(1));
			shop.Add(new Item(ItemID.FishingBobber) { shopCustomPrice = 50000 }, Condition.AnglerQuestsFinishedOver(3));

			Item chumBucket = new(ItemID.ChumBucket) { shopCustomPrice = 2500 };
			Item sextant = new(ItemID.Sextant) { shopCustomPrice = 100000 };
			Item anglerEarring = new(ItemID.AnglerEarring) { shopCustomPrice = 100000 };
			Item fishermansGuide = new(ItemID.FishermansGuide) { shopCustomPrice = 100000 };
			Item lavaFishingHook = new(ItemID.LavaFishingHook) { shopCustomPrice = 200000 };
			Item highTestFishingLine = new(ItemID.HighTestFishingLine) { shopCustomPrice = 100000 };
			Item weatherRadio = new(ItemID.WeatherRadio) { shopCustomPrice = 100000 };
			Item tackleBox = new(ItemID.TackleBox) { shopCustomPrice = 100000 };

			shop.Add(chumBucket, Condition.AnglerQuestsFinishedOver(5), Condition.MoonPhaseFull);
			shop.Add(sextant, Condition.AnglerQuestsFinishedOver(5), Condition.MoonPhaseWaningGibbous);
			shop.Add(anglerEarring, Condition.AnglerQuestsFinishedOver(5), Condition.MoonPhaseThirdQuarter);
			shop.Add(fishermansGuide, Condition.AnglerQuestsFinishedOver(5), Condition.MoonPhaseWaningCrescent);
			shop.Add(lavaFishingHook, Condition.AnglerQuestsFinishedOver(5), Condition.MoonPhaseNew, Condition.DownedEowOrBoc);
			shop.Add(highTestFishingLine, Condition.AnglerQuestsFinishedOver(5), Condition.MoonPhaseWaxingCrescent);
			shop.Add(weatherRadio, Condition.AnglerQuestsFinishedOver(5), Condition.MoonPhaseFirstQuarter);
			shop.Add(tackleBox, Condition.AnglerQuestsFinishedOver(5), Condition.MoonPhaseWaxingGibbous);

			shop.Add(lavaFishingHook, Condition.AnglerQuestsFinishedOver(10), Condition.MoonPhaseFull, Condition.DownedEowOrBoc);
			shop.Add(highTestFishingLine, Condition.AnglerQuestsFinishedOver(10), Condition.MoonPhaseWaningGibbous);
			shop.Add(weatherRadio, Condition.AnglerQuestsFinishedOver(10), Condition.MoonPhaseThirdQuarter);
			shop.Add(tackleBox, Condition.AnglerQuestsFinishedOver(10), Condition.MoonPhaseWaningCrescent);
			shop.Add(chumBucket, Condition.AnglerQuestsFinishedOver(10), Condition.MoonPhaseNew);
			shop.Add(highTestFishingLine, Condition.AnglerQuestsFinishedOver(10), Condition.MoonPhaseWaxingCrescent);
			shop.Add(anglerEarring, Condition.AnglerQuestsFinishedOver(10), Condition.MoonPhaseFirstQuarter);
			shop.Add(fishermansGuide, Condition.AnglerQuestsFinishedOver(10), Condition.MoonPhaseWaxingGibbous);

			shop.Add(weatherRadio, Condition.AnglerQuestsFinishedOver(15), Condition.MoonPhaseFull);
			shop.Add(tackleBox, Condition.AnglerQuestsFinishedOver(15), Condition.MoonPhaseWaningGibbous);
			shop.Add(chumBucket, Condition.AnglerQuestsFinishedOver(15), Condition.MoonPhaseThirdQuarter);
			shop.Add(sextant, Condition.AnglerQuestsFinishedOver(15), Condition.MoonPhaseWaningCrescent);
			shop.Add(anglerEarring, Condition.AnglerQuestsFinishedOver(15), Condition.MoonPhaseNew);
			shop.Add(fishermansGuide, Condition.AnglerQuestsFinishedOver(15), Condition.MoonPhaseWaxingCrescent);
			shop.Add(lavaFishingHook, Condition.AnglerQuestsFinishedOver(15), Condition.MoonPhaseFirstQuarter, Condition.DownedEowOrBoc);
			shop.Add(highTestFishingLine, Condition.AnglerQuestsFinishedOver(15), Condition.MoonPhaseWaxingGibbous);

			shop.Add(anglerEarring, Condition.AnglerQuestsFinishedOver(20), Condition.MoonPhaseFull);
			shop.Add(fishermansGuide, Condition.AnglerQuestsFinishedOver(20), Condition.MoonPhaseWaningGibbous);
			shop.Add(lavaFishingHook, Condition.AnglerQuestsFinishedOver(20), Condition.MoonPhaseThirdQuarter, Condition.DownedEowOrBoc);
			shop.Add(highTestFishingLine, Condition.AnglerQuestsFinishedOver(20), Condition.MoonPhaseWaningCrescent);
			shop.Add(weatherRadio, Condition.AnglerQuestsFinishedOver(20), Condition.MoonPhaseNew);
			shop.Add(tackleBox, Condition.AnglerQuestsFinishedOver(20), Condition.MoonPhaseWaxingCrescent);
			shop.Add(chumBucket, Condition.AnglerQuestsFinishedOver(20), Condition.MoonPhaseFirstQuarter);
			shop.Add(sextant, Condition.AnglerQuestsFinishedOver(20), Condition.MoonPhaseWaxingGibbous);

			shop.Add(ItemID.AnglerHat, Condition.AnglerQuestsFinishedOver(10));
			shop.Add(ItemID.AnglerVest, Condition.AnglerQuestsFinishedOver(15));
			shop.Add(ItemID.AnglerPants, Condition.AnglerQuestsFinishedOver(20));

			shop.Add(ModContent.ItemType<Fisherman_Vanity_Shirt>(), Condition.NpcIsPresent(NPCID.Clothier), ShopConditions.SellModdedItems);
			shop.Add(ModContent.ItemType<Fisherman_Vanity_Pants>(), Condition.NpcIsPresent(NPCID.Clothier), ShopConditions.SellModdedItems);

			if (customExtraShop.Count > 0)
			{
				foreach (KeyValuePair<int, Tuple<int, List<Condition>>> set in customExtraShop)
				{
					shop.Add(new Item(set.Key) { shopCustomPrice = set.Value.Item1 }, set.Value.Item2.ToArray());
				}
			}
		}
	}
}