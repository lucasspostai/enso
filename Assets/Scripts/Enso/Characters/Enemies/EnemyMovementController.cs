using Framework;
using UnityEngine;

namespace Enso.Characters.Enemies
{
    public class EnemyMovementController : CharacterMovementController
    {
        private float distanceToTarget;
        private Vector3 movementAmount;

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

            SetMovement(distanceToTarget > AcceptanceRadius ? movementAmount : Vector3.zero);

            base.Update();

            //UpdateHitBoxAnchorRotation();
        }

        private void SetMovementDirectionAndDistance()
        {
            movementAmount = ThisFighter.Target.position - transform.position;
            distanceToTarget = movementAmount.magnitude;
            
            movementAmount.Normalize();
        }
    }
}
