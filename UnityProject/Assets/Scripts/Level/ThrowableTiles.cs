using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IThrowableTile
{
    void OnDrop();
    void OnPickUp();
    void OnThrow(Vector3 direction, float force);
}

public class ThrowableTiles : MonoBehaviour, IThrowableTile
{
    private Collider col;
    private Rigidbody rb;

    private void Awake()
    {
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        OnDrop();
    }

    public void OnDrop()
    {
        col.isTrigger = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void OnPickUp()
    {
        col.isTrigger = true;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void OnThrow(Vector3 direction, float force)
    {
        StopAllCoroutines();
        StartCoroutine(SwitchOffTrigger(0.5f));

        rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        rb.AddForce(direction * force, ForceMode.Impulse);
    }

    private IEnumerator SwitchOffTrigger(float after)
    {
        yield return new WaitForSeconds(after);

        col.isTrigger = false;
    }
}
