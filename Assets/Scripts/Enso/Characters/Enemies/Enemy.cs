using System;
using Enso.CombatSystem;
using UnityEngine;

namespace Enso.Characters.Enemies
{
    public abstract class Enemy : Fighter
    {
        protected EnemyMovementController ThisEnemyMovementController;

        protected virtual void OnEnable()
        {
            ThisEnemyMovementController.UpdateDistanceToTargetValue += ChooseBehavior;
        }
        
        protected virtual void OnDisable()
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

        protected void MustMove(bool move)
        {
            ThisEnemyMovementController.MustMove = move;
        }
        
        protected virtual void ChooseBehavior()
        {
            
        }
    }
}
