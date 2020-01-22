using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeWall : MonoBehaviour
{

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Throwable") && col.gameObject.GetComponent<Rigidbody>().velocity.sqrMagnitude > 0.1f)
        {
            gameObject.SetActive(false);
        }
    }

}
