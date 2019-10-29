using UnityEngine;
using System.Collections;
public interface IPlayerState
{
    void OnEnter(PlayerController player);
    void OnExit(PlayerController player);
    void Update(PlayerController player);
}

public abstract class PlayerState : IPlayerState
{
    public virtual void OnEnter(PlayerController player)
    {
#if STATE_MACHINE_DEBUG
        Debug.Log("[PlayerState] OnEnter " + this.ToString());
#endif
    }

    public abstract void OnExit(PlayerController player);
    public abstract void Update(PlayerController player);


    public static T GetStateObject<T>() where T : PlayerState, new()
    {
        return new T();
    }
}


public class IdleState : PlayerState
{
    public override void Update(PlayerController player)
    {
        if (InputHelper.IsMoving())
        {
            player.ChangeState(GetStateObject<WalkState>());
            return;
        }

        if (InputHelper.JumpPressed())
        {
            player.ChangeState(GetStateObject<JumpState>());
            return;
        }
    }

    public override void OnEnter(PlayerController player)
    {
        base.OnEnter(player);

        player.animator.SetBool("IsMoving", false);
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
            player.ChangeState(GetStateObject<IdleState>());
            return;
        }

        if (InputHelper.JumpPressed())
        {
            player.ChangeState(GetStateObject<JumpState>());
            return;
        }

        player.MovePlayer(InputHelper.GetMovement());
    }


    public override void OnEnter(PlayerController player)
    {
        base.OnEnter(player);

        player.animator.SetBool("IsMoving", true);
    }

    public override void OnExit(PlayerController player)
    {
        player.animator.SetBool("IsMoving", false);
    }
}



public class JumpState : PlayerState
{
    public override void Update(PlayerController player)
    {
        player.MovePlayer(InputHelper.GetMovement());

        if (player.body.velocity.sqrMagnitude == 0)
        {
            player.ChangeState(GetStateObject<IdleState>());
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