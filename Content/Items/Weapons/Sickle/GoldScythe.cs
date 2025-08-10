using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using LifeStealClass.Content.Items.Ingredients;
using Microsoft.Xna.Framework;

namespace LifeStealClass.Content.Items.Weapons.Sickle
{
    public class GoldScythe : LifestealSickle
    {
        private int tickTimer = 0;
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.value = Item.sellPrice(0, 2, 30);

            Item.damage = 17;

            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.scale = 1.2f;
        }

        public override void HoldItem(Player player)
        {
            if (player.itemAnimation > 0)
            {
                tickTimer++;

                if (tickTimer >= 60)
                {
                    player.ChangeDir(Main.MouseWorld.X > player.Center.X ? 1 : -1);
                    tickTimer = 0;

                    if (player.whoAmI == Main.myPlayer)
                    {
                        Vector2 direction = Main.MouseWorld - player.Center;
                        direction.Normalize();
                        direction *= 7f;

                        Projectile.NewProjectile( player.GetSource_ItemUse(Item), player.Center, direction, ProjectileID.RubyBolt, Item.damage, Item.knockBack, player.whoAmI);
                    }
                }
            }
            else
            {
                tickTimer = 0; // Reset wenn nicht mehr geschwungen wird
            }
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {

            if (Main.rand.NextBool(3)) // ~33% Chance pro Frame während Schwung
            {
                int dust = Dust.NewDust(hitbox.TopLeft(), hitbox.Width, hitbox.Height, DustID.GoldCoin);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].scale = 1.4f;
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.GoldBar, 12);
            recipe.AddIngredient(ModContent.ItemType<LifeShard>(), 6);
            recipe.AddTile(TileID.WorkBenches);
            recipe = CreateRecipe();
        }
    }
}
