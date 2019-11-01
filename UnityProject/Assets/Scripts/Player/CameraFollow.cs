using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField, Range(0, 1)] float safeHorizontalZone = 0.5f;
    [SerializeField, Range(0, 1)] float safeVerticalZone = 0.5f;
    [SerializeField] float followRate = 1;
    [SerializeField] float slowDownRate = 1f;

    private Camera camera;

    private float minViewportX;
    private float maxViewportX;

    private float minViewportY;
    private float maxViewportY;

    private Vector3 lastMovement;
    private float lastVelocity;

    private void Start()
    {
        camera = GetComponent<Camera>();

        maxViewportX = 0.5f + safeHorizontalZone * 0.5f;
        minViewportX = 0.5f - safeHorizontalZone * 0.5f;

        maxViewportY = 0.5f + safeVerticalZone * 0.5f;
        minViewportY = 0.5f - safeVerticalZone * 0.5f;
    }

    // Update is called once per frame
    private void Update()
    {
        if (target == null)
        {
            // My job here is done....
            enabled = false;

            return;
        }

        Vector3 targetViewportPosition = camera.WorldToViewportPoint(target.position);

        float camMovementX = GetMovement(minViewportX, maxViewportX, targetViewportPosition.x);
        float camMovementY = GetMovement(minViewportY, maxViewportY, targetViewportPosition.y);

        Vector3 camMovement = new Vector3(camMovementX, camMovementY);
        if (camMovement.sqrMagnitude > 0)
        {
            transform.position += camMovement.normalized * followRate * Time.deltaTime;

            lastVelocity = followRate;
            lastMovement = camMovement;
        }
        else
        {
            if (lastVelocity > 0)
            {
                lastVelocity -= slowDownRate * Time.deltaTime;
                if (lastVelocity <= 0)
                {
                    lastVelocity = 0;
                    lastMovement = Vector3.zero;
                }
                else
                {
                    transform.position += lastMovement.normalized * lastVelocity * Time.deltaTime;
                }
            }
        }

    }

    private float GetMovement(float min, float max, float val)
    {
        if (val < min)
            return val - min;

        if (val > max)
            return val - max;

        // within range, returns 0
        return 0;
    }
}
