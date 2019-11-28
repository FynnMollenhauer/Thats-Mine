using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : MonoBehaviour, IDamagable
{
    [SerializeField] float health;
    [SerializeField] float movementSpeed;
    [SerializeField] float damage;

    public float Health { get { return health; } }
    public float MovementSpeed { get { return movementSpeed; } }

    private float currentHealth;

    void Start()
    {
        currentHealth -= health;
    }

    public void Damage(DamageInfo damageInfo)
    {
        currentHealth -= damageInfo.damage;

        if (currentHealth <= 0)
            Die();
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Throwable"))
        {
            Damage(new DamageInfo() { damage = 1 });
        }
        else if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            col.gameObject.GetComponent<IDamagable>().Damage(new DamageInfo() { damage = this.damage });
        }
    }
}
