using Enso.CombatSystem;
using UnityEngine;

namespace Enso.Characters.Enemies.AshigaruWarrior
{
    public class AshigaruWarriorAttackController : EnemyAttackController
    {
        [SerializeField] private AttackAnimation LightAttackAnimation;

        public void StartLightAttack()
        {
            if (ThisFighter.AnimationHandler.IsAnyAnimationDifferentThanAttackPlaying() ||
                ThisFighter.AnimationHandler.IsAnyGuardAnimationPlaying())
                return;

            RotateTowardsTarget();

            //CanCutAnimation = true;

            StartAttack(LightAttackAnimation);

            CanAttack = false;
        }

        public override void OnLastFrameEnd()
        {
            print("LAST FRAME");
            
            base.OnLastFrameEnd();

            ThisFighter.AnimationHandler.Play(this,
                ThisFighter.MovementController.Animations.IdleAnimationClipHolder.AnimatorStateName);
            
            Wait();
        }
    }
}