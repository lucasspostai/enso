using UnityEngine;

namespace Enso.Characters.Enemies.AshigaruWarrior
{
    public class AshigaruWarrior : Enemy
    {
        [SerializeField] private AshigaruWarriorAttackController AttackController;
        [SerializeField] private AshigaruWarriorGuardController GuardController;

        protected override void ChooseBehavior()
        {
            base.ChooseBehavior();

            if (AnimationHandler.IsAnyCustomAnimationPlaying() || AnimationHandler.IsAnyGuardAnimationPlaying())
                return;

            if (ThisEnemyMovementController.DistanceToTarget < 10f)
            {
                if (!AttackController.CanAttack)
                {
                    StartGuard();
                }

                if (ThisEnemyMovementController.DistanceToTarget < 1.5f)
                {
                    PerformSimpleAttack();
                    AttackController.WaitAfterAttack(2);
                }
            }
        }

        private void PerformSimpleAttack()
        {
            AttackController.StartLightAttack();
        }

        private void StartGuard()
        {
            GuardController.StartGuard();
            GuardController.WaitAfterStartGuard(3);
        }

        public AshigaruWarriorProperties GetProperties()
        {
            return BaseProperties as AshigaruWarriorProperties;
        }
    }
}