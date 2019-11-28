using UnityEngine;
using System.Collections;

public class EnemyMovement : EnemyBehavior
{
    [Header("Movement stats")]
    [SerializeField] float moveMagnitude;
    [SerializeField] float movementSpeed;

    protected override void Start()
    {
        base.Start();

        StartCoroutine(Oscillate());
    }

    private IEnumerator Oscillate()
    {
        while (true)
        {
            Vector3 previousPos = transform.position;
            transform.position = new Vector3(Mathf.Cos(Time.time * movementSpeed) * moveMagnitude, transform.position.y, 0) ;
            transform.forward = (transform.position - previousPos).normalized;
            yield return new WaitForEndOfFrame();
        }
    }
}

