using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float lifeTime;

    float remaining;

    void OnSpawn()
    {
        remaining = lifeTime;
    }

    void OnDespawn()
    {

    }

    private void Update()
    {
        remaining -= Time.deltaTime;

        if (remaining <= 0)
        {
            gameObject.Despawn();
        }
    }
}
