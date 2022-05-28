using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Chat;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using FishermanNPC.NPCs.TownNPCs;
using Terraria.Localization;

namespace FishermanNPC.NPCs
{
	public class NPCHelper
	{
		private static bool shop1;
		private static bool shop2;

		public static void SetShop1(bool tOrF)
		{
			shop1 = tOrF;
		}
		public static void SetShop2(bool tOrF)
		{
			shop2 = tOrF;
		}
		public static bool StatusShop1()
		{
			return shop1;
		}
		public static bool StatusShop2()
		{
			return shop2;
		}
	}
}