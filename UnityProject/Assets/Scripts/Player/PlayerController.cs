﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player attributes")]
    public float movementSpeed = 4.0f;
    public float jumpHeight = 2.5f;
    public float rotateRate = 450.0f;
    public float throwableDetectionRadius = 1.0f;
    public float throwingForce = 8.5f;
    [Range(0, 60)] public float throwingAngle = 30.0f;
    [Range(0, 1)] public float wallStoppingDistance = 0.5f;

    [Header("Joints")]
    [SerializeField] private Transform throwingHandJoint;

    [Header("Components")]
    public Animator animator;
    public Rigidbody body;

    public GameObject marker;

    private PlayerState movementState;
    private PlayerState upperBodyState;

    private GameObject nearbyThrowable;
    private GameObject holdingThrowable;

    private int wallLayer;

    private void Awake()
    {
        wallLayer = LayerMask.GetMask("Default", "Throwable");
    }

    // Start is called before the first frame update
    private void Start()
    {
        ChangeMovementState(PlayerState.GetStateObject<IdleState>());
        ChangeUpperBodyState(PlayerState.GetStateObject<UnequipedState>());

        marker.transform.SetParent(null);
    }

    private void Update()
    {
        DetectThrowable();
        UpdateMarker();

        movementState.Update(this);
        upperBodyState.Update(this);
    }

    #region Movements
    public void ChangeMovementState(PlayerState state)
    {
        ChangeState(ref movementState, state);
    }

    public void ChangeUpperBodyState(PlayerState state)
    {
        ChangeState(ref upperBodyState, state);
    }

    private void ChangeState(ref PlayerState stateObject, PlayerState newState)
    {
        if (newState == null)
            return;

        if (stateObject != null)
            stateObject.OnExit(this);

        stateObject = newState;
        stateObject.OnEnter(this);
    }

    public void MovePlayer(float movement)
    {
        Vector3 forward = new Vector3(movement, 0);
        forward = forward.normalized;

        // If not facing the desired direction
        if (Vector3.Dot(forward, transform.forward) < 0)
        {
            if (rotateCoroutine != null)
            {
                StopCoroutine(rotateCoroutine);
                rotateCoroutine = null;
            }

            rotateCoroutine = RotateTo(forward, () => { rotateCoroutine = null; });
            StartCoroutine(rotateCoroutine);
        }

        // Check collision in the desired direction and stop if needed
        // to prevent weird physics problems
        if (!Physics.Raycast(transform.position + 0 * Vector3.up, forward, wallStoppingDistance, wallLayer) &&
            !Physics.Raycast(transform.position + 1 * Vector3.up, forward, wallStoppingDistance, wallLayer) &&
            !Physics.Raycast(transform.position + 2 * Vector3.up, forward, wallStoppingDistance, wallLayer))
            transform.position += forward * movementSpeed * Time.deltaTime;
    }

    private bool IsFacingRight
    {
        get
        {
            return Vector3.Dot(Vector3.right, transform.forward) > 0;
        }
    }

    IEnumerator rotateCoroutine = null;
    private IEnumerator RotateTo(Vector3 forward, System.Action OnFinish)
    {
        while (Vector3.Dot(forward, transform.forward) < 0.9f)
        {
            transform.Rotate(Vector3.up, rotateRate * Time.deltaTime);
            yield return null;
        }

        transform.forward = forward;
        OnFinish();
    }
    #endregion

    #region Pickup and throw
    private void DetectThrowable()
    {
        // Already holding something
        if (holdingThrowable != null)
        {
            return;
        }

        Collider[] cols = Physics.OverlapSphere(transform.position, throwableDetectionRadius, LayerMask.GetMask("Throwable"));
        if (cols.Length > 0)
        {
            nearbyThrowable = cols[0].gameObject;
        }
        else
        {
            nearbyThrowable = null;
        }
    }

    public bool Pickup()
    {
        if (nearbyThrowable == null)
        {
            return false;
        }

        holdingThrowable = nearbyThrowable;

        holdingThrowable.transform.SetParent(throwingHandJoint);
        holdingThrowable.transform.localPosition = Vector3.zero;
        holdingThrowable.GetComponent<IThrowableTile>().OnPickUp();

        return true;
    }

    public void Throw()
    {
        if (holdingThrowable != null)
        {
            holdingThrowable.transform.SetParent(null);

            Vector3 facingDirection = Vector3.right;
            Vector3 rightDirection = Vector3.forward;
            if (!IsFacingRight)
            {
                facingDirection = -Vector3.right;
                rightDirection = -Vector3.forward;
            }

            Vector3 throwingDirection = Quaternion.AngleAxis(throwingAngle, rightDirection) * facingDirection;

            holdingThrowable.GetComponent<IThrowableTile>().OnThrow(throwingDirection, throwingForce);

            holdingThrowable = null;
        }
    }

    public void Drop()
    {
        if (holdingThrowable != null)
        {
            holdingThrowable.transform.SetParent(null);

            holdingThrowable.transform.position = new Vector3(marker.transform.position.x, holdingThrowable.transform.position.y);

            holdingThrowable.GetComponent<IThrowableTile>().OnDrop();

            holdingThrowable = null;
        }
    }
    #endregion

    void UpdateMarker()
    {
        if (holdingThrowable == null)
        {
            marker.SetActive(false);
            return;
        }

        marker.SetActive(true);

        Vector3 position = transform.position;
        if (IsFacingRight)
        {
            position.x = Mathf.Ceil(position.x) + 0.5f;
        }
        else
        {
            position.x = Mathf.Floor(position.x) - 0.5f;
        }
        marker.transform.position = position;
    }
}

