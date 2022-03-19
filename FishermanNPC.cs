using FishermanNPC.NPCs.TownNPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FishermanNPC
{
	public class FishermanNPC : Mod
	{
		internal static FishermanNPCConfigServer ConfigServer;
		internal static FishermanNPC Instance;
		public override void Unload()
		{
			ConfigServer = null;
			Instance = null;
		}
        public override void PostSetupContent()
        {
            if (ModLoader.TryGetMod("Census", out Mod censusMod))
            {
                censusMod.Call("TownNPCCondition", ModContent.NPCType<Fisherman>(), "Rescue the Angler and have at least 5 Town NPCs");
            }
        }
    }
}