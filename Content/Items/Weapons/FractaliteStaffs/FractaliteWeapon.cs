using Terraria;
using LifeStealClass.Content.Core;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace LifeStealClass.Content.Items.Weapons.FractaliteStaffs
{
    public abstract class FractaliteWeapon : LifeStealItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.scale = 1.5f;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.UseSound = SoundID.Item28;

            Item.autoReuse = true;
            Item.crit = 8;
            Item.ArmorPenetration = 5;

            Item.DamageType = ModContent.GetInstance<LifestealDamage>();
            Item.damage = 12;
            Item.knockBack = 3f;
            Item.noMelee = true;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            // Position of the Spawn location of the projetile

            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 40f;
            //muzzleOffset.Y += 15f;

            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }

            // Projetile Speed

            velocity = velocity *= 1.5f;
        }
    }
}
