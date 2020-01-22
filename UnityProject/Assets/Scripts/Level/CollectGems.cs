using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectGems : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            gameObject.SetActive(false);
        }
    }
}
