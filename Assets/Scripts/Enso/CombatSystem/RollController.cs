using UnityEngine;

namespace Enso.CombatSystem
{
    public class RollController : CustomAnimationController
    {
        [SerializeField] protected ActionAnimation RollAnimation;

        protected void PlayRollAnimation()
        {
            if (ThisFighter.AnimationHandler.IsAnyAnimationDifferentThanAttackPlaying() || 
                !ThisFighter.AnimationHandler.CanCutAttackAnimation())
                return;

            CurrentCharacterAnimation = RollAnimation;

            SetAnimationPropertiesAndPlay(RollAnimation.ClipHolder, RollAnimation.AnimationFrameChecker);
        }
    }
}
