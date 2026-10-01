using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using LifeStealClass.Content.Core;
using Terraria.ModLoader;
using System;

namespace LifeStealClass.Content.Projectiles.Weapon.Sickle
{
    public class StarScytheProjectile : LifestealSickleProjectile
    {
        private bool meteorSpawned;

        public override string Texture => "LifeStealClass/Content/Items/Weapons/Sickle/StarScythe";

        public override void SetDefaults()
        {
            base.SetDefaults();

            Projectile.width = 62;
            Projectile.height = 48;
            Projectile.scale = 1.2f;

            Projectile.timeLeft = 50;
        }

        public override SickleStats GetStats()
        {
            return new SickleStats
            {
                PrepTime = 12f,
                ExecTime = 8f,
                HideTime = 12f,

                scale = 1.4f,
                hitboxWidth = 15f,

                rotationOffsetRight = MathHelper.ToRadians(45f),
                rotationOffsetLeft = MathHelper.ToRadians(135f)
            };
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPC(target, ref modifiers);

            if (CurrentAttack == SickleAttackType.HeavySlash)
            {
                SoundEngine.PlaySound(SoundID.Item89);
                target.AddBuff(BuffID.OnFire, 180);

                Vector2 pos = target.Center;

                int aoeDamage = 12;
                float aoeRadius = 120f;

                foreach (NPC npc in Main.npc)
                {
                    if (npc.active && !npc.friendly && npc.Distance(pos) < aoeRadius)
                    {
                        npc.SimpleStrikeNPC(aoeDamage, 0);
                    }
                }

                for (int i = 0; i < 20; i++)
                {
                    Dust d = Dust.NewDustDirect(
                        target.position,
                        target.width,
                        target.height,
                        DustID.MeteorHead,
                        Main.rand.NextFloat(-2f, 2f),
                        Main.rand.NextFloat(-2f, 2f),
                        100,
                        default,
                        1.5f
                    );
                    d.noGravity = true;
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (meteorSpawned || Main.myPlayer != Projectile.owner)
                return;

            meteorSpawned = true;

            Vector2 spawnPosition = target.Center + new Vector2(
                Main.rand.NextFloat(-90f, 90f),
                -520f);
            Vector2 velocity = (target.Center - spawnPosition)
                .SafeNormalize(Vector2.UnitY)
                * 12f;

            Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                spawnPosition,
                velocity,
                ModContent.ProjectileType<StarScytheMeteorProjectile>(),
                System.Math.Max(1, (int)(damageDone * 0.75f)),
                Projectile.knockBack,
                Projectile.owner,
                target.whoAmI);
        }
    }
}
