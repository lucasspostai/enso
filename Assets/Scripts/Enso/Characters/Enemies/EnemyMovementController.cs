using Framework;
using UnityEngine;

namespace Enso.Characters.Enemies
{
    public class EnemyMovementController : CharacterMovementController
    {
        private Vector3 movementDirection;
        private float distanceToTarget;

        public float DistanceToTarget
        {
            get => distanceToTarget;
            private set => distanceToTarget = value;
        }

        [SerializeField] private float AcceptanceRadius = 2f;
        
        protected override void Start()
        {
            base.Start();
            
            //TEMP
            ThisFighter.EnterCombatWith(FindObjectOfType<Player.Player>());
        }
        
        protected override void Update()
        {
            SetMovementDirectionAndDistance();

            SetMovement(distanceToTarget > AcceptanceRadius ? movementDirection : Vector3.zero);

            base.Update();
        }

        private void SetMovementDirectionAndDistance()
        {
            movementDirection = ThisFighter.Target.position - transform.position;
            DistanceToTarget = movementDirection.magnitude;
            
            movementDirection.Normalize();
        }
    }
}
