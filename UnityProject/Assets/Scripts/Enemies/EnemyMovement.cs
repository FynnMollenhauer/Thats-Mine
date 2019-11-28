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
            transform.position = new Vector3(Mathf.Cos(Time.time * movementSpeed) * moveMagnitude, transform.position.y, 0) ;
            yield return new WaitForEndOfFrame();
        }
    }
}

