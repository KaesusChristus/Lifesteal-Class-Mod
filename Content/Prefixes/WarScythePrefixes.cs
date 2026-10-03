using System;
using System.Collections.Generic;
using LifeStealClass.Content.Items.Weapons.WarScythe;
using LifeStealClass.Content.Projectiles.Weapon.WarScythe;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace LifeStealClass.Content.Prefixes
{
    public abstract class WarScythePrefix : ModPrefix
    {
        protected virtual float DamageMultiplier => 1f;
        protected virtual float KnockbackMultiplier => 1f;
        protected virtual float AttackTimeMultiplier => 1f;
        protected virtual float SizeMultiplier => 1f;
        protected virtual int CriticalStrikeBonus => 0;

        protected virtual float ThrustTimeModifier => 1f;
        protected virtual float SwingTimeModifier => 1f;
        protected virtual float RecoverTimeModifier => 1f;
        protected virtual float SwingArcMultiplier => 1f;
        protected virtual float GripInsetMultiplier => 1f;

        protected virtual float DashSpeedMultiplier => 1f;
        protected virtual float DashDurationMultiplier => 1f;
        protected virtual float DashCooldownMultiplier => 1f;
        protected virtual float DashDamageMultiplier => 1f;
        protected virtual int DashCriticalStrikeBonus => 0;
        protected virtual float DashHealthCostMultiplier => 1f;

        public float ProjectileScaleMultiplier => SizeMultiplier;
        public float ThrustDurationMultiplier => AttackTimeMultiplier * ThrustTimeModifier;
        public float SwingDurationMultiplier => AttackTimeMultiplier * SwingTimeModifier;
        public float RecoverDurationMultiplier => AttackTimeMultiplier * RecoverTimeModifier;
        public float AttackArcMultiplier => SwingArcMultiplier;
        public float HeldGripMultiplier => GripInsetMultiplier;
        public float DashMovementSpeedMultiplier => DashSpeedMultiplier;
        public float DashActiveTimeMultiplier => DashDurationMultiplier;
        public float DashRechargeTimeMultiplier => DashCooldownMultiplier;
        public float DashBonusDamageMultiplier => DashDamageMultiplier;
        public int DashBonusCriticalStrike => DashCriticalStrikeBonus;
        public float DashLifeCostMultiplier => DashHealthCostMultiplier;

        public override PrefixCategory Category => PrefixCategory.Custom;

        public override bool CanRoll(Item item)
        {
            return item.ModItem is LifestealWarScytheWeapon;
        }

        public sealed override void SetStats(
            ref float damageMult,
            ref float knockbackMult,
            ref float useTimeMult,
            ref float scaleMult,
            ref float shootSpeedMult,
            ref float manaMult,
            ref int critBonus)
        {
            damageMult *= DamageMultiplier;
            knockbackMult *= KnockbackMultiplier;
            useTimeMult *= AttackTimeMultiplier;
            scaleMult *= SizeMultiplier;
            critBonus += CriticalStrikeBonus;
        }

        public sealed override IEnumerable<TooltipLine> GetTooltipLines(Item item)
        {
            int handlingBenefits = 0;
            int handlingDrawbacks = 0;
            CountModifier(ThrustTimeModifier, lowerIsBetter: true, ref handlingBenefits, ref handlingDrawbacks);
            CountModifier(SwingTimeModifier, lowerIsBetter: true, ref handlingBenefits, ref handlingDrawbacks);
            CountModifier(RecoverTimeModifier, lowerIsBetter: true, ref handlingBenefits, ref handlingDrawbacks);
            CountModifier(AttackArcMultiplier, lowerIsBetter: false, ref handlingBenefits, ref handlingDrawbacks);
            CountModifier(HeldGripMultiplier, lowerIsBetter: false, ref handlingBenefits, ref handlingDrawbacks);

            if (handlingBenefits + handlingDrawbacks > 0)
            {
                yield return CreateSummaryLine(
                    "PrefixAttackHandling",
                    "attack handling",
                    handlingBenefits,
                    handlingDrawbacks);
            }

            int chargeBenefits = 0;
            int chargeDrawbacks = 0;
            CountModifier(DashMovementSpeedMultiplier, lowerIsBetter: false, ref chargeBenefits, ref chargeDrawbacks);
            CountModifier(DashActiveTimeMultiplier, lowerIsBetter: false, ref chargeBenefits, ref chargeDrawbacks);
            CountModifier(DashRechargeTimeMultiplier, lowerIsBetter: true, ref chargeBenefits, ref chargeDrawbacks);
            CountModifier(DashBonusDamageMultiplier, lowerIsBetter: false, ref chargeBenefits, ref chargeDrawbacks);
            CountModifier(DashBonusCriticalStrike, ref chargeBenefits, ref chargeDrawbacks);
            CountModifier(DashLifeCostMultiplier, lowerIsBetter: true, ref chargeBenefits, ref chargeDrawbacks);

            if (chargeBenefits + chargeDrawbacks > 0)
            {
                yield return CreateSummaryLine(
                    "PrefixChargeCapabilities",
                    "charge capabilities",
                    chargeBenefits,
                    chargeDrawbacks);
            }
        }

        private TooltipLine CreateModifierLine(string name, string text, bool isBad)
        {
            return new TooltipLine(Mod, name, text)
            {
                IsModifier = true,
                IsModifierBad = isBad
            };
        }

        private TooltipLine CreateSummaryLine(
            string name,
            string statName,
            int benefits,
            int drawbacks)
        {
            if (benefits > 0 && drawbacks > 0)
            {
                return new TooltipLine(Mod, name, $"Rebalanced {statName}")
                {
                    OverrideColor = new Color(255, 190, 80)
                };
            }

            bool isBad = drawbacks > 0;
            bool isGreat = benefits + drawbacks >= 4;
            string state = isBad
                ? isGreat ? "Greatly reduced" : "Reduced"
                : isGreat ? "Greatly improved" : "Improved";
            return CreateModifierLine(
                name,
                $"{state} {statName}",
                isBad);
        }

        private static void CountModifier(
            float multiplier,
            bool lowerIsBetter,
            ref int benefits,
            ref int drawbacks)
        {
            int direction = MathF.Abs(multiplier - 1f) < 0.001f
                ? 0
                : multiplier > 1f ? 1 : -1;

            if (direction == 0)
                return;

            bool beneficial = lowerIsBetter ? direction < 0 : direction > 0;
            if (beneficial)
                benefits++;
            else
                drawbacks++;
        }

        private static void CountModifier(
            int modifier,
            ref int benefits,
            ref int drawbacks)
        {
            if (modifier > 0)
                benefits++;
            else if (modifier < 0)
                drawbacks++;
        }
    }

    public static class WarScythePrefixPool
    {
        public static int BestPrefixType => ModContent.PrefixType<ReaperforgedWarScythePrefix>();

        public static bool Contains(int prefix)
        {
            return prefix == ModContent.PrefixType<PiercingWarScythePrefix>()
                || prefix == ModContent.PrefixType<SweepingWarScythePrefix>()
                || prefix == ModContent.PrefixType<ChargingWarScythePrefix>()
                || prefix == ModContent.PrefixType<FrenziedWarScythePrefix>()
                || prefix == ModContent.PrefixType<HulkingWarScythePrefix>()
                || prefix == ModContent.PrefixType<BloodthirstyWarScythePrefix>()
                || prefix == ModContent.PrefixType<RecklessWarScythePrefix>()
                || prefix == ModContent.PrefixType<BluntedWarScythePrefix>()
                || prefix == ModContent.PrefixType<CumbersomeWarScythePrefix>()
                || prefix == ModContent.PrefixType<CrackedWarScythePrefix>()
                || prefix == BestPrefixType;
        }

        public static int Choose(UnifiedRandom random)
        {
            (int Prefix, double Weight)[] prefixes =
            {
                (ModContent.PrefixType<PiercingWarScythePrefix>(), 0.85),
                (ModContent.PrefixType<SweepingWarScythePrefix>(), 0.85),
                (ModContent.PrefixType<ChargingWarScythePrefix>(), 0.85),
                (ModContent.PrefixType<FrenziedWarScythePrefix>(), 1.0),
                (ModContent.PrefixType<HulkingWarScythePrefix>(), 1.0),
                (ModContent.PrefixType<BloodthirstyWarScythePrefix>(), 1.0),
                (ModContent.PrefixType<RecklessWarScythePrefix>(), 1.0),
                (ModContent.PrefixType<BluntedWarScythePrefix>(), 1.0),
                (ModContent.PrefixType<CumbersomeWarScythePrefix>(), 1.0),
                (ModContent.PrefixType<CrackedWarScythePrefix>(), 1.0),
                (BestPrefixType, 0.75)
            };

            double totalWeight = 0.0;
            foreach ((int _, double weight) in prefixes)
                totalWeight += weight;

            double roll = random.NextDouble() * totalWeight;
            foreach ((int prefix, double weight) in prefixes)
            {
                roll -= weight;
                if (roll <= 0.0)
                    return prefix;
            }

            return prefixes[^1].Prefix;
        }

        public static WarScytheStats ApplyAnimationStats(Item item, WarScytheStats stats)
        {
            WarScythePrefix prefix = GetPrefix(item);
            if (prefix == null)
                return stats;

            stats.ThrustTime *= prefix.ThrustDurationMultiplier;
            stats.SwingTime *= prefix.SwingDurationMultiplier;
            stats.RecoverTime *= prefix.RecoverDurationMultiplier;
            stats.SwingArc *= prefix.AttackArcMultiplier;
            stats.ThrustStartGrip *= prefix.HeldGripMultiplier;
            stats.ThrustEndGrip *= prefix.HeldGripMultiplier;
            stats.SwingGrip *= prefix.HeldGripMultiplier;
            stats.RecoverGrip *= prefix.HeldGripMultiplier;
            stats.DashGrip *= prefix.HeldGripMultiplier;
            stats.Scale *= prefix.ProjectileScaleMultiplier;
            return stats;
        }

        public static float GetDashSpeed(Item item, float baseValue)
        {
            return baseValue * (GetPrefix(item)?.DashMovementSpeedMultiplier ?? 1f);
        }

        public static int GetDashDuration(Item item, int baseValue)
        {
            float multiplier = GetPrefix(item)?.DashActiveTimeMultiplier ?? 1f;
            return Math.Max(1, (int)MathF.Round(baseValue * multiplier));
        }

        public static int GetDashCooldown(Item item, int baseValue)
        {
            float multiplier = GetPrefix(item)?.DashRechargeTimeMultiplier ?? 1f;
            return Math.Max(1, (int)MathF.Round(baseValue * multiplier));
        }

        public static int GetDashDamageBonus(Item item, int baseValue)
        {
            float multiplier = GetPrefix(item)?.DashBonusDamageMultiplier ?? 1f;
            return Math.Max(0, (int)MathF.Round(baseValue * multiplier));
        }

        public static int GetDashCritBonus(Item item, int baseValue)
        {
            return baseValue + (GetPrefix(item)?.DashBonusCriticalStrike ?? 0);
        }

        public static int GetDashHealthCost(Item item, int baseValue)
        {
            float multiplier = GetPrefix(item)?.DashLifeCostMultiplier ?? 1f;
            return Math.Max(0, (int)MathF.Round(baseValue * multiplier));
        }

        private static WarScythePrefix GetPrefix(Item item)
        {
            return PrefixLoader.GetPrefix(item.prefix) as WarScythePrefix;
        }
    }

    public sealed class PiercingWarScythePrefix : WarScythePrefix
    {
        protected override float DamageMultiplier => 1.1f;
        protected override int CriticalStrikeBonus => 3;
        protected override float ThrustTimeModifier => 0.9f;
        protected override float DashDamageMultiplier => 1.05f;
    }

    public sealed class SweepingWarScythePrefix : WarScythePrefix
    {
        protected override float KnockbackMultiplier => 1.15f;
        protected override float SizeMultiplier => 1.1f;
        protected override float SwingArcMultiplier => 1.15f;
        protected override float SwingTimeModifier => 0.95f;
    }

    public sealed class ChargingWarScythePrefix : WarScythePrefix
    {
        protected override float DashSpeedMultiplier => 1.12f;
        protected override float DashDurationMultiplier => 1.1f;
        protected override float DashCooldownMultiplier => 0.9f;
        protected override float DashDamageMultiplier => 1.1f;
    }

    public sealed class FrenziedWarScythePrefix : WarScythePrefix
    {
        protected override float DamageMultiplier => 0.92f;
        protected override float KnockbackMultiplier => 0.85f;
        protected override float AttackTimeMultiplier => 0.78f;
        protected override float DashSpeedMultiplier => 1.12f;
        protected override float DashHealthCostMultiplier => 1.2f;
    }

    public sealed class HulkingWarScythePrefix : WarScythePrefix
    {
        protected override float DamageMultiplier => 1.2f;
        protected override float KnockbackMultiplier => 1.35f;
        protected override float AttackTimeMultiplier => 1.18f;
        protected override float SizeMultiplier => 1.12f;
        protected override int CriticalStrikeBonus => -2;
        protected override float SwingArcMultiplier => 1.1f;
        protected override float DashSpeedMultiplier => 0.9f;
        protected override float DashDamageMultiplier => 1.15f;
    }

    public sealed class BloodthirstyWarScythePrefix : WarScythePrefix
    {
        protected override float DamageMultiplier => 1.12f;
        protected override int CriticalStrikeBonus => 4;
        protected override float DashDamageMultiplier => 1.2f;
        protected override int DashCriticalStrikeBonus => 8;
        protected override float DashCooldownMultiplier => 1.1f;
        protected override float DashHealthCostMultiplier => 1.2f;
    }

    public sealed class RecklessWarScythePrefix : WarScythePrefix
    {
        protected override float DamageMultiplier => 1.15f;
        protected override float KnockbackMultiplier => 0.75f;
        protected override float AttackTimeMultiplier => 0.88f;
        protected override float DashSpeedMultiplier => 1.18f;
        protected override float DashCooldownMultiplier => 0.9f;
        protected override float DashHealthCostMultiplier => 1.15f;
    }

    public sealed class BluntedWarScythePrefix : WarScythePrefix
    {
        protected override float DamageMultiplier => 0.82f;
        protected override float KnockbackMultiplier => 0.8f;
        protected override float SwingArcMultiplier => 0.9f;
        protected override float DashDamageMultiplier => 0.85f;
    }

    public sealed class CumbersomeWarScythePrefix : WarScythePrefix
    {
        protected override float AttackTimeMultiplier => 1.22f;
        protected override float SizeMultiplier => 0.9f;
        protected override float GripInsetMultiplier => 0.9f;
        protected override float DashSpeedMultiplier => 0.85f;
        protected override float DashCooldownMultiplier => 1.15f;
    }

    public sealed class CrackedWarScythePrefix : WarScythePrefix
    {
        protected override float DamageMultiplier => 0.9f;
        protected override float KnockbackMultiplier => 0.65f;
        protected override float AttackTimeMultiplier => 1.1f;
        protected override float SizeMultiplier => 0.9f;
        protected override int CriticalStrikeBonus => -3;
        protected override float DashDamageMultiplier => 0.85f;
        protected override int DashCriticalStrikeBonus => -5;
        protected override float DashHealthCostMultiplier => 1.2f;
    }

    // Unique best-in-slot War Scythe prefix with the special reforge feedback.
    public sealed class ReaperforgedWarScythePrefix : WarScythePrefix
    {
        protected override float DamageMultiplier => 1.18f;
        protected override float KnockbackMultiplier => 1.2f;
        protected override float AttackTimeMultiplier => 0.85f;
        protected override float SizeMultiplier => 1.12f;
        protected override int CriticalStrikeBonus => 5;
        protected override float SwingArcMultiplier => 1.12f;
        protected override float GripInsetMultiplier => 1.08f;
        protected override float DashSpeedMultiplier => 1.15f;
        protected override float DashDurationMultiplier => 1.1f;
        protected override float DashCooldownMultiplier => 0.85f;
        protected override float DashDamageMultiplier => 1.2f;
        protected override int DashCriticalStrikeBonus => 8;
        protected override float DashHealthCostMultiplier => 0.9f;
    }
}
