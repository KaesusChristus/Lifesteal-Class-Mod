using LifeStealClass.Common.GlobalItems.Other;
using LifeStealClass.Common.GlobalProjectiles;
using LifeStealClass.Common.Interfaces;
using LifeStealClass.Content.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace LifeStealClass.Common.ModPlayers
{
    public class LifestealEffectsPlayer : ModPlayer
    {
        private const float LifestealPercentage = 0.05f;
        private const ulong TicksPerSecond = 60;

        private int criticalDamageDealt;
        private int queuedHealAmount;
        private int healAmountAccumulator;
        private ulong lastHealingSampleUpdate;
        private int currentHealAmountPerSecond;
        private int bonusHealAdd;
        private int bonusHealMultiplier = 1;

        public int bonusHealOnHit;
        public bool allowHeal = true;
        public int getOverHeal;
        public int reduceLifecostFlat;

        public override void OnHitNPCWithProj(
            Projectile projectile,
            NPC target,
            NPC.HitInfo hit,
            int damageDone)
        {
            if (projectile.ModProjectile is INoLifestealProjectile || !allowHeal)
                return;

            if (projectile.DamageType != ModContent.GetInstance<HarvesterDamage>())
                return;

            RegisterHit(damageDone, hit.Crit);

            Item sourceItem = projectile
                .GetGlobalProjectile<LifestealEffectsProjectile>()
                .SourceItem;

            if (sourceItem == null)
                return;

            OnHitHeal healData = sourceItem.GetGlobalItem<OnHitHeal>();
            int totalHeal = healData.GetTotalHeal(sourceItem);
            if (healData.baseHealOnHit == 0 || totalHeal == 0)
                return;

            if (projectile.ModProjectile is IConditionalHitHealProjectile conditionalHeal
                && !conditionalHeal.TryConsumeHitHeal())
            {
                return;
            }

            SetHealAmount(totalHeal);
        }

        public void RegisterHit(int damageDone, bool isCritical)
        {
            if (isCritical)
            {
                criticalDamageDealt += damageDone;
            }
        }

        public void SetHealAmount(int healAmount)
        {
            queuedHealAmount += healAmount;
        }

        public void BonusHealAmountAdd(int bonusHealAmountAdd)
        {
            bonusHealAdd += bonusHealAmountAdd;
        }

        public void BonusHealAmountMulti(int bonusHealAmountMulti)
        {
            bonusHealMultiplier *= bonusHealAmountMulti;
        }

        public int GetHealBonus()
        {
            return bonusHealAdd + bonusHealOnHit;
        }

        public override void ResetEffects()
        {
            bonusHealOnHit = 0;
            allowHeal = true;
        }

        public override void UpdateLifeRegen()
        {
            if (allowHeal)
            {
                int lifestealAmount = (int)(criticalDamageDealt * LifestealPercentage);
                int baseHealAmount = lifestealAmount + queuedHealAmount;

                if (baseHealAmount > 0)
                {
                    Player.lifeRegenTime = 0;
                    Player.lifeRegen += baseHealAmount * 2;

                    int totalHeal = (baseHealAmount * bonusHealMultiplier) + GetHealBonus();
                    healAmountAccumulator += ApplyHealing(totalHeal);
                }
            }

            criticalDamageDealt = 0;
            queuedHealAmount = 0;
            bonusHealAdd = 0;
            bonusHealMultiplier = 1;

            UpdateHealingPerSecond();
        }

        public int GetHealAmountPerSecond()
        {
            return currentHealAmountPerSecond;
        }

        private int ApplyHealing(int amount)
        {
            int oldLife = Player.statLife;
            Player.statLife = System.Math.Min(Player.statLife + amount, Player.statLifeMax2);

            int actualHealed = Player.statLife - oldLife;
            int overheal = amount - actualHealed;

            if (actualHealed > 0)
            {
                Player.HealEffect(actualHealed);
            }

            if (overheal > 0)
            {
                OverHeal(overheal);
            }

            return actualHealed;
        }

        private void UpdateHealingPerSecond()
        {
            if (Main.GameUpdateCount - lastHealingSampleUpdate < TicksPerSecond)
                return;

            currentHealAmountPerSecond = healAmountAccumulator;
            healAmountAccumulator = 0;
            lastHealingSampleUpdate = Main.GameUpdateCount;
        }

        public void OverHeal(int value)
        {
            getOverHeal = value;

            if (value > 0)
            {
                Color overhealColor = new Color(255, 100, 180);
                CombatText.NewText(Player.Hitbox, overhealColor, value.ToString());
            }
        }
    }
}
