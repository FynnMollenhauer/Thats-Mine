using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitfallCollision : MonoBehaviour
{
    public int damage;

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            col.gameObject.GetComponent<IDamagable>().Damage(new DamageInfo() { damage = this.damage });
        }
    }
}
