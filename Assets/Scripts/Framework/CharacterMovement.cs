using System;
using UnityEngine;

namespace Framework
{
    public abstract class CharacterMovement : MonoBehaviour
    {
        protected float MovementDeadZone = 0f;
        protected CharacterCollisions Collisions;
        
        [HideInInspector] public Vector3 CurrentDirection;

        protected void SetCharacterCollisions(CharacterCollisions collisions)
        {
            Collisions = collisions;
        }
        
        public void Move(Vector2 moveAmount)
        {
            Collisions.UpdateRaycastOrigins();
            Collisions.Info.Reset();

            if (Math.Abs(moveAmount.x) > MovementDeadZone)
                Collisions.GetHorizontalCollisions(ref moveAmount);

            if (Math.Abs(moveAmount.y) > MovementDeadZone)
                Collisions.GetVerticalCollisions(ref moveAmount);

            transform.Translate(moveAmount);

            PlayMovementAnimation();
        }

        protected virtual void PlayMovementAnimation()
        {
            
        }
    }
}
