using LifeStealClass.Content.Core;
using LifeStealClass.Content.Projectiles.Weapon.Sickle;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;


namespace LifeStealClass.Content.Items.Weapons.Sickle
{
    public class AquaScythe : LifestealSickle
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.scale = 1.2f;

            Item.value = Item.sellPrice(0, 7, 70);
            Item.rare = ItemRarityID.Orange;

            Item.damage = 22;

            Item.useTime = 20;
            Item.useAnimation = 20;

            Item.shoot = ModContent.ProjectileType<AquaScytheProjectile>();
            Item.shootSpeed = 14f;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            float randomAngle = Main.rand.NextFloat(MathHelper.TwoPi);
            Vector2 direction = randomAngle.ToRotationVector2();

            float speed = 5f;
            float offset = -50f;

            Vector2 velocity = direction * speed;
            Vector2 spawnPos = target.Center + direction * offset;

            Projectile proj = Projectile.NewProjectileDirect(target.GetSource_FromThis(), spawnPos, velocity, ProjectileID.Muramasa, Item.damage / 4, 3f, Main.myPlayer);
            proj.DamageType = ModContent.GetInstance<LifestealDamage>();
            proj.penetrate = -1;
        }
    }
}
