using UnityEngine;

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

        if (!player.animator.GetBool("IsJumping") && player.body.velocity.y < -0.15f)
        {
            player.ChangeMovementState(GetStateObject<FallState>());
            return;
        }
    }

    public override void OnEnter(PlayerController player)
    {
        base.OnEnter(player);

        player.animator.SetFloat("Forward", 0);
        player.animator.SetBool("IsJumping", false);
        player.animator.SetBool("IsFalling", false);
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

        if (player.body.velocity.magnitude < 0.05f)
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



public class FallState : PlayerState
{
    public override void Update(PlayerController player)
    {
        
        if (player.body.velocity.magnitude < 0.05f)
        {
            player.ChangeMovementState(GetStateObject<IdleState>());
            return;
        }
        player.MovePlayer(InputHelper.GetMovement());
    }

    public override void OnEnter(PlayerController player)
    {
        base.OnEnter(player);

        player.animator.SetBool("IsFalling", true);
    }

    public override void OnExit(PlayerController player)
    {
        player.animator.SetBool("IsFalling", false);
    }
}