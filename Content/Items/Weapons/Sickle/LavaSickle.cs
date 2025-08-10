using LifeStealClass.Content.Projectiles.Weapon.Sickle;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using LifeStealClass.Content.Items.Ingredients;
using LifeStealClass.Common.GlobalItems.Other;

namespace LifeStealClass.Content.Items.Weapons.Sickle
{
    public class LavaSickle : LifestealSickle
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.scale = 1.3f;

            Item.value = Item.sellPrice(0, 8);
            Item.rare = ItemRarityID.Orange;

            Item.damage = 29;

            Item.useTime = 35;
            Item.useAnimation = 35;

            Item.shoot = ModContent.ProjectileType<LavaSickleProjectile>();
            Item.shootSpeed = 9f;

            Item.GetGlobalItem<OnHitHeal>().baseHealOnHit = 2;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.NewProjectile(target.GetSource_FromThis(), target.position, Vector2.Zero, ProjectileID.Volcano, Item.damage / 2, Item.knockBack, Main.myPlayer);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.HellstoneBar, 25);
            recipe.AddIngredient(ModContent.ItemType<BloodyVeins>(), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
