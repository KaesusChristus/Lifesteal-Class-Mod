using LifeStealClass.Content.Core;
using Terraria.Audio;
using Terraria;
using Terraria.ID;

namespace LifeStealClass.Content.Items.Weapons.Spears
{
    public abstract class LifestealSpearWeapon : LifeStealItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.SkipsInitialUseSound[Item.type] = true; // This skips use animation-tied sound playback, so that we're able to make it be tied to use time instead in the UseItem() hook.
            ItemID.Sets.Spears[Item.type] = true; // This allows the game to recognize our new item as a spear.
        }

        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.scale = 1.2f;

            Item.useStyle = ItemUseStyleID.Shoot;

            Item.UseSound = SoundID.Item71;
            Item.autoReuse = true;

            Item.noUseGraphic = true;
            Item.noMelee = true;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }

        public override bool? UseItem(Player player)
        {
            if (!Main.dedServ && Item.UseSound.HasValue)
            {
                SoundEngine.PlaySound(Item.UseSound.Value, player.Center);
            }
            return null;
        }
    }
}
