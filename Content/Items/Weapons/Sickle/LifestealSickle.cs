using LifeStealClass.Common.GlobalItems.Other;
using LifeStealClass.Common.Systems;
using LifeStealClass.Content.Core;
using LifeStealClass.Content.Prefixes;
using LifeStealClass.Content.Projectiles.Weapon.Sickle;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace LifeStealClass.Content.Items.Weapons.Sickle
{
    public abstract class LifestealSickle : LifeStealItem
    {
        private int comboStep;
        private ulong lastAttackUpdate;

        protected virtual ulong ComboResetTime => 45;
        protected abstract int HeavyHitHeal { get; }

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
            Item.shootSpeed = 7f;
            Item.GetGlobalItem<OnHitHeal>().baseHealOnHit = HeavyHitHeal;
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

        public override int ChoosePrefix(UnifiedRandom rand)
        {
            return ScythePrefixPool.Choose(rand);
        }

        public override bool AllowPrefix(int pre)
        {
            return ScythePrefixPool.Contains(pre);
        }

        public override bool? PrefixChance(int pre, UnifiedRandom rand)
        {
            if (pre > 0 && !ScythePrefixPool.Contains(pre))
            {
                return false;
            }

            return null;
        }

        public override bool CanReforge()
        {
            return ScytheReforgeFeedbackSystem.CanReforge(Item);
        }

        public override void PostReforge()
        {
            if (Item.prefix != ScythePrefixPool.BestPrefixType)
                return;

            ScytheReforgeFeedbackSystem.RegisterSoulforged(Item);
        }

        public override bool MeleePrefix() => false;
        public override bool WeaponPrefix() => false;
    }
}
