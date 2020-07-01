using Enso.CombatSystem;
using UnityEngine;

namespace Enso.Characters.Enemies.AshigaruArcher
{
    public class AshigaruArcher : Enemy
    {
        [SerializeField] private AshigaruArcherAttackController AttackController;
        [SerializeField] private EnemyRollController RollController;
        [SerializeField] private float DelayAfterRoll;

        protected override void ChooseBehavior()
        {
            if (GetHealthSystem().IsDead)
                return;
            
            base.ChooseBehavior();

            if (AnimationHandler.IsAnyCustomAnimationPlaying())
                return;

            if (ThisEnemyMovementController.DistanceToTarget < GetProperties().ShootArrowDistance)
            {
                MustMove(false);
                
                if (RollController.CanRoll && ThisEnemyMovementController.DistanceToTarget < GetProperties().RollDistance)
                {
                    PerformRoll();
                    RollController.WaitAfterRoll(DelayAfterRoll);
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