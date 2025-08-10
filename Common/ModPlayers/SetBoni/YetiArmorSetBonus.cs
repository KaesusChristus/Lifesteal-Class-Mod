using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using System.Collections.Generic;
using System;
using LifeStealClass.Content.Projectiles;
using Mono.Cecil;
using Terraria.DataStructures;
using LifeStealClass.Content.Core;
using LifeStealClass.Content.Items.Armor.YetiArmor;

namespace LifeStealClass.Common.ModPlayers.SetBoni
{
    public class YetiArmorSetBonus : ModPlayer
    {
        public bool setBonusActive = false;
        int healDone;

        private bool WearsFullSet(Player player)
        {
            return player.armor[0].type == ModContent.ItemType<YetiHood>() &&
                   player.armor[1].type == ModContent.ItemType<YetiBreastplate>() &&
                   player.armor[2].type == ModContent.ItemType<YetiLeggins>();
        }

        public override void UpdateEquips()
        {
            if (WearsFullSet(Player))
            {
                setBonusActive = true; // Aktiviere den Set-Bonus
            }
            else
            {
                setBonusActive = false; // Deaktiviere den Set-Bonus
            }
        }


        public override void UpdateDead()
        {
            setBonusActive = false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (setBonusActive)
            {
                IEntitySource source = Player.GetSource_ItemUse(Player.HeldItem);
                Vector2 spawnPosition = Player.position;
                healDone = Player.GetModPlayer<LifestealEffectsPlayer>().GetHealAmountPerSecond();

                if (healDone > 0 && hit.DamageType == ModContent.GetInstance<LifestealDamage>() && hit.Crit)
                {
                    Projectile.NewProjectile(source, spawnPosition, Vector2.Zero, ProjectileID.SpectreWrath, healDone * 2, 0, Main.myPlayer, 12f);
                }

            }
        }
    }
}
