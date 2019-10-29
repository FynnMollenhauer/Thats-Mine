using UnityEngine;

public static class InputHelper
{
    public static float GetMovement()
    {
        return Input.GetAxis("Horizontal");
    }

    public static bool IsMoving()
    {
        return GetMovement() != 0.0f;
    }

    public static bool JumpPressed()
    {
        return Input.GetAxis("Jump") > 0;
    }
}

