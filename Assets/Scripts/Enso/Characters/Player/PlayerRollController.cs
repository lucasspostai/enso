using Enso.CombatSystem;
using UnityEngine;

namespace Enso.Characters.Player
{
    public class PlayerRollController : RollController
    {
        private Player player;

        [SerializeField] [Range(0, 1)] private float RollCost = 0.1f;

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

        public override void PlayRollAnimation()
        {
            if (ThisFighter.GetBalanceSystem().GetBalance() <= 0)
                return;
            
            base.PlayRollAnimation();

            //Roll Cost
            player.GetBalanceSystem()
                .TakeDamage(Mathf.RoundToInt(player.GetBalanceSystem().GetMaxBalance() * RollCost));
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