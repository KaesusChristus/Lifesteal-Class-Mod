using LifeStealClass.Common.Utils;
using LifeStealClass.Content.Projectiles.Weapon.Sickle;
using Terraria;
using Terraria.ModLoader;

namespace LifeStealClass.Content.Items.Weapons.Sickle
{
    public class SlimeSickle : LifestealSickle
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.value = Item.sellPrice(0, 0, 70);

            Item.damage = 14;

            Item.useTime = 35;
            Item.useAnimation = 35;

            Item.shoot = ModContent.ProjectileType<SlimeSickleProjectile>();
            Item.shootSpeed = 7f;

            //Item.useStyle = CustomUseStyleID.SickleUseStyle;
        }
    }
}
