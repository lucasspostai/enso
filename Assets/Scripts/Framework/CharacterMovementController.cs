using System;
using Enso.Characters;
using Enso.CombatSystem;
using UnityEngine;

namespace Framework
{
    public abstract class CharacterMovementController : CustomAnimationController
    {
        private bool isHitboxAnchorNull;
        private bool sprint;
        private float currentSpeed;
        private Vector2 movementAmount;
        private Vector3 currentVelocity;
        private Vector3 targetVelocity;

        private const float MovementDeadZone = 0f;

        [HideInInspector] public Vector3 Velocity;

        [SerializeField] private Transform HitboxAnchor;

        public LocomotionAnimations Animations;

        protected override void Awake()
        {
            base.Awake();
            
            isHitboxAnchorNull = HitboxAnchor == null;
        }

        protected override void Start()
        {
            base.Start();

            ThisFighter.AnimationHandler.SetFacingDirection(Vector3.down);

            SetRegularRunSpeed();

            currentVelocity = Vector3.zero;
        }

        protected override void Update()
        {
            base.Update();

            UpdateHitBoxAnchorRotation();
            
            if (ThisFighter.AnimationHandler.IsAnyCustomAnimationPlaying())
            {
                Velocity = Vector3.zero;
                return;
            }

            if (movementAmount != Vector2.zero && !ThisFighter.AnimationHandler.IsAnyGuardAnimationPlaying())
            {
                SetDirection(movementAmount);
            }

            //Check Collisions
            if (ThisFighter.Collisions.Info.Above || ThisFighter.Collisions.Info.Below)
                Velocity.y = 0;

            if (ThisFighter.Collisions.Info.Left || ThisFighter.Collisions.Info.Right)
                Velocity.x = 0;

            //Move
            targetVelocity = movementAmount * currentSpeed;
            Velocity = Vector3.SmoothDamp(Velocity, targetVelocity, ref currentVelocity,
                ThisFighter.GetBaseProperties().AccelerationTime);

            Move(Velocity * Time.deltaTime);
        }

        private void UpdateHitBoxAnchorRotation()
        {
            if (isHitboxAnchorNull)
                return;
            
            float angle = Mathf.Atan2(ThisFighter.AnimationHandler.CurrentDirection.y,
                ThisFighter.AnimationHandler.CurrentDirection.x) * Mathf.Rad2Deg;

            HitboxAnchor.rotation = Quaternion.AngleAxis(angle, transform.forward);
        }

        protected void SetMovement(Vector2 movement)
        {
            movementAmount = movement;
        }

        public void SetFighter(Fighter fighter)
        {
            ThisFighter = fighter;
        }

        protected void SetSprintSpeed()
        {
            if (ThisFighter.AnimationHandler.IsAnyGuardAnimationPlaying())
                return;

            currentSpeed = ThisFighter.GetBaseProperties().SprintSpeed;
        }

        protected void SetRegularRunSpeed()
        {
            if (ThisFighter.AnimationHandler.IsAnyGuardAnimationPlaying())
                return;

            currentSpeed = ThisFighter.GetBaseProperties().RunSpeed;
        }

        public void Move(Vector2 moveAmount)
        {
            ThisFighter.Collisions.UpdateRaycastOrigins();
            ThisFighter.Collisions.Info.Reset();

            if (Math.Abs(moveAmount.x) > MovementDeadZone)
                ThisFighter.Collisions.GetHorizontalCollisions(ref moveAmount);

            if (Math.Abs(moveAmount.y) > MovementDeadZone)
                ThisFighter.Collisions.GetVerticalCollisions(ref moveAmount);

            transform.Translate(moveAmount);

            PlayMovementAnimation();
        }

        private void PlayMovementAnimation()
        {
            if (ThisFighter.AnimationHandler.IsAnyCustomAnimationPlaying() ||
                ThisFighter.AnimationHandler.IsAnyGuardAnimationPlaying())
                return;

            if (Velocity != Vector3.zero)
            {
                ThisFighter.AnimationHandler.Play(this, currentSpeed >= ThisFighter.GetBaseProperties().SprintSpeed
                    ? Animations.SprintAnimationClipHolder.AnimatorStateName
                    : Animations.RunAnimationClipHolder.AnimatorStateName);
            }
            else
            {
                ThisFighter.AnimationHandler.Play(this, Animations.IdleAnimationClipHolder.AnimatorStateName);
            }
        }

        protected void SetDirection(Vector3 direction)
        {
            ThisFighter.AnimationHandler.SetFacingDirection(direction);
        }

        public void SetSpeed(float speed)
        {
            currentSpeed = speed;
        }
    }
}