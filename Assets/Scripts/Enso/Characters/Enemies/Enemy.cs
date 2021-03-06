﻿using System;
using Enso.CombatSystem;
using Enso.UI;
using Framework;
using UnityEngine;

namespace Enso.Characters.Enemies
{
    [RequireComponent(typeof(XpDropper))]
    public abstract class Enemy : Fighter
    {
        private Transform target;
        private XpDropper xpDropper;

        protected EnemyMovementController ThisEnemyMovementController;

        [SerializeField] private Element EnemyCanvas;
        
        [HideInInspector] public bool IsInCombat;

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

            if (EnemyCanvas)
            {
                EnemyCanvas.gameObject.SetActive(true);
                EnemyCanvas.Enable();
            }
        }

        private void OnDeath()
        {
            if (!target && !GetHealthSystem().IsDead)
                return;
            
            if(EnemyCanvas)
                EnemyCanvas.Disable();
            
            var xpReceiver = target.GetComponent<XpReceiver>();

            if (xpReceiver && xpDropper)
                xpReceiver.GainXp(xpDropper.XpAmount);

            target = null;

            Collisions.DisableCollisions();
            Collisions.enabled = false;
            
            AnimationHandler.ChangeSortingOrderOnDeath();
        }
    }
}