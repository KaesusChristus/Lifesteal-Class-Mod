using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using System;
using Terraria.ModLoader;
using LifeStealClass.Content.Core;
using LifeStealClass.Common.ModPlayers;

namespace LifeStealClass.Content.Projectiles.Weapon.Sickle
{
    public class NightsScytheProjectile : LifestealSickleProjectile
    {
        public override string Texture => "LifeStealClass/Content/Items/Weapons/Sickle/NightsScythe";

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.timeLeft = 60;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item60, Projectile.position);

            for (int i = 0; i < 20; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Shadowflame, 0f, 0f, 100, new Color(140, 27, 130), 1.8f);
                dust.noGravity = true;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, ProjectileID.NightsEdge, Projectile.damage / 2, Projectile.knockBack, Projectile.whoAmI, 2f, 25f, 1f);
            }
        }

        // Heal the player, if the target dies
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.ShadowFlame, 120);
            if (target.life <= 0 && target.lifeMax > 5)
            {
                Player localPlayer = Main.LocalPlayer;

                localPlayer.GetModPlayer<LifestealEffectsPlayer>().SetHealAmount(8);
            }
        }

        public override Color GetGlowColor()
        {
            return new Color(150, 20, 140);
        }
    }
}
