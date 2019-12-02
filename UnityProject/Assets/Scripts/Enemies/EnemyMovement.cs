using UnityEngine;
using System.Collections;

public class EnemyMovement : EnemyBehavior
{
    [Header("Movement stats")]
    [SerializeField] float moveMagnitude;
    [SerializeField] float movementSpeed;

    float startPositionX;

    protected override void Start()
    {
        base.Start();

        startPositionX = transform.position.x;

        StartCoroutine(Oscillate());
    }

    private IEnumerator Oscillate()
    {
        while (true)
        {
            Vector3 previousPos = transform.position;
            transform.position = new Vector3(startPositionX + (Mathf.Cos(Time.time * movementSpeed) * moveMagnitude), transform.position.y, 0) ;
            transform.forward = (transform.position - previousPos).normalized;
            yield return new WaitForEndOfFrame();
        }
    }
}

