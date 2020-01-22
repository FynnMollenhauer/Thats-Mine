using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public GameObject target;

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player") || col.gameObject.layer == LayerMask.NameToLayer("Throwable"))
        {
            target.SetActive(false);
            gameObject.SetActive(false);
        }
    }

}
