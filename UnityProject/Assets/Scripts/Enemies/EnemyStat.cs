using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] float movementSpeed;
    [SerializeField] float damage;

    public float Health { get { return health; } }
    public float MovementSpeed { get { return movementSpeed; } }
    public float Damage { get { return damage; } }
}
