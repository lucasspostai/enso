using UnityEngine;

namespace Enso.Characters.Enemies.AshigaruWarrior
{
    public class AshigaruWarrior : Enemy
    {
        [SerializeField] private AshigaruWarriorAttackController AttackController;
        [SerializeField] private AshigaruWarriorGuardController GuardController;

        protected override void OnEnable()
        {
            base.OnEnable();

            GetHealthSystem().Damage += EnableGuardImmediately;
            GetBalanceSystem().LoseBalance += StayOnGuard;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            GetHealthSystem().Damage -= EnableGuardImmediately;
            GetBalanceSystem().LoseBalance -= StayOnGuard;
        }

        protected override void ChooseBehavior()
        {
            base.ChooseBehavior();

            if (AnimationHandler.IsAnyCustomAnimationPlaying())
                return;

            if (ThisEnemyMovementController.DistanceToTarget < GetProperties().GuardDistance)
            {
                if (GuardController.CanGuard)
                {
                    MustMove(false);
                    StartGuard();
                    GuardController.WaitAfterStartGuard(3);
                    GuardController.WaitAfterEndGuard(3);
                }
                else
                {
                    MustMove(true);
                    
                    if (AttackController.CanAttack &&
                        ThisEnemyMovementController.DistanceToTarget < GetProperties().AttackDistance)
                    {
                        PerformSimpleAttack();
                        AttackController.WaitAfterAttack(1);
                    }
                }
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

        private void EnableGuardImmediately()
        {
            GuardController.CanGuard = true;
        }

        private void StayOnGuard()
        {
            GuardController.WaitAfterStartGuard(2);
        }

        public AshigaruWarriorProperties GetProperties()
        {
            return BaseProperties as AshigaruWarriorProperties;
        }
    }
}