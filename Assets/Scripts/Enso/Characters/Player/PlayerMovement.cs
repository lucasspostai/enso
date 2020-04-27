using System;
using UnityEngine;

namespace Enso.Characters.Player
{
    [RequireComponent(typeof(Player))]
    public class PlayerMovement : MonoBehaviour
    {
        private float currentSpeed;
        private Vector3 targetVelocity;
        private Vector3 currentVelocity;
        private Player player;

        [HideInInspector] public Vector3 Velocity;
        [HideInInspector] public Vector3 CurrentDirection;

        [SerializeField] private Transform HitboxAnchor;

        private void Start()
        {
            player = GetComponent<Player>();
            currentSpeed = player.GetProperties().MoveSpeed;

            SetDirection(Vector3.down);
        }

        private void Update()
        {
            if (PlayerInput.Movement != Vector2.zero && player.AttackController.CanCutAnimation)
            {
                SetDirection(PlayerInput.Movement);
            }
            
            if (player.AttackController.IsAttackAnimationPlaying)
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

        public void Move(Vector2 moveAmount)
        {
            player.Collisions.UpdateRaycastOrigins();
            player.Collisions.Info.Reset();

            if (Math.Abs(moveAmount.x) > player.GetProperties().DeadZone)
                player.Collisions.GetHorizontalCollisions(ref moveAmount);

            if (Math.Abs(moveAmount.y) > player.GetProperties().DeadZone)
                player.Collisions.GetVerticalCollisions(ref moveAmount);

            transform.Translate(moveAmount);

            PlayMovementAnimation();
        }

        private void PlayMovementAnimation()
        {
            if (player.AttackController.IsAttackAnimationPlaying)
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

        private void SetDirection(Vector3 direction)
        {
            player.Animator.SetFloat(CharacterAnimations.FaceX, direction.x);
            player.Animator.SetFloat(CharacterAnimations.FaceY, direction.y);
                
            CurrentDirection = direction;
        }

        private void UpdateHitBoxAnchorRotation()
        {
            float angle = Mathf.Atan2(Velocity.y, Velocity.x) * Mathf.Rad2Deg;
            HitboxAnchor.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}