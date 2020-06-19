using Enso.CombatSystem;
using UnityEngine;

namespace Enso.Characters.Enemies.AshigaruArcher
{
    public class AshigaruArcher : Enemy
    {
        [SerializeField] private AshigaruArcherAttackController AttackController;
        [SerializeField] private EnemyRollController RollController;

        protected override void OnEnable()
        {
            base.OnEnable();

            GetHealthSystem().Damage += BreakBalance;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            GetHealthSystem().Damage -= BreakBalance;
        }

        private void BreakBalance()
        {
            if (GetBalanceSystem().GetBalance() > 0)
                GetBalanceSystem().TakeDamage(Mathf.RoundToInt(GetBalanceSystem().GetMaxBalance()));
        }

        protected override void ChooseBehavior()
        {
            base.ChooseBehavior();

            if (AnimationHandler.IsAnyCustomAnimationPlaying())
                return;

            if (ThisEnemyMovementController.DistanceToTarget < GetProperties().ShootArrowDistance)
            {
                MustMove(false);
                
                if (ThisEnemyMovementController.DistanceToTarget < GetProperties().RollDistance)
                {
                    PerformRoll();
                }
                else if (AttackController.CanAttack)
                {
                    RaiseBow();
                }
            }
            else
            {
                MustMove(true);
            }
        }

        private void PerformRoll()
        {
            RollController.PlayRollAnimation();
        }

        private void RaiseBow()
        {
            AttackController.RaiseBow();
        }
        
        public AshigaruArcherProperties GetProperties()
        {
            return BaseProperties as AshigaruArcherProperties;
        }
    }
}