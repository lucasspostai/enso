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

            if (AnimationHandler.IsAnyCustomAnimationPlaying())
                return;

            if (GuardController.CanGuard &&
                ThisEnemyMovementController.DistanceToTarget < GetProperties().GuardDistance)
            {
                MustMove(false);
                StartGuard();
                GuardController.WaitAfterStartGuard(3);
                GuardController.WaitAfterEndGuard(3);
            }
            else if (AttackController.CanAttack &&
                     ThisEnemyMovementController.DistanceToTarget < GetProperties().AttackDistance)
            {
                PerformSimpleAttack();
                AttackController.WaitAfterAttack(1);
            }
            else
            {
                MustMove(true);
            }
        }

        private void PerformSimpleAttack()
        {
            AttackController.StartLightAttack();
        }

        private void StartGuard()
        {
            GuardController.StartGuard();
        }

        public AshigaruWarriorProperties GetProperties()
        {
            return BaseProperties as AshigaruWarriorProperties;
        }
    }
}