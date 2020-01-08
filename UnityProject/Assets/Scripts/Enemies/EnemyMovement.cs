using UnityEngine;
using System.Collections;

public class EnemyMovement : EnemyBehavior
{
    [Header("Movement stats")]
    [SerializeField] float moveMagnitude;
    [SerializeField] float movementSpeed;
    [SerializeField] bool doesMoveOnCeiling;
    [SerializeField] float timeOnFloorOrCeiling;

    float startPositionX;
    bool isOnCeiling;
    float ceilingHeight;
    float floorHeight;

    protected override void Start()
    {
        base.Start();

        startPositionX = transform.position.x;
        floorHeight = transform.position.y + 0.2f;

        StartCoroutine(Oscillate());
        if (doesMoveOnCeiling)
        {
            isOnCeiling = false;

            // Get ceiling height
            RaycastHit raycastHit;
            if (Physics.Raycast(transform.position + Vector3.up, Vector3.up, out raycastHit, 100f, LayerMask.GetMask("Default", "Throwable")))
            {
                ceilingHeight = raycastHit.point.y - 0.5f;
            }
            else
            {
                ceilingHeight = floorHeight + 10f;
            }

            StartCoroutine(SwitchToCeiling());
        }
    }

    private IEnumerator SwitchToCeiling()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeOnFloorOrCeiling);

            isOnCeiling = !isOnCeiling;

            GetComponent<EnemyIntervalFiring>().shootHorizontally = !isOnCeiling;
        }
    }

    private IEnumerator Oscillate()
    {
        while (true)
        {
            Vector3 previousPos = transform.position;
            transform.position = new Vector3(startPositionX + (Mathf.Cos(Time.time * movementSpeed) * moveMagnitude), 
                isOnCeiling ? ceilingHeight : floorHeight, 0) ;
            transform.forward = (transform.position - previousPos).normalized;
            yield return new WaitForEndOfFrame();
        }
    }
}

