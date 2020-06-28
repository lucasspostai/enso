using Framework.Utils;
using UnityEngine;

namespace Enso.Characters.Enemies.AshigaruWarrior
{
    public class AshigaruWarrior : Enemy
    {
        private int chosenBehaviorIndex;
        private UniqueRandom randomBehavior;
        
        [SerializeField] private AshigaruWarriorAttackController AttackController;
        [SerializeField] private AshigaruWarriorGuardController GuardController;
        [SerializeField] private float MinimumGuardTime = 0.5f;
        [SerializeField] private float MaximumGuardTime = 3f;
        [SerializeField] private float GuardCooldown = 3f;
        [SerializeField] private float AttackCooldown = 0f;

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

        protected override void Start()
        {
            base.Start();

            randomBehavior = new UniqueRandom(0, 2);
            
            chosenBehaviorIndex = randomBehavior.GetRandomInt();
        }

        protected override void ChooseBehavior()
        {
            if (GetHealthSystem().IsDead)
                return;
            
            base.ChooseBehavior();

            if (AnimationHandler.IsAnyCustomAnimationPlaying() || AnimationHandler.IsAnyGuardAnimationPlaying())
                return;

            if (ThisEnemyMovementController.DistanceToTarget < GetProperties().GuardDistance)
            {
                if (GuardController.CanGuard && chosenBehaviorIndex == 0)
                {
                    MustMove(false);
                    StartGuard();
                    GuardController.WaitAfterStartGuard(Random.Range(MinimumGuardTime, MaximumGuardTime));
                    GuardController.WaitAfterEndGuard(GuardCooldown);
                    
                    chosenBehaviorIndex = randomBehavior.GetRandomInt();
                }
                else
                {
                    MustMove(true);
                    
                    if (AttackController.CanAttack &&
                        ThisEnemyMovementController.DistanceToTarget < GetProperties().AttackDistance)
                    {
                        PerformSimpleAttack();
                        AttackController.WaitAfterAttack(AttackCooldown);
                        
                        chosenBehaviorIndex = randomBehavior.GetRandomInt();
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