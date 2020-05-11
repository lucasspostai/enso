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
            
            Debug.Log((new Vector2(0,1) - new Vector2(0,-1)).magnitude);
            Debug.Log((new Vector2(0,1) - new Vector2(1,0)));
            Debug.Log((new Vector2(0,1) - new Vector2(-1,0)));
            Debug.Log((new Vector2(0,1) - new Vector2(0,1)).magnitude);
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
                float magnitude =
                    GetMagnitudeFromVectorDifference(player.Movement.CurrentDirection, PlayerInput.Movement);
                
                if(magnitude <= 0.5f)
                    PlayGuardAnimation(Animations.ForwardGuardWalkAnimationClipHolder, true);
                else if (magnitude >= 1.5f)
                    PlayGuardAnimation(Animations.BackwardGuardWalkAnimationClipHolder, true);
                else
                {
                    PlayGuardAnimation(InputIsLeft(player.Movement.CurrentDirection, PlayerInput.Movement)
                        ? Animations.LeftGuardWalkAnimationClipHolder
                        : Animations.RightGuardWalkAnimationClipHolder, 
                        true);
                }
            }
                
        }

        private float GetMagnitudeFromVectorDifference(Vector3 vectorA, Vector3 vectorB)
        {
            return (vectorA - vectorB).magnitude;
        }
        
        private bool InputIsLeft(Vector2 vectorA, Vector2 vectorB)
        {
            return -vectorA.x * vectorB.y + vectorA.y * vectorB.x < 0;
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
