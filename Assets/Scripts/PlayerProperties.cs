using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "PlayerProperties", menuName = "Enso/Player")]
public class PlayerProperties : ScriptableObject
{
    [Header("Movement")]
    public float MoveSpeed;
    public float AccelerationTime;

    [Header("Dodge Roll")] 
    public float SlidingSpeed;
    public float SlidingLength;

    [Header("Simple Attack")]
    public Vector2 AttackRange;
}