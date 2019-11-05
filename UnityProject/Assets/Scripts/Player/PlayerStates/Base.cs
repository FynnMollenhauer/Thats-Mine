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