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
            PlayerInput.Parry += Parry;
        }

        private void OnDisable()
        {
            PlayerInput.GuardInputDown -= StartGuard;
            PlayerInput.GuardInputUp -= EndGuard;
            PlayerInput.Parry -= Parry;
        }

        #endregion

        protected override void Start()
        {
            base.Start();

            player = GetComponent<Player>();
        }

        protected override void Update()
        {
            base.Update();
            
            if(IsGuarding)
                player.SetActionDirection();
        }

        protected override void PlayMovementAnimation()
        {
            base.PlayMovementAnimation();

            if (player.MovementController.Velocity == Vector3.zero)
            {
                PlayGuardAnimation(Animations.GuardIdleAnimationClipHolder);
            }
            else
            {
                var magnitude = VectorExtender.GetMagnitudeFromVectorDifference(
                    player.AnimationHandler.CurrentDirection,
                    PlayerInput.Movement
                );

                if (magnitude <= 0.5f)
                    PlayGuardAnimation(Animations.ForwardGuardWalkAnimationClipHolder, true);
                else if (magnitude >= 1.5f)
                    PlayGuardAnimation(Animations.BackwardGuardWalkAnimationClipHolder, true);
                else
                {
                    PlayGuardAnimation(
                        VectorExtender.InputIsLeft(player.AnimationHandler.CurrentDirection, PlayerInput.Movement)
                            ? Animations.LeftGuardWalkAnimationClipHolder
                            : Animations.RightGuardWalkAnimationClipHolder,
                        true
                    );
                }
            }
        }

        public override void Block()
        {
            base.Block();
            
            GameManager.Instance.ChangeTimeScale(0f, 0.02f);
        }

        protected override void ResetAllProperties()
        {
            base.ResetAllProperties();

            if (player)
                player.MovementController.SetSpeed(player.GetBaseProperties().RunSpeed);
        }
    }
}