using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : MonoBehaviour, IDamagable
{
    public float health;
    public float damage;

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
            if (col.gameObject.GetComponent<Rigidbody>().velocity.sqrMagnitude > 0.1f)
            {
                Damage(new DamageInfo() { damage = 1 });
            }

        }
        else if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            col.gameObject.GetComponent<IDamagable>().Damage(new DamageInfo() { damage = this.damage });
        }
    }
}
