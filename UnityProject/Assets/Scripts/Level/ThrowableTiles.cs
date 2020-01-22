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

    private bool shouldSnap;

    public int damage;

    public bool isTreasure;
    public GameObject gem;

    public float raycastDistance;

    private bool isPickedUp;

    private void Awake()
    {
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();

        OnDrop();
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.identity;

        int layerMask = 1 << 8;

        if ( tag == "FallingTile" && Physics.Raycast(transform.position, -Vector3.up, raycastDistance, layerMask))
        {
                gameObject.GetComponent<Rigidbody>().useGravity = true;
        }
    }

    public void OnDrop()
    {
        col.isTrigger = false;
        rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        isPickedUp = false;
    }

    public void OnPickUp()
    {
        col.isTrigger = true;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        shouldSnap = true;
        isPickedUp = true;
    }

    public void OnThrow(Vector3 direction, float force)
    {
        StopAllCoroutines();
        StartCoroutine(SwitchOffTrigger(0.5f));

        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        rb.AddForce(direction * force, ForceMode.Impulse);

        shouldSnap = true;
        isPickedUp = false;
    }

    private IEnumerator SwitchOffTrigger(float after)
    {
        yield return new WaitForSeconds(after);

        col.isTrigger = false;
    }

    void OnCollisionEnter(Collision col)
    {
        if (tag == "SpikedTile" && col.gameObject.layer == LayerMask.NameToLayer("Player") && GetComponent<Rigidbody>().velocity.sqrMagnitude < 0.1f && isPickedUp == false)
        {
            col.gameObject.GetComponent<IDamagable>().Damage(new DamageInfo() { damage = this.damage });
        }

        else if (tag == "FallingTile" && col.gameObject.layer == LayerMask.NameToLayer("Player") && GetComponent<Rigidbody>().velocity.sqrMagnitude > 1.0f && isPickedUp == false)
        {
            col.gameObject.GetComponent<IDamagable>().Damage(new DamageInfo() { damage = this.damage });
        }

        else if (isTreasure == true && col.gameObject.layer != LayerMask.NameToLayer("Player") && GetComponent<Rigidbody>().velocity.sqrMagnitude > 0.1f)
        {
            Instantiate(gem, transform.position, Quaternion.identity);
            Instantiate(gem, transform.position, Quaternion.identity);
            Instantiate(gem, transform.position, Quaternion.identity);
            Instantiate(gem, transform.position, Quaternion.identity);
            Instantiate(gem, transform.position, Quaternion.identity);

            gameObject.SetActive(false);
        }
    }
}
