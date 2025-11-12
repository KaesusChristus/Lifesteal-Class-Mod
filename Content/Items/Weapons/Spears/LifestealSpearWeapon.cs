using LifeStealClass.Content.Core;
using Terraria.Audio;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using LifeStealClass.Common.ModPlayers;
using LifeStealClass.Common.GlobalItems.Other;

namespace LifeStealClass.Content.Items.Weapons.Spears
{
    public abstract class LifestealSpearWeapon : LifeStealItem, IDashWeapon
    {
        public float DashSpeed => 12f; // Base DashSpeed
        public int DashDuration => 8; // Base BashDuration
        public int DashCooldown => 240; // Base Dashcooldown in ticks (60 ticks = 1 Sec)
        public int DashDamageBonus => 50; // Base von 50 extra damage
        public int DashCritBonus => 70; // Base von 70 % mehr auf Critchance
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

            Item.GetGlobalItem<HealthCost>().dashHealthCost = 1;

            var dashStats = Item.GetGlobalItem<DashBonusStats>();
            dashStats.dashDamageBonus = DashDamageBonus;
            dashStats.dashCritBonus = DashCritBonus;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2) // Rechtsklick Dash
            {
                Vector2 dir = Main.MouseWorld - player.Center;
                var dashPlayer = player.GetModPlayer<WarScytheDash>();
                dashPlayer.TryDash(
                    dir,
                    Item,
                    Item.shoot,
                    Item.shootSpeed / 2f
                );

                return false;
            }

            return player.ownedProjectileCounts[Item.shoot] < 1 && base.CanUseItem(player);
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
