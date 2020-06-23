using Enso.Characters.Enemies.AshigaruWarrior;
using UnityEngine;

namespace Enso.Characters.Enemies.Komuso
{
    public class Komuso : Enemy
    {
        [SerializeField] private KomusoAttackController AttackController;
        [SerializeField] private KomusoGuardController GuardController;

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
            
            return;


            if (ThisEnemyMovementController.DistanceToTarget < GetProperties().GuardDistance)
            {
                if (AnimationHandler.IsAnyCustomAnimationPlaying() || AnimationHandler.IsAnyGuardAnimationPlaying())
                    return;
                
                if(GuardController.CanParry)
                    StartParry();
                else if (GuardController.CanGuard) //Guard
                {
                    MustMove(true);
                    StartGuard();
                    GuardController.WaitAfterStartGuard(2);
                    GuardController.WaitAfterEndGuard(5);
                }
                else //Attack
                {
                    MustMove(false);

                    if (AttackController.CanUseStrongAttack && AttackController.CanAttack &&
                        ThisEnemyMovementController.DistanceToTarget < GetProperties().StrongAttackDistance)
                    {
                        PerformStrongAttack();
                        AttackController.WaitAfterAttack(0.5f);
                    }
                    else if (AttackController.CanAttack &&
                             ThisEnemyMovementController.DistanceToTarget < GetProperties().LightAttackDistance)
                    {
                        PerformLightAttack();
                        AttackController.WaitAfterAttack(0.5f);
                    }
                }
            }
            else //Follow Target
            {
                MustMove(true);

                if (GuardController.IsGuarding)
                    GuardController.EndGuard();
            }
        }

        private void PerformLightAttack()
        {
            AttackController.StartLightAttack();
        }

        private void PerformStrongAttack()
        {
            AttackController.StartStrongAttack();
        }

        private void StartGuard()
        {
            GuardController.StartGuard();
        }

        private void StartParry()
        {
            GuardController.Parry();
        }

        private void EnableGuardImmediately()
        {
            GuardController.CanGuard = true;
        }

        private void StayOnGuard()
        {
            GuardController.WaitAfterStartGuard(2);
        }

        public KomusoProperties GetProperties()
        {
            return BaseProperties as KomusoProperties;
        }
    }
}