﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float Speed { get; set; }
    public Vector3 Direction { get; set; }
    public DamageInfo DamageInfo { get; set; }
    public GameObject Owner { get; set; }

    // Update is called once per frame
    void Update()
    {
        transform.position += Direction * Speed * Time.deltaTime;
        transform.forward = Direction;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == Owner)
        {
            return;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.gameObject.GetComponent<IDamagable>().Damage(DamageInfo);
        }

        gameObject.Despawn();
    }
}
