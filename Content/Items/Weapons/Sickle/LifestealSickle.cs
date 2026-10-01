using LifeStealClass.Content.Core;
using LifeStealClass.Content.Projectiles.Weapon.Sickle;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace LifeStealClass.Content.Items.Weapons.Sickle
{
    public abstract class LifestealSickle : LifeStealItem
    {
        private int comboStep;
        private ulong lastAttackUpdate;

        protected virtual ulong ComboResetTime => 45;

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 64;
            Item.height = 64;
            Item.rare = ItemRarityID.Blue;
            Item.knockBack = 4f;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item71;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;
        }

        public override bool Shoot(
            Player player,
            EntitySource_ItemUse_WithAmmo source,
            Vector2 position,
            Vector2 velocity,
            int type,
            int damage,
            float knockback)
        {
            ulong currentUpdate = Main.GameUpdateCount;
            if (currentUpdate - lastAttackUpdate > ComboResetTime)
            {
                comboStep = 0;
            }

            SickleAttackType attack = (SickleAttackType)comboStep;
            ConfigureAttack(attack);

            Projectile.NewProjectile(
                source,
                position,
                velocity,
                type,
                damage,
                knockback,
                player.whoAmI,
                (float)attack);

            comboStep = (comboStep + 1) % 3;
            lastAttackUpdate = currentUpdate;
            return false;
        }

        public override void UpdateInventory(Player player)
        {
            if (comboStep != 0 && Main.GameUpdateCount - lastAttackUpdate > ComboResetTime)
            {
                comboStep = 0;
            }
        }

        protected virtual void ConfigureAttack(SickleAttackType attack)
        {
        }

        public override bool MeleePrefix() => true;
    }
}
