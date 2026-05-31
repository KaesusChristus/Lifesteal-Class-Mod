namespace LifeStealClass.Content.Projectiles.Weapon.Sickle
{
    public class TungstenScytheProjectile : LifestealSickleProjectile
    {
        public override string Texture => "LifeStealClass/Content/Items/Weapons/Sickle/TungstenScythe";

        public override void SetDefaults()
        {
            base.SetDefaults();

            Projectile.timeLeft = 60;
        }
    }
}