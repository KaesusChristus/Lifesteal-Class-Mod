using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using LifeStealClass.Content.Core;

namespace LifeStealClass.Content.Items.Weapons.Sickle
{
    public abstract class LifestealSickle : LifeStealItem
    {
        public int attackType = 0;
        public int comboStep = 0;
        public int lastAttackType = 0;
        public int comboExpireTimer = 0;

        protected virtual int ComboResetTime => 120;

        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.rare = ItemRarityID.Blue;

            Item.DamageType = ModContent.GetInstance<HarvesterDamage>();
            Item.knockBack = 4f;

            Item.useTime = 30;
            Item.useAnimation = 30;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item71;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int attack;

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

            Projectile.NewProjectile(source, position, velocity, type, damage, knockback,
                Main.myPlayer,
                attack,
                comboStep);

            comboExpireTimer = 0;

            return false;
        }

        public override void UpdateInventory(Player player)
        {
            if (comboExpireTimer++ >= ComboResetTime)
                comboStep = 0;
        }

        public override bool MeleePrefix() => true;
    }
}