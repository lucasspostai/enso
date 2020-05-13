using System;
using Framework;
using UnityEngine;

namespace Enso.Characters.Player
{
    [RequireComponent(typeof(Player))]
    public class PlayerMovement : CharacterMovement
    {
        private float currentSpeed;
        private Vector3 targetVelocity;
        private Vector3 currentVelocity;
        private Player player;

        [HideInInspector] public Vector3 Velocity;

        [SerializeField] private Transform HitboxAnchor;

        private void Start()
        {
            player = GetComponent<Player>();
            currentSpeed = player.GetProperties().MoveSpeed;
            
            SetCharacterCollisions(player.Collisions);

            SetDirection(Vector3.down);
        }

        private void Update()
        {
            if (player.GuardController.StartingGuard || player.GuardController.EndingGuard || player.DamageController.IsAnimationPlaying)
            {
                Velocity = Vector3.zero;
                return;
            }
            
            if (PlayerInput.Movement != Vector2.zero && player.AttackController.CanCutAnimation && !player.GuardController.IsAnimationPlaying)
            {
                SetDirection(PlayerInput.Movement);
            }
            
            if (player.AttackController.IsAnimationPlaying)
                return;

            //Check Collisions
            if (player.Collisions.Info.Above || player.Collisions.Info.Below)
                Velocity.y = 0;

            if (player.Collisions.Info.Left || player.Collisions.Info.Right)
                Velocity.x = 0;

            //Move
            targetVelocity = PlayerInput.Movement * currentSpeed;
            Velocity = Vector3.SmoothDamp(Velocity, targetVelocity, ref currentVelocity,
                player.GetProperties().AccelerationTime);
            
            Move(Velocity * Time.deltaTime);

            UpdateHitBoxAnchorRotation();
        }

        protected override void PlayMovementAnimation()
        {
            if (player.AttackController.IsAnimationPlaying || player.GuardController.IsAnimationPlaying || player.DamageController.IsAnimationPlaying)
                return;

            switch (PlayerInput.ActualMovementState)
            {
                case PlayerInput.MovementState.Idle:
                    player.Animator.Play(CharacterAnimations.IdleState);
                    break;
                case PlayerInput.MovementState.Walking:
                    player.Animator.Play(CharacterAnimations.WalkingState);
                    break;
                case PlayerInput.MovementState.Running:
                    player.Animator.Play(CharacterAnimations.RunningState);
                    break;
                case PlayerInput.MovementState.Sprinting:
                    player.Animator.Play(CharacterAnimations.SprintingState);
                    break;
                default:
                    player.Animator.Play(CharacterAnimations.IdleState);
                    break;
            }
        }

        public void SetDirection(Vector3 direction)
        {
            player.Animator.SetFloat(CharacterAnimations.FaceX, direction.x);
            player.Animator.SetFloat(CharacterAnimations.FaceY, direction.y);
                
            CurrentDirection = direction;
        }

        public void SetSpeed(float speed)
        {
            currentSpeed = speed;
        }

        private void UpdateHitBoxAnchorRotation()
        {
            float angle = Mathf.Atan2(Velocity.y, Velocity.x) * Mathf.Rad2Deg;
            HitboxAnchor.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}