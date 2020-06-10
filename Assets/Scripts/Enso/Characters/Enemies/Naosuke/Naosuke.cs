using System;
using Enso.CombatSystem;
using UnityEditor;
using UnityEngine;

namespace Enso.Characters.Enemies.Naosuke
{
    public class Naosuke : Enemy
    {
        private bool canAttack = true;
        
        [SerializeField] private NaosukeAttackController ThisAttackController;
        [SerializeField] private RollController ThisRollController;

        protected override void Start()
        {
            base.Start();
            
            ThisAttackController.SetMaxCombo(2);
        }

        protected override void Update()
        {
            base.Update();

            if (AnimationHandler.IsAnyCustomAnimationPlaying())
                return;
            
            if (ThisEnemyMovementController.DistanceToTarget < 2f)
            {
                canAttack = false;
                PerformSimpleAttack();
            }
        }

        private void PerformSimpleAttack()
        {
            
            
            ThisAttackController.StartLightAttack();
        }

        private void PerformStrongAttack()
        {
            
        }

        private void PerformSpecialAttack()
        {
            
        }

        private void PerformRoll()
        {
            
        }

        public NaosukeProperties GetProperties()
        {
            return BaseProperties as NaosukeProperties;
        }
    }
}
