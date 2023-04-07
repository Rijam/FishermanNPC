using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace FishermanNPC.NPCs
{
	public class MealwormCritter : ModNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = 2;
			NPCID.Sets.TakesDamageFromHostilesWithoutBeingFriendly[Type] = true;
			NPCID.Sets.CountsAsCritter[Type] = true;
			NPCID.Sets.ShimmerTransformToNPC[Type] = NPCID.Shimmerfly;

			// Influences how the NPC looks in the Bestiary
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new(0)
			{
				Position = new Vector2(0f, 2f),
				Velocity = 1f
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
		}

		public override void SetDefaults()
		{
			NPC.width = 10;
			NPC.height = 4;
			NPC.aiStyle = NPCAIStyleID.CritterWorm;
			NPC.damage = 0;
			NPC.defense = 0;
			NPC.lifeMax = 5;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.npcSlots = 0.1f;
			Main.npcCatchable[Type] = true;
			NPC.catchItem = ModContent.ItemType<Items.Fishing.Mealworm>();
			NPC.friendly = false;
			AIType = NPCID.Worm;
			AnimationType = NPCID.Worm;
			DrawOffsetY = -2;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement(NPCHelper.BestiaryPath(Name))
			});
		}

		public override void PostAI()
		{
			NPC.spriteDirection = NPC.direction;
		}
	}

	public class RedWormCritter : ModNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = 2;
			NPCID.Sets.TakesDamageFromHostilesWithoutBeingFriendly[Type] = true;
			NPCID.Sets.CountsAsCritter[Type] = true;
			NPCID.Sets.ShimmerTransformToNPC[Type] = NPCID.Shimmerfly;

			// Influences how the NPC looks in the Bestiary
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new(0)
			{
				Position = new Vector2(0f, 2f),
				Velocity = 1f
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
		}

		public override void SetDefaults()
		{
			NPC.width = 14;
			NPC.height = 6;
			NPC.aiStyle = NPCAIStyleID.CritterWorm;
			NPC.damage = 0;
			NPC.defense = 0;
			NPC.lifeMax = 5;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.npcSlots = 0.1f;
			Main.npcCatchable[Type] = true;
			NPC.catchItem = ModContent.ItemType<Items.Fishing.RedWorm>();
			NPC.friendly = false;
			AIType = NPCID.Worm;
			AnimationType = NPCID.Worm;
			DrawOffsetY = -2;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement(NPCHelper.BestiaryPath(Name))
			});
		}

		public override void PostAI()
		{
			NPC.spriteDirection = NPC.direction;
		}
	}
}