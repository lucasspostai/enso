using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "PlayerProperties", menuName = "Enso/Player")]
public class PlayerProperties : ScriptableObject
{
    [Header("Movement")]
    public float MoveSpeed;
    public float AccelerationTime;

    [Header("Simple Attack")] 
    public int MaxNumberOfAttacks;
    public float AttackCooldown;
    public Vector2 AttackRange;
}