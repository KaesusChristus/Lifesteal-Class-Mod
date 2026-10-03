using System;
using System.Collections.Generic;
using LifeStealClass.Content.Items.Weapons.Sickle;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace LifeStealClass.Content.Prefixes
{
    public abstract class ScythePrefix : ModPrefix
    {
        protected virtual float DamageMultiplier => 1f;
        protected virtual float KnockbackMultiplier => 1f;
        protected virtual float UseTimeMultiplier => 1f;
        protected virtual float SizeMultiplier => 1f;
        protected virtual int CriticalStrikeBonus => 0;
        protected virtual int PrefixHealModifier => 0;
        protected virtual float HeavyDamageMultiplier => 1f;

        public float SwingTimeMultiplier => UseTimeMultiplier;
        public int HealModifier => Math.Clamp(PrefixHealModifier, -3, 4);
        public float HeavyStrikeDamageMultiplier => HeavyDamageMultiplier;

        public override PrefixCategory Category => PrefixCategory.Custom;

        public override bool CanRoll(Item item)
        {
            return item.ModItem is LifestealSickle;
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
            useTimeMult *= UseTimeMultiplier;
            scaleMult *= SizeMultiplier;
            critBonus += CriticalStrikeBonus;
        }

        public sealed override IEnumerable<TooltipLine> GetTooltipLines(Item item)
        {
            if (HealModifier != 0)
            {
                yield return CreateModifierLine(
                    "PrefixHeal",
                    $"{HealModifier:+0;-0} Heal",
                    HealModifier < 0);
            }

            int heavyDamagePercent = GetPercentageChange(HeavyDamageMultiplier);
            if (heavyDamagePercent != 0)
            {
                yield return CreateModifierLine(
                    "PrefixHeavyDamage",
                    $"{heavyDamagePercent:+0;-0}% heavy attack damage",
                    heavyDamagePercent < 0);
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

        private static int GetPercentageChange(float multiplier)
        {
            return (int)MathF.Round((multiplier - 1f) * 100f);
        }
    }

    public static class ScythePrefixPool
    {
        public static int BestPrefixType => ModContent.PrefixType<SoulforgedScythePrefix>();

        public static bool Contains(int prefix)
        {
            return prefix == ModContent.PrefixType<HonedScythePrefix>()
                || prefix == ModContent.PrefixType<SweepingScythePrefix>()
                || prefix == ModContent.PrefixType<RelentlessScythePrefix>()
                || prefix == ModContent.PrefixType<FrenziedScythePrefix>()
                || prefix == ModContent.PrefixType<HulkingScythePrefix>()
                || prefix == ModContent.PrefixType<GlassEdgedScythePrefix>()
                || prefix == ModContent.PrefixType<RecklessScythePrefix>()
                || prefix == ModContent.PrefixType<BluntedScythePrefix>()
                || prefix == ModContent.PrefixType<CumbersomeScythePrefix>()
                || prefix == ModContent.PrefixType<CrackedScythePrefix>()
                || prefix == BestPrefixType;
        }

        public static int Choose(UnifiedRandom random)
        {
            (int Prefix, double Weight)[] prefixes =
            {
                (ModContent.PrefixType<HonedScythePrefix>(), 0.85),
                (ModContent.PrefixType<SweepingScythePrefix>(), 0.85),
                (ModContent.PrefixType<RelentlessScythePrefix>(), 0.85),
                (ModContent.PrefixType<FrenziedScythePrefix>(), 1.0),
                (ModContent.PrefixType<HulkingScythePrefix>(), 1.0),
                (ModContent.PrefixType<GlassEdgedScythePrefix>(), 1.0),
                (ModContent.PrefixType<RecklessScythePrefix>(), 1.0),
                (ModContent.PrefixType<BluntedScythePrefix>(), 1.0),
                (ModContent.PrefixType<CumbersomeScythePrefix>(), 1.0),
                (ModContent.PrefixType<CrackedScythePrefix>(), 1.0),
                (BestPrefixType, 0.75)
            };

            double totalWeight = 0.0;
            foreach ((int _, double weight) in prefixes)
            {
                totalWeight += weight;
            }

            double roll = random.NextDouble() * totalWeight;
            foreach ((int prefix, double weight) in prefixes)
            {
                roll -= weight;
                if (roll <= 0.0)
                {
                    return prefix;
                }
            }

            return prefixes[^1].Prefix;
        }

        public static float GetSwingSpeedMultiplier(Item item)
        {
            ScythePrefix prefix = GetPrefix(item);
            return prefix == null ? 1f : 1f / prefix.SwingTimeMultiplier;
        }

        public static int GetHealModifier(Item item)
        {
            return GetPrefix(item)?.HealModifier ?? 0;
        }

        public static float GetHeavyDamageMultiplier(Item item)
        {
            return GetPrefix(item)?.HeavyStrikeDamageMultiplier ?? 1f;
        }

        private static ScythePrefix GetPrefix(Item item)
        {
            return PrefixLoader.GetPrefix(item.prefix) as ScythePrefix;
        }
    }

    // Positive prefixes
    public sealed class HonedScythePrefix : ScythePrefix
    {
        protected override float DamageMultiplier => 1.1f;
        protected override int CriticalStrikeBonus => 3;
        protected override int PrefixHealModifier => 1;
        protected override float HeavyDamageMultiplier => 1.05f;
    }

    public sealed class SweepingScythePrefix : ScythePrefix
    {
        protected override float KnockbackMultiplier => 1.15f;
        protected override int PrefixHealModifier => 1;
        protected override float SizeMultiplier => 1.15f;
    }

    public sealed class RelentlessScythePrefix : ScythePrefix
    {
        protected override float DamageMultiplier => 1.08f;
        protected override float UseTimeMultiplier => 0.9f;
        protected override int PrefixHealModifier => 1;
    }

    // Mixed prefixes: stronger identity in exchange for a real downside.
    public sealed class FrenziedScythePrefix : ScythePrefix
    {
        protected override float DamageMultiplier => 0.9f;
        protected override float KnockbackMultiplier => 0.85f;
        protected override float UseTimeMultiplier => 0.78f;
        protected override int PrefixHealModifier => -1;
        protected override float HeavyDamageMultiplier => 0.92f;
    }

    public sealed class HulkingScythePrefix : ScythePrefix
    {
        protected override float DamageMultiplier => 1.2f;
        protected override float KnockbackMultiplier => 1.35f;
        protected override float UseTimeMultiplier => 1.2f;
        protected override int CriticalStrikeBonus => -2;
        protected override int PrefixHealModifier => 2;
        protected override float HeavyDamageMultiplier => 1.1f;
        protected override float SizeMultiplier => 1.1f;
    }

    public sealed class GlassEdgedScythePrefix : ScythePrefix
    {
        protected override float DamageMultiplier => 1.25f;
        protected override float KnockbackMultiplier => 0.65f;
        protected override int CriticalStrikeBonus => 6;
        protected override int PrefixHealModifier => -2;
        protected override float HeavyDamageMultiplier => 1.12f;
        protected override float SizeMultiplier => 0.85f;
    }

    public sealed class RecklessScythePrefix : ScythePrefix
    {
        protected override float DamageMultiplier => 1.15f;
        protected override float KnockbackMultiplier => 0.75f;
        protected override float UseTimeMultiplier => 0.88f;
        protected override int PrefixHealModifier => -1;
        protected override float HeavyDamageMultiplier => 1.08f;
    }

    // Negative prefixes
    public sealed class BluntedScythePrefix : ScythePrefix
    {
        protected override float DamageMultiplier => 0.82f;
        protected override float KnockbackMultiplier => 0.8f;
        protected override int PrefixHealModifier => -2;
        protected override float HeavyDamageMultiplier => 0.9f;
    }

    public sealed class CumbersomeScythePrefix : ScythePrefix
    {
        protected override float UseTimeMultiplier => 1.22f;
        protected override int PrefixHealModifier => -1;
        protected override float SizeMultiplier => 0.85f;
    }

    public sealed class CrackedScythePrefix : ScythePrefix
    {
        protected override float DamageMultiplier => 0.9f;
        protected override float KnockbackMultiplier => 0.65f;
        protected override float UseTimeMultiplier => 1.1f;
        protected override int PrefixHealModifier => -3;
        protected override float HeavyDamageMultiplier => 0.85f;
        protected override float SizeMultiplier => 0.9f;
    }

    // The unique best-in-slot prefix. It triggers the vanilla-style best-reforge feedback.
    public sealed class SoulforgedScythePrefix : ScythePrefix
    {
        protected override float DamageMultiplier => 1.18f;
        protected override float KnockbackMultiplier => 1.2f;
        protected override float UseTimeMultiplier => 0.85f;
        protected override int CriticalStrikeBonus => 5;
        protected override int PrefixHealModifier => 4;
        protected override float HeavyDamageMultiplier => 1.1f;
        protected override float SizeMultiplier => 1.12f;
    }

    public sealed class SoulforgedRarity : ModRarity
    {
        public override Color RarityColor => Main.DiscoColor;
    }
}
