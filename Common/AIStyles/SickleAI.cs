using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using LifeStealClass.Common.Utils;

namespace LifeStealClass.Common.AIStyles
{
    public class SickleAI : GlobalProjectile
    {
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
        {
            return entity.aiStyle == CustomAiStyleID.Sickle;
        }
    }
}
