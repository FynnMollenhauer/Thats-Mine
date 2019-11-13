﻿using UnityEngine;

public class IdleState : PlayerState
{
    public override void Update(PlayerController player)
    {
        if (InputHelper.IsMoving())
        {
            player.ChangeMovementState(GetStateObject<WalkState>());
            return;
        }

        if (InputHelper.JumpPressed())
        {
            player.ChangeMovementState(GetStateObject<JumpState>());
            return;
        }
    }

    public override void OnEnter(PlayerController player)
    {
        base.OnEnter(player);

        player.animator.SetFloat("Forward", 0);
        player.animator.SetBool("IsJumping", false);
    }

    public override void OnExit(PlayerController player)
    {
    }
}



public class WalkState : PlayerState
{
    public override void Update(PlayerController player)
    {
        if (!InputHelper.IsMoving())
        {
            player.ChangeMovementState(GetStateObject<IdleState>());
            return;
        }

        if (InputHelper.JumpPressed())
        {
            player.ChangeMovementState(GetStateObject<JumpState>());
            return;
        }

        player.MovePlayer(InputHelper.GetMovement());
    }


    public override void OnEnter(PlayerController player)
    {
        base.OnEnter(player);

        player.animator.SetFloat("Forward", 1);
    }

    public override void OnExit(PlayerController player)
    {
    }
}



public class JumpState : PlayerState
{
    public override void Update(PlayerController player)
    {
        player.MovePlayer(InputHelper.GetMovement());

        if (player.body.velocity.sqrMagnitude < 0.05)
        {
            player.ChangeMovementState(GetStateObject<IdleState>());
            return;
        }
    }

    public override void OnEnter(PlayerController player)
    {
        base.OnEnter(player);

        player.animator.SetBool("IsJumping", true);

        player.body.velocity = Vector3.zero;
        player.body.AddForce(Vector3.up * player.jumpHeight, ForceMode.Impulse);
    }

    public override void OnExit(PlayerController player)
    {
        player.animator.SetBool("IsJumping", false);
        player.body.velocity = Vector3.zero;
    }
}