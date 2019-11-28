using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIntervalFiring : EnemyBehavior
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float fireIntervalMin;
    [SerializeField] float fireIntervalMax;
    [SerializeField] float projectileSpeed;

    float firingCountdown = -1;

    private void Update()
    {
        if (firingCountdown > 0)
        {
            firingCountdown -= Time.deltaTime;
            return;
        }

        GameObject projInstance = projectilePrefab.Spawn(transform.position);
        EnemyProjectile projectile = projInstance.GetComponent<EnemyProjectile>();
        projectile.Direction = transform.forward;
        projectile.Speed = projectileSpeed;
        projectile.DamageInfo = new DamageInfo() { damage = Stat.damage };

        firingCountdown = Random.Range(fireIntervalMin, fireIntervalMax);        
    }
}
