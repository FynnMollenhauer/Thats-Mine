using System.Collections;
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

    private PlayerState currentState;

    // Start is called before the first frame update
    private void Start()
    {
        ChangeState(PlayerState.GetStateObject<IdleState>());

    }

    private void Update()
    {
        currentState.Update(this);
    }

    public void ChangeState(PlayerState state)
    {
        if (state == null)
            return;

        if (currentState != null)
            currentState.OnExit(this);

        currentState = state;
        currentState.OnEnter(this);
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

        //if (movement != 0)
        //    spriteRenderer.flipX = movement < 0;
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
}
