using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyProperties", menuName = "Enso/Enemy")]
public class EnemyProperties : FighterProperties
{
    [Header("Movement")]
    public float MoveSpeed;
    public float MoveSpeedWhileDefending;
    public float AccelerationTime;

    [Header("Simple Attack")]
    public Vector2 AttackRange;
}
