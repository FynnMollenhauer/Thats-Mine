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

    private static PlayerState[] precachedStates = new PlayerState[]
    {
        new IdleState(),
        new WalkState(),
        new JumpState(),
        new FallState(),

        new UnequipedState(),
        new PickupState()
    };

    public static T GetStateObject<T>() where T : PlayerState, new()
    {
        foreach (var s in precachedStates)
        {
            if (s is T)
                return (T)s;
        }

        throw new System.NullReferenceException("No cached object of type " + typeof(T));
    }
}