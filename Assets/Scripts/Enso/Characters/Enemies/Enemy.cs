using System;
using Enso.CombatSystem;
using UnityEngine;

namespace Enso.Characters.Enemies
{
    public abstract class Enemy : Fighter
    {
        protected EnemyMovementController ThisEnemyMovementController;

        private void OnEnable()
        {
            ThisEnemyMovementController.UpdateDistanceToTargetValue += ChooseBehavior;
        }
        
        private void OnDisable()
        {
            ThisEnemyMovementController.UpdateDistanceToTargetValue -= ChooseBehavior;
        }

        protected override void Awake()
        {
            base.Awake();

            ThisEnemyMovementController = MovementController as EnemyMovementController;
        }

        protected virtual void Update()
        {
            if (!ThisEnemyMovementController)
                return;
        }
        
        protected virtual void ChooseBehavior()
        {
            
        }
    }
}
