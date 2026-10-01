using Terraria;
using Terraria.ModLoader;

namespace LifeStealClass.Content.Core
{
    public sealed class HarvesterDamage : DamageClass
    {
        public override void SetDefaultStats(Player player)
        {
            player.GetCritChance<HarvesterDamage>() = 2;
            player.GetArmorPenetration<HarvesterDamage>() += 10;
        }
    }
}
