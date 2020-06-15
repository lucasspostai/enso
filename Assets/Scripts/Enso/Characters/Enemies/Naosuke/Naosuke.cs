using System;
using Enso.CombatSystem;
using UnityEditor;
using UnityEngine;

namespace Enso.Characters.Enemies.Naosuke
{
    public class Naosuke : Enemy
    {
        [SerializeField] private NaosukeAttackController AttackController;
        [SerializeField] private NaosukeGuardController GuardController;
        [SerializeField] private NaosukeRollController RollController;
        
        protected override void Start()
        {
            base.Start();
            
            AttackController.SetMaxCombo(2);
        }

        protected override void ChooseBehavior()
        {
            base.ChooseBehavior();
            
            if (AnimationHandler.IsAnyCustomAnimationPlaying() || AnimationHandler.IsAnyGuardAnimationPlaying())
                return;
            
            //if(!AttackController.CanAttack)
            //    PerformRoll();
            
            if(!AttackController.CanAttack)
                StartGuard();
            
            if (AttackController.CanAttack && ThisEnemyMovementController.DistanceToTarget < 1.5f)
            {
                // if (AttackController.CanUseSpecialAttack)
                // {
                //     PerformSpecialAttack();
                //     AttackController.WaitAfterAttack(1);
                // }
                // else if (AttackController.CanUseStrongAttack)
                // {
                //     PerformStrongAttack();
                //     AttackController.WaitAfterAttack(1);
                // }
                // else
                // {
                    PerformSimpleAttack();
                    AttackController.WaitAfterAttack(3);
                // }
            }
        }

        private void PerformSimpleAttack()
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

        public NaosukeProperties GetProperties()
        {
            return BaseProperties as NaosukeProperties;
        }
    }
}
