using System.Collections;
using Enso.Characters.Enemies.AshigaruWarrior;
using UnityEngine;

namespace Enso.Characters.Enemies.Komuso
{
    public class Komuso : Enemy
    {
        private bool chasePlayerAndAttack;
        private Coroutine waitForPlayerCoroutine;

        [SerializeField] private KomusoAttackController AttackController;
        [SerializeField] private KomusoGuardController GuardController;
        [SerializeField] private float TimeToWaitForThePlayer = 3f;
        [SerializeField] private float MinimumGuardTime = 0.5f;
        [SerializeField] private float MaximumGuardTime = 3f;
        [SerializeField] private float GuardCooldown = 3f;
        [SerializeField] private float LightAttackCooldown = 1f;
        [SerializeField] private float StrongAttackCooldown = 2f;

        protected override void OnEnable()
        {
            base.OnEnable();

            GetHealthSystem().Damage += EnableGuardImmediately;
            GetHealthSystem().Damage += EnableParry;
            GetBalanceSystem().LoseBalance += StayOnGuard;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            GetHealthSystem().Damage -= EnableGuardImmediately;
            GetHealthSystem().Damage -= EnableParry;
            GetBalanceSystem().LoseBalance -= StayOnGuard;
        }

        protected override void Start()
        {
            base.Start();
            
            MustMove(true);
            
            Wait();
        }

        protected override void ChooseBehavior()
        {
            base.ChooseBehavior();
            
            if (AnimationHandler.IsAnyCustomAnimationPlaying() || AnimationHandler.IsAnyGuardAnimationPlaying())
                return;

            if (ThisEnemyMovementController.DistanceToTarget < GetProperties().WaitDistance)
            {
                if (chasePlayerAndAttack)
                {
                    MustMove(false);

                    if (AttackController.CanUseStrongAttack && AttackController.CanAttack &&
                        ThisEnemyMovementController.DistanceToTarget < GetProperties().StrongAttackDistance)
                    {
                        PerformStrongAttack();
                        AttackController.WaitAfterAttack(StrongAttackCooldown);
                        
                        Wait();
                    }
                    else if (AttackController.CanAttack &&
                             ThisEnemyMovementController.DistanceToTarget < GetProperties().LightAttackDistance)
                    {
                        PerformLightAttack();
                        AttackController.WaitAfterAttack(LightAttackCooldown);
                        
                        Wait();
                    }
                }
                else
                {
                    if (ThisEnemyMovementController.DistanceToTarget < GetProperties().GuardDistance)
                    {
                        if (GuardController.CanParry &&
                            GetHealthSystem().GetHealth() < GetHealthSystem().GetMaxHealth() / 2)
                        {
                            MustMove(false);
                            
                            StartParry();
                            
                            Wait();
                        }
                        else if (GuardController.CanGuard)
                        {
                            MustMove(true);
                            StartGuard();
                            GuardController.WaitAfterStartGuard(Random.Range(MinimumGuardTime, MaximumGuardTime));
                            GuardController.WaitAfterEndGuard(GuardCooldown);

                            Wait();
                        }
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

        private void Wait()
        {
            if(waitForPlayerCoroutine != null)
                StopCoroutine(waitForPlayerCoroutine);

            waitForPlayerCoroutine = StartCoroutine(WaitForPlayer());
        }

        private IEnumerator WaitForPlayer()
        {
            chasePlayerAndAttack = false;

            yield return new WaitForSeconds(TimeToWaitForThePlayer);

            chasePlayerAndAttack = true;
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

        private void EnableParry()
        {
            if(GetHealthSystem().GetHealth() < GetHealthSystem().GetMaxHealth() / 2)
                GuardController.CanParry = true;
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