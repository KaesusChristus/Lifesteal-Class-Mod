using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace LifeStealClass.Common.GlobalItems.VanillaItemModification
{
    public class VanillaItemModify : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.BoneHelm;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ItemID.BoneHelm)
            {
                TooltipUtilities.LifeStealToolTip(tooltips, Mod);
            }
        }

    }

    public class TooltipUtilities
    {
        // Diese Methode fügt den Tooltip für Lifesteal-Items hinzu
        public static void LifeStealToolTip(List<TooltipLine> tooltips, Mod mod)
        {
            // Tooltip hinzufügen, dass es ein Lifesteal-Item ist
            tooltips.Add(new TooltipLine(mod, "LifestealItem", "This item belongs now to the Lifesteal class")
            {
                OverrideColor = new Color(180, 0, 0)
            });

            // Optional: Tooltip für Schaden ändern, wenn vorhanden
            TooltipLine damageTooltip = tooltips.Find(x => x.Name == "Damage" && x.Mod == "Terraria");
            if (damageTooltip != null)
            {
                damageTooltip.OverrideColor = new Color(180, 0, 0);
            }
        }
    }
}
