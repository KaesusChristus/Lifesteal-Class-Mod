using LifeStealClass.Content.Items.Armor.ZomboidArmor;
using Terraria.ModLoader;
using Terraria;
using LifeStealClass.Common.GlobalItems.Other;
using LifeStealClass.Content.Core;

namespace LifeStealClass.Common.ModPlayers.SetBoni
{
    public class ZomboidArmorSetBonus : ModPlayer
    {
        public bool setBonusActive;

        public override void UpdateDead()
        {
            // Stelle sicher, dass die Variable zurückgesetzt wird, wenn der Spieler stirbt
            setBonusActive = false;
        }

        public override void PostUpdate()
        {
            var modPlayer = Player.GetModPlayer<LifestealEffectsPlayer>();
            bool isUnder50 = Player.statLife < Player.statLifeMax2 / 2;

            setBonusActive = WearsFullSet(Player);

            foreach (var item in Player.inventory)
            {
                if (!item.IsAir && item.DamageType == ModContent.GetInstance<LifestealDamage>())
                {
                    var healData = item.GetGlobalItem<OnHitHeal>();

                    if (setBonusActive)
                    {
                        if (isUnder50)
                        {
                            healData.bonusHealOnHit = 1;
                        }
                        else
                        {
                            healData.bonusHealOnHit = 0;
                        }
                    }
                    else
                    {
                        healData.bonusHealOnHit = 0;
                    }
                }
            }
            modPlayer.reduceLifecostFlat = (setBonusActive && !isUnder50) ? 2 : 0;
        }


        private bool WearsFullSet(Player player)
        {
            return player.armor[0].type == ModContent.ItemType<ZomboidHead>() &&
                   player.armor[1].type == ModContent.ItemType<ZomboidBreastplate>() &&
                   player.armor[2].type == ModContent.ItemType<ZomboidLegs>();
        }
    }
}
