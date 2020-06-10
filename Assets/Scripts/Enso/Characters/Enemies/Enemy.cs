using System;
using Enso.CombatSystem;
using UnityEngine;

namespace Enso.Characters.Enemies
{
    public abstract class Enemy : Fighter
    {
        protected EnemyMovementController ThisEnemyMovementController;

        protected override void Start()
        {
            base.Start();
            
            ThisEnemyMovementController = MovementController as EnemyMovementController;
        }

        protected virtual void Update()
        {
            if (!ThisEnemyMovementController)
                return;
        }
    }
}
