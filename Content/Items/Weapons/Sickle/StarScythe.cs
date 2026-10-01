using LifeStealClass.Common.GlobalItems.Other;
using Microsoft.Xna.Framework;
using LifeStealClass.Content.Projectiles.Weapon.Sickle;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace LifeStealClass.Content.Items.Weapons.Sickle
{
    public class StarScythe : LifestealSickle
    {
        private int attack;
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 62;
            Item.height = 48;
            Item.scale = 1.2f;

            Item.value = Item.sellPrice(1, 20);
            Item.rare = ItemRarityID.Orange;

            Item.damage = 18;

            Item.useTime = 30;
            Item.useAnimation = 30;

            Item.shoot = ModContent.ProjectileType<StarScytheProjectile>();
            Item.shootSpeed = 8f;

            Item.GetGlobalItem<OnHitHeal>().baseHealOnHit = 2;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.MeteoriteBar, 18);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            if (comboStep < 2)
            {
                attack = 0; // Swing
                comboStep++;
            }
            else
            {
                attack = 1; // Spin
                comboStep = 0;
            }

            lastAttackType = attack;


            if (attack == 1)
            {
                Item.GetGlobalItem<OnHitHeal>().baseHealOnHit = 4; // Spin
            }
            else
            {
                Item.GetGlobalItem<OnHitHeal>().baseHealOnHit = 2; // Swing
            }

            Projectile.NewProjectile(source, position, velocity, type, damage, knockback,
                Main.myPlayer,
                attack,
                comboStep);

            comboExpireTimer = 0;

            return false;
        }
    }
}
