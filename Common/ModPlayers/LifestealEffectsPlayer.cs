using Terraria;
using Terraria.ModLoader;
using LifeStealClass.Content.Core;
using LifeStealClass.Common.GlobalItems.Other;
using Microsoft.Xna.Framework;

namespace LifeStealClass.Common.ModPlayers
{
    public class LifestealEffectsPlayer : ModPlayer
    {
        private int totalDamageDealt;
        private const float lifestealPercentage = 0.05f;
        private bool crit;
        public int healAmount;
        private int healAmountAccumulator;
        private double lastUpdate;
        private int currentHealAmountPerSecond;
        private int lastHealAmountPerSecond;
        private int constHealAmount;
        private int bonusHealAdd;
        private int bonusHealMulti = 1;
        public int bonusHealOnHit;
        public bool allowHeal = true;

        private int overheal;
        public int getOverHeal;

        public int reduceLifecostFlat = 0;


        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (proj.localAI.Length > 1 && proj.localAI[1] == 1f || allowHeal == false)
                return;

            if (proj.DamageType == ModContent.GetInstance<LifestealDamage>())
            {
                crit = hit.Crit;
                AddDamage(damageDone);
                IsCrit(crit);

                var tracked = proj.GetGlobalProjectile<ProjectileSourceTracker>();
                Item sourceItem = tracked?.sourceItem;

                if (sourceItem != null)
                {
                    var healData = sourceItem.GetGlobalItem<OnHitHeal>();
                    if (healData.baseHealOnHit > 0)
                    {
                        int totalBonus = healData.bonusHealOnHit; // direkt vom Item
                        int heal = healData.baseHealOnHit + totalBonus;

                        int oldLife = Player.statLife;
                        int maxLife = Player.statLifeMax2;

                        Player.statLife += heal;
                        if (Player.statLife > maxLife)
                            Player.statLife = maxLife;

                        int actualHealed = Player.statLife - oldLife;
                        overheal = heal - actualHealed;

                        if (maxLife != oldLife)
                            Player.HealEffect(actualHealed);

                        if (overheal > 0)
                        {
                            OverHeal(overheal);
                        }
                    }
                }
            }
        }


        public void AddDamage(int damageDone)
        {
            totalDamageDealt = damageDone;
        }

        public void IsCrit(bool critDone)
        {
            crit = critDone;
        }

        public void SetHealAmount(int setHealAmount)
        {
            constHealAmount = setHealAmount;
        }

        public void BonusHealAmountAdd(int bonusHealAmountAdd)
        {
            bonusHealAdd = bonusHealAmountAdd;
        }

        public void BonusHealAmountMulti(int bonusHealAmountMulti)
        {
            bonusHealMulti = bonusHealAmountMulti;
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
            if (allowHeal == false)
                return;

            if (totalDamageDealt > 0 && crit || constHealAmount > 0)
            {
                if (constHealAmount == 0)
                {
                    healAmount = (int)(totalDamageDealt * lifestealPercentage);
                } 
                else
                {
                    healAmount = constHealAmount;
                }

                Player.lifeRegenTime = 0;
                Player.lifeRegen += (healAmount * 2);
                int totalHeal = (healAmount * bonusHealMulti) + bonusHealAdd;
                int oldLife = Player.statLife;
                int maxLife = Player.statLifeMax2;

                // Heilung anwenden
                Player.statLife += totalHeal;
                if (Player.statLife > maxLife)
                    Player.statLife = maxLife;

                int actualHealed = Player.statLife - oldLife;
                overheal = totalHeal - actualHealed;

                if (maxLife != oldLife && actualHealed != 0)
                    Player.HealEffect(actualHealed);

                // Overheal amount
                if (overheal > 0)
                {
                    OverHeal(overheal);
                }


                healAmountAccumulator += healAmount;
            }

            totalDamageDealt = 0;
            constHealAmount = 0;
            bonusHealAdd = 0;
            bonusHealMulti = 1;
            allowHeal = true;

            if (Main.GameUpdateCount - lastUpdate >= 1)
            {
                currentHealAmountPerSecond = healAmountAccumulator;
                if (currentHealAmountPerSecond > 0)
                {
                    lastHealAmountPerSecond = currentHealAmountPerSecond;
                }
                healAmountAccumulator = 0;
                lastUpdate = Main.GameUpdateCount;
            }
        }

        public int GetHealAmountPerSecond()
        {
            return currentHealAmountPerSecond != 0 ? currentHealAmountPerSecond : lastHealAmountPerSecond;
        }

        public void OverHeal(int value)
        {
            getOverHeal = value;

            if (value > 0)
            {
                Color overhealColor = new Color(255, 100, 180); // Rosa/Pink
                CombatText.NewText(Player.Hitbox, overhealColor, $"{value}");
            }
        }
    }
}
