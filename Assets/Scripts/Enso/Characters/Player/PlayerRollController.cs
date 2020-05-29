using Enso.CombatSystem;
using UnityEngine;

namespace Enso.Characters.Player
{
    public class PlayerRollController : RollController
    {
        private Player player;
        
        #region Delegates

        private void OnEnable()
        {
            PlayerInput.RollInputDown += PlayRollAnimation;
        }

        private void OnDisable()
        {
            PlayerInput.RollInputDown -= PlayRollAnimation;
        }

        #endregion
        
        protected override void Start()
        {
            base.Start();

            player = GetComponent<Player>();
        }

        protected override void SetDirection()
        {
            base.SetDirection();
            
            if (PlayerInput.Movement != Vector2.zero)
                ThisFighter.AnimationHandler.SetFacingDirection(PlayerInput.Movement);
        }

        public override void OnLastFrameEnd()
        {
            base.OnLastFrameEnd();

            if (PlayerInput.HoldingGuardInput)
            {
                player.GuardController.StartGuard();
            }
            else if (PlayerInput.HoldingHealInput)
            {
                player.HealController.TryHeal();
            }
        }
    }
}
