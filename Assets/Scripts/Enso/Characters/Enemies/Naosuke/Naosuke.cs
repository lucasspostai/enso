using System;
using System.Collections;
using System.Diagnostics;
using Framework.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enso.Characters.Enemies.Naosuke
{
    public class Naosuke : Enemy
    {
        private bool chasePlayerAndAttack;
        private Coroutine waitForPlayerCoroutine;
        private int chosenBehaviorIndex;
        private int currentStage = 1;
        private UniqueRandom randomBehavior;

        [SerializeField] private NaosukeAttackController AttackController;
        [SerializeField] private NaosukeGuardController GuardController;
        [SerializeField] private EnemyRollController RollController;
        [SerializeField] private float TimeToWaitForThePlayer = 3f;
        [SerializeField] private float MinimumGuardTime = 0.5f;
        [SerializeField] private float MaximumGuardTime = 3f;
        [SerializeField] private float GuardCooldown = 3f;
        [SerializeField] private float LightAttackCooldown = 1f;
        [SerializeField] private float StrongAttackCooldown = 2f;
        [SerializeField] private float SpecialAttackCooldown = 2f;
        [SerializeField] private float RollCooldown = 2f;
        [SerializeField] [Range(0, 1)] private float Stage2Percentage = 0.66f;
        [SerializeField] [Range(0, 1)] private float Stage3Percentage = 0.33f;

        protected override void OnEnable()
        {
            base.OnEnable();

            GetHealthSystem().Damage += EnableGuardImmediately;
            GetHealthSystem().Damage += ChangeStage;
            GetBalanceSystem().LoseBalance += StayOnGuard;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            GetHealthSystem().Damage -= EnableGuardImmediately;
            GetHealthSystem().Damage -= ChangeStage;
            GetBalanceSystem().LoseBalance -= StayOnGuard;
        }

        protected override void Start()
        {
            base.Start();

            AttackController.SetMaxCombo(2);

            MustMove(true);

            Wait();

            randomBehavior = new UniqueRandom(0, 6);

            chosenBehaviorIndex = randomBehavior.GetRandomInt();
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

                    if (currentStage >= 3 && chosenBehaviorIndex >= 5 && AttackController.CanUseSpecialAttack &&
                        AttackController.CanAttack &&
                        ThisEnemyMovementController.DistanceToTarget < GetProperties().SpecialAttackDistance)
                    {
                        PerformSpecialAttack();
                        AttackController.WaitAfterAttack(SpecialAttackCooldown);

                        Wait();
                        
                        chosenBehaviorIndex = randomBehavior.GetRandomInt();
                    }
                    else if (currentStage >= 2 && chosenBehaviorIndex >= 3 && AttackController.CanUseStrongAttack &&
                             AttackController.CanAttack &&
                             ThisEnemyMovementController.DistanceToTarget < GetProperties().StrongAttackDistance)
                    {
                        PerformStrongAttack();
                        AttackController.WaitAfterAttack(StrongAttackCooldown);

                        Wait();
                        
                        chosenBehaviorIndex = randomBehavior.GetRandomInt();
                    }
                    else if (AttackController.CanAttack &&
                             ThisEnemyMovementController.DistanceToTarget < GetProperties().LightAttackDistance)
                    {
                        PerformLightAttack();
                        AttackController.WaitAfterAttack(LightAttackCooldown);

                        Wait();
                        
                        chosenBehaviorIndex = randomBehavior.GetRandomInt();
                    }
                }
                else
                {
                    if (ThisEnemyMovementController.DistanceToTarget < GetProperties().GuardDistance)
                    {
                        if (chosenBehaviorIndex >= 5)
                        {
                            MustMove(false);

                            StartParry();

                            Wait();
                            
                            chosenBehaviorIndex = randomBehavior.GetRandomInt();
                        }

                        if (chosenBehaviorIndex >= 3 && RollController.CanRoll &&
                            ThisEnemyMovementController.DistanceToTarget < GetProperties().RollDistance)
                        {
                            MustMove(false);

                            PerformRoll();
                            RollController.WaitAfterRoll(RollCooldown);

                            Wait();
                            
                            chosenBehaviorIndex = randomBehavior.GetRandomInt();
                        }
                        else if (GuardController.CanGuard)
                        {
                            MustMove(true);
                            StartGuard();
                            GuardController.WaitAfterStartGuard(Random.Range(MinimumGuardTime, MaximumGuardTime));
                            GuardController.WaitAfterEndGuard(GuardCooldown);

                            Wait();
                            
                            chosenBehaviorIndex = randomBehavior.GetRandomInt();
                        }
                    }
                }
            }
        }

        private void Wait()
        {
            if (waitForPlayerCoroutine != null)
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

        private void PerformSpecialAttack()
        {
            AttackController.StartSpecialAttack();
        }

        private void StartGuard()
        {
            GuardController.StartGuard();
            GuardController.WaitAfterStartGuard(3);
        }

        private void PerformRoll()
        {
            RollController.PlayRollAnimation();
        }

        private void StartParry()
        {
            GuardController.Parry();
        }

        private void EnableGuardImmediately()
        {
            GuardController.CanGuard = true;
        }

        private void ChangeStage()
        {
            if (GetHealthSystem().GetHealth() < GetHealthSystem().GetMaxHealth() * Stage3Percentage &&
                currentStage != 3)
            {
                currentStage = 3;
                randomBehavior = new UniqueRandom(0, 6);
            }
            else if (GetHealthSystem().GetHealth() < GetHealthSystem().GetMaxHealth() * Stage2Percentage &&
                     currentStage != 2)
            {
                currentStage = 2;
                randomBehavior = new UniqueRandom(0, 6);
                AttackController.SetMaxCombo(3);
            }
        }

        private void StayOnGuard()
        {
            GuardController.WaitAfterStartGuard(2);
        }

        public NaosukeProperties GetProperties()
        {
            return BaseProperties as NaosukeProperties;
        }
    }
}