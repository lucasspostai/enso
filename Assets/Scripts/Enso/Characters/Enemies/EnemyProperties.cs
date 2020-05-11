using UnityEngine;

namespace Enso.Characters.Enemies
{
    public abstract class EnemyProperties : FighterProperties
    {
        [Header("Movement")]
        public float MoveSpeed;
        public float MoveSpeedWhileDefending;
        public float AccelerationTime;

        [Header("Simple Attack")]
        public Vector2 AttackRange;
    }
}
