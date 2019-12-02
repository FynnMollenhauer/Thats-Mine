public interface IDamagable
{
    void Damage(DamageInfo damage);

    void Die();
}

public struct DamageInfo
{
    public float damage;
}