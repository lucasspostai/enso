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

        protected override void Start()
        {
            base.Start();

            ghostEffectController = GetComponent<CharacterGhostEffectController>();
        }

        protected void PlayRollAnimation()
        {
            if (ThisFighter.AnimationHandler.IsAnyAnimationDifferentThanAttackPlaying() ||
                !ThisFighter.AnimationHandler.CanCutAttackAnimation())
                return;

            ThisFighter.AnimationHandler.InterruptAllGuardAnimations();

            CurrentCharacterAnimation = RollAnimation;

            SetDirection();
            
            ghostEffectController.ActivateGhostEffects();

            SetAnimationPropertiesAndPlay(RollAnimation.ClipHolder, RollAnimation.AnimationFrameChecker);
        }

        public override void OnInterrupted()
        {
            base.OnInterrupted();
            
            ghostEffectController.DisableGhostEffects();
        }

        public override void OnEndMovement()
        {
            base.OnEndMovement();
            
            ghostEffectController.DisableGhostEffects();
        }

        protected virtual void SetDirection()
        {
        }
    }
}