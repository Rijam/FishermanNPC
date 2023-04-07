using Microsoft.Xna.Framework;
using rail;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace FishermanNPC.Tiles
{
	public class MealwormCage : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.critterCage = true;
			Main.tileSolidTop[Type] = true;
			Main.tileTable[Type] = true;
			Main.tileLavaDeath[Type] = true;
			Main.tileFrameImportant[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleSmallCage);
			TileObjectData.addTile(Type);

			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(122, 217, 232), name);

			AnimationFrameHeight = 36;
			DustType = DustID.Glass;
		}

		public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
		{
			int x = i - Main.tile[i, j].TileFrameX / 18;
			int y = j - Main.tile[i, j].TileFrameY / 18;
			int smallAnimalCageFrame = x / 3 * (y / 3) % Main.cageFrames;
			
			frameYOffset = Main.wormCageFrame[smallAnimalCageFrame] * AnimationFrameHeight;

			/*for (int k = 0; k < Main.cageFrames; k++)
			{
				frameYOffset = Main.wormCageFrame[k] * AnimationFrameHeight;
			}*/
		}

		// Vanilla Cages draw into the ground and stretch up when stacked. TODO
		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
		{
			base.SetDrawPositions(i, j, ref width, ref offsetY, ref height, ref tileFrameX, ref tileFrameY);
		}
	}

	public class RedWormCage : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.critterCage = true;
			Main.tileSolidTop[Type] = true;
			Main.tileTable[Type] = true;
			Main.tileLavaDeath[Type] = true;
			Main.tileFrameImportant[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleSmallCage);
			TileObjectData.addTile(Type);

			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(122, 217, 232), name);

			AnimationFrameHeight = 36;
			DustType = DustID.Glass;
		}

		public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
		{
			int x = i - Main.tile[i, j].TileFrameX / 18;
			int y = j - Main.tile[i, j].TileFrameY / 18;
			int smallAnimalCageFrame = x / 3 * (y / 3) % Main.cageFrames;

			frameYOffset = Main.wormCageFrame[smallAnimalCageFrame] * AnimationFrameHeight;

			/*for (int k = 0; k < Main.cageFrames; k++)
			{
				frameYOffset = Main.wormCageFrame[k] * AnimationFrameHeight;
			}*/
		}

		// Vanilla Cages draw into the ground and stretch up when stacked. TODO
		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
		{
			base.SetDrawPositions(i, j, ref width, ref offsetY, ref height, ref tileFrameX, ref tileFrameY);
		}
	}

	public class PlasticWormLureCage : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.critterCage = true;
			Main.tileSolidTop[Type] = true;
			Main.tileTable[Type] = true;
			Main.tileLavaDeath[Type] = true;
			Main.tileFrameImportant[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleSmallCage);
			TileObjectData.addTile(Type);

			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(122, 217, 232), name);

			DustType = DustID.Glass;
		}

		// Vanilla Cages draw into the ground and stretch up when stacked. TODO
		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
		{
			base.SetDrawPositions(i, j, ref width, ref offsetY, ref height, ref tileFrameX, ref tileFrameY);
		}
	}
}