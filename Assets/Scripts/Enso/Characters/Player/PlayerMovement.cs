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

        private void Start()
        {
            player = GetComponent<Player>();
            currentSpeed = player.GetProperties().MoveSpeed;
        }

        // private void OnEnable()
        // {
        //     PlayerInput.DefenseInputDown += StartDefending;
        //     PlayerInput.DefenseInputUp += StopDefending;
        // }
        //
        // private void OnDisable()
        // {
        //     PlayerInput.DefenseInputDown -= StartDefending;
        //     PlayerInput.DefenseInputUp -= StopDefending;
        // }

        private void Update()
        {
            if (PlayerInput.Movement != Vector2.zero)
            {
                player.Animator.SetFloat(CharacterAnimations.FaceX, PlayerInput.Movement.x);
                player.Animator.SetFloat(CharacterAnimations.FaceY, PlayerInput.Movement.y);
            }
        
            //Check Collisions
            if (player.Collisions.Info.Above || player.Collisions.Info.Below)
                Velocity.y = 0;

            if (player.Collisions.Info.Left || player.Collisions.Info.Right)
                Velocity.x = 0;

            // if (player.DodgeRoll.ActualRollState == RollState.Sliding)
            //     return;
        
            //Move
            targetVelocity = PlayerInput.Movement * currentSpeed;
            Velocity = Vector3.SmoothDamp(Velocity, targetVelocity, ref currentVelocity, player.GetProperties().AccelerationTime);
            Move(Velocity * Time.deltaTime);
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

            // if (Attack.IsPerformingSimpleAttack || Attack.IsPerformingHeavyAttack || DodgeRoll.ActualRollState == RollState.Sliding || Defense.IsDefending)
            //     return;

            PlayMovementAnimation();
        }

        private void PlayMovementAnimation()
        {
            switch(PlayerInput.ActualMovementState)
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

        // private void StartDefending()
        // {
        //     currentSpeed = player.Properties.MoveSpeedWhileDefending;
        // }
        //
        // private void StopDefending()
        // {
        //     currentSpeed = player.Properties.MoveSpeed;
        // }
    }
}