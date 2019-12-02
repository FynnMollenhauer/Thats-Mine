using UnityEngine;

[RequireComponent(typeof(EnemyStat))]
public abstract class EnemyBehavior : MonoBehaviour
{
    private EnemyStat stat = null;
    protected EnemyStat Stat
    {
        get
        {
            if (stat == null)
                stat = GetComponent<EnemyStat>();

            return stat;
        }
    }

    protected virtual void Start() { }
}
