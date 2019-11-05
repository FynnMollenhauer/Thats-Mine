﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player attributes")]
    public float movementSpeed = 5;
    public float jumpHeight = 5;
    public float rotateRate = 30;

    [Header("Components")]
    public Animator animator;
    public Rigidbody body;

    private PlayerState movementState;
    private PlayerState upperBodyState;

    private Throwable throwable;

    // Start is called before the first frame update
    private void Start()
    {
        ChangeMovementState(PlayerState.GetStateObject<IdleState>());
        ChangeUpperBodyState(PlayerState.GetStateObject<UnequipedState>());
    }

    private void Update()
    {
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

        transform.position += forward * movementSpeed * Time.deltaTime;
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
    public void Pickup(Throwable throwable)
    {
    }

    public void Throw()
    {
    }
    #endregion
}

