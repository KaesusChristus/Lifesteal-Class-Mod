public interface IDashWeapon
{
    float DashSpeed { get; }
    int DashDuration { get; }
    int DashCooldown { get; }
    int DashDamageBonus { get; }
    int DashCritBonus { get; }
}