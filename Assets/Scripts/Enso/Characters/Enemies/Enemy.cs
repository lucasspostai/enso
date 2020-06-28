using System;
using Enso.CombatSystem;
using UnityEngine;

namespace Enso.Characters.Enemies
{
    [RequireComponent(typeof(XpDropper))]
    public abstract class Enemy : Fighter
    {
        private Transform target;
        private XpDropper xpDropper;

        protected EnemyMovementController ThisEnemyMovementController;

        protected virtual void OnEnable()
        {
            ThisEnemyMovementController.UpdateDistanceToTargetValue += ChooseBehavior;
            GetHealthSystem().Death += OnDeath;
        }

        protected virtual void OnDisable()
        {
            ThisEnemyMovementController.UpdateDistanceToTargetValue -= ChooseBehavior;
            GetHealthSystem().Death -= OnDeath;
        }

        protected override void Awake()
        {
            base.Awake();

            ThisEnemyMovementController = MovementController as EnemyMovementController;
        }

        protected override void Start()
        {
            base.Start();

            xpDropper = GetComponent<XpDropper>();
        }

        protected void MustMove(bool move)
        {
            ThisEnemyMovementController.MustMove = move;
        }

        protected virtual void ChooseBehavior()
        {
        }

        public override void EnterCombatWith(Fighter fighter)
        {
            base.EnterCombatWith(fighter);

            target = Target;
        }

        private void OnDeath()
        {
            if (!target && !GetHealthSystem().IsDead)
                return;

            var xpReceiver = target.GetComponent<XpReceiver>();

            if (xpReceiver)
                xpReceiver.GainXp(xpDropper.XpAmount);

            target = null;
        }
    }
}