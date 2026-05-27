public interface IDamagable
{
    public bool IsAlive { get; }
    public void TakeDamage(int amount);
}
