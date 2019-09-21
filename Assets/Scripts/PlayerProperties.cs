using UnityEngine;

[CreateAssetMenu(fileName = "PlayerProperties", menuName = "Enso/Player")]
public class PlayerProperties : ScriptableObject
{
    [Header("Movement")]
    public float MoveSpeed;
    public float AccelerationTime;
}