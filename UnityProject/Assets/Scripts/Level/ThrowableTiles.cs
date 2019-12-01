﻿using System.Collections;
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

    private bool shouldSnap;

    private void Awake()
    {
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();

        OnDrop();
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.identity;
    }

    public void OnDrop()
    {
        col.isTrigger = false;
        rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
    }

    public void OnPickUp()
    {
        col.isTrigger = true;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        shouldSnap = true;
    }

    public void OnThrow(Vector3 direction, float force)
    {
        StopAllCoroutines();
        StartCoroutine(SwitchOffTrigger(0.5f));

        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        rb.AddForce(direction * force, ForceMode.Impulse);

        shouldSnap = true;
    }

    private IEnumerator SwitchOffTrigger(float after)
    {
        yield return new WaitForSeconds(after);

        col.isTrigger = false;
    }
}
