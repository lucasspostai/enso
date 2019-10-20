using UnityEngine;

[CreateAssetMenu(fileName = "PlayerProperties", menuName = "Enso/Player")]
public class PlayerProperties : FighterProperties
{
    [Header("Movement")]
    public float MoveSpeed;
    public float MoveSpeedWhileDefending;
    public float AccelerationTime;

    [Header("Dodge Roll")] 
    public float SlidingSpeed;
    public float SlidingLength;

    [Header("Simple Attack")]
    public Vector2 AttackRange;
    public int Damage;
}