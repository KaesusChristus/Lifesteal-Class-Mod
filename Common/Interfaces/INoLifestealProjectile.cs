namespace LifeStealClass.Common.Interfaces
{
    public interface INoLifestealProjectile
    {
    }

    public interface IConditionalHitHealProjectile
    {
        bool TryConsumeHitHeal();
    }
}
