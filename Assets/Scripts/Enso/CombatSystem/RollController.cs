using System.Collections;
using Framework;
using UnityEngine;

namespace Enso.CombatSystem
{
    [RequireComponent(typeof(CharacterGhostEffectController))]
    public class RollController : CustomAnimationController
    {
        private CharacterGhostEffectController ghostEffectController;

        [SerializeField] protected ActionAnimation RollAnimation;
        [SerializeField] [Range(0, 1)] private float RollCost = 0.1f;

        protected override void Start()
        {
            base.Start();

            ghostEffectController = GetComponent<CharacterGhostEffectController>();
        }

        public virtual void PlayRollAnimation()
        {
            if (ThisFighter.GetBalanceSystem().GetBalance() <= 0 ||
                ThisFighter.AnimationHandler.IsAnyAnimationDifferentThanAttackPlaying() ||
                !ThisFighter.AnimationHandler.CanCutAttackAnimation())
                return;

            ThisFighter.AnimationHandler.InterruptAllGuardAnimations();

            CurrentCharacterAnimation = RollAnimation;

            SetDirection();

            if (ghostEffectController)
                ghostEffectController.ActivateGhostEffects();

            SetAnimationPropertiesAndPlay(RollAnimation.ClipHolder, RollAnimation.AnimationFrameChecker, false);

            ThisFighter.GetHealthSystem().IsInvincible = true;

            //Roll Cost
            ThisFighter.GetBalanceSystem()
                .TakeDamage(Mathf.RoundToInt(ThisFighter.GetBalanceSystem().GetMaxBalance() * RollCost));
        }

        public override void OnInterrupted()
        {
            base.OnInterrupted();

            if (ghostEffectController)
                ghostEffectController.DisableGhostEffects();
            
            if (ThisFighter)
                ThisFighter.GetHealthSystem().IsInvincible = false;
        }

        public override void OnEndMovement()
        {
            base.OnEndMovement();

            ghostEffectController.DisableGhostEffects();
        }

        protected virtual void SetDirection()
        {
        }

        public override void OnLastFrameEnd()
        {
            base.OnLastFrameEnd();
            
            if (ThisFighter)
                ThisFighter.GetHealthSystem().IsInvincible = false;
        }
    }
}