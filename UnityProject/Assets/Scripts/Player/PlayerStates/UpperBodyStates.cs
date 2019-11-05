using UnityEngine;

public class UnequipedState : PlayerState
{
    public override void OnExit(PlayerController player)
    {
    }

    public override void Update(PlayerController player)
    {
        if (Input.GetButtonDown("Use"))
        {
            bool pickup = player.Pickup();
            if (pickup)
            {
                player.ChangeUpperBodyState(PlayerState.GetStateObject<PickupState>());
            }
        }
    }
}

public class PickupState : PlayerState
{
    public override void OnExit(PlayerController player)
    {
    }

    public override void Update(PlayerController player)
    {
        if (Input.GetButtonDown("Use"))
        {
            player.Throw();
            player.ChangeUpperBodyState(PlayerState.GetStateObject<UnequipedState>());
        }
    }
}