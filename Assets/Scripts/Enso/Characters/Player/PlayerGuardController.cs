using System;
using Enso.CombatSystem;
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
                PlayGuardAnimation(Animations.ForwardGuardWalkAnimationClipHolder);
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
            
            if(player)
                player.Movement.SetSpeed(player.GetProperties().MoveSpeed);
        }
    }
}
