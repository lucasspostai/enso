using System;
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
            private set
            {
                distanceToTarget = value;

                OnUpdateDistanceToTargetValue();
            }
        }

        [HideInInspector] public bool MustMove;

        public event Action UpdateDistanceToTargetValue;  

        [SerializeField] private float AcceptanceRadius = 2f;
        
        protected override void Update()
        {
            if (!ThisFighter.Target || ThisFighter.AnimationHandler.IsAnyCustomAnimationPlaying())
            {
                SetMovement(Vector3.zero);
                return;
            }

            SetMovementDirectionAndDistance();

            if (distanceToTarget < AcceptanceRadius || !MustMove)
                SetMovement(Vector3.zero);
            else
                SetMovement(movementDirection);
            
            if(ThisFighter.AnimationHandler.IsAnyGuardAnimationPlaying())
                SetDirection(movementDirection);

            base.Update();
        }

        private void SetMovementDirectionAndDistance()
        {
            movementDirection = ThisFighter.Target.position - transform.position;
            DistanceToTarget = movementDirection.magnitude;
            
            movementDirection.Normalize();
        }

        private void OnUpdateDistanceToTargetValue()
        {
            UpdateDistanceToTargetValue?.Invoke();
        }
    }
}
