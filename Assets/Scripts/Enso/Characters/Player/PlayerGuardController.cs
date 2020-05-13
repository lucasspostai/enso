using System;
using Enso.CombatSystem;
using Framework.Utils;
using UnityEngine;

namespace Enso.Characters.Player
{
    [RequireComponent(typeof(Player))]
    public class PlayerGuardController : GuardController
    {
        private Player player;

        #region Delegates

        private void OnEnable()
        {
            PlayerInput.GuardInputDown += StartGuard;
            PlayerInput.GuardInputUp += EndGuard;
        }

        private void OnDisable()
        {
            PlayerInput.GuardInputDown -= StartGuard;
            PlayerInput.GuardInputUp -= EndGuard;
        }

        #endregion

        protected override void Start()
        {
            base.Start();

            player = GetComponent<Player>();
        }

        protected override void PlayMovementAnimation()
        {
            base.PlayMovementAnimation();

            if (player.Movement.Velocity == Vector3.zero)
            {
                PlayGuardAnimation(Animations.GuardIdleAnimationClipHolder);
            }
            else
            {
                var magnitude = VectorExtender.GetMagnitudeFromVectorDifference(
                    player.Movement.CurrentDirection,
                    PlayerInput.Movement
                );

                if (magnitude <= 0.5f)
                    PlayGuardAnimation(Animations.ForwardGuardWalkAnimationClipHolder, true);
                else if (magnitude >= 1.5f)
                    PlayGuardAnimation(Animations.BackwardGuardWalkAnimationClipHolder, true);
                else
                {
                    PlayGuardAnimation(
                        VectorExtender.InputIsLeft(player.Movement.CurrentDirection, PlayerInput.Movement)
                            ? Animations.LeftGuardWalkAnimationClipHolder
                            : Animations.RightGuardWalkAnimationClipHolder,
                        true
                    );
                }
            }
        }

        protected override void StartGuard()
        {
            base.StartGuard();

            player.Movement.SetSpeed(player.GetProperties().MoveSpeedWhileDefending);
        }

        protected override void ResetAllProperties()
        {
            base.ResetAllProperties();

            if (player)
                player.Movement.SetSpeed(player.GetProperties().MoveSpeed);
        }
    }
}