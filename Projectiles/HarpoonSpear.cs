using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FishermanNPC.Projectiles
{
	public class HarpoonSpear : ModProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.Harpoon;

		public override void SetDefaults()
		{
			Projectile.arrow = false;
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			AIType = ProjectileID.WoodenArrowFriendly;
			if (!Main.hardMode)
			{
				Projectile.penetrate = 1;
			}
			if (Main.hardMode)
			{
				Projectile.penetrate = 2;
			}
		}
	}
}