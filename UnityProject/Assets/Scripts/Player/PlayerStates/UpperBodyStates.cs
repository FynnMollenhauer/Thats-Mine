using UnityEngine;

public class UnequipedState : PlayerState
{
    public override void OnExit(PlayerController player)
    {
    }

    public override void Update(PlayerController player)
    {
        if (Input.GetButton("Use"))
        {
            player.Pickup();
        }
    }
}