﻿using Enso.CombatSystem;
using Framework;
using UnityEngine;

namespace Enso.Characters
{
    [RequireComponent(typeof(HealthSystem))]
    [RequireComponent(typeof(BalanceSystem))]
    [RequireComponent(typeof(CharacterAnimationHandler))]
    public abstract class Fighter : MonoBehaviour
    {
        private HealthSystem healthSystem;
        private BalanceSystem balanceSystem;

        [SerializeField] protected FighterProperties BaseProperties;
        
        public CharacterCollisions Collisions;
        public CharacterMovementController MovementController;

        [HideInInspector] public CharacterAnimationHandler AnimationHandler;

        protected virtual void Awake()
        {
            healthSystem = GetComponent<HealthSystem>();
            balanceSystem = GetComponent<BalanceSystem>();
            AnimationHandler = GetComponent<CharacterAnimationHandler>();
        }

        protected void Start()
        {
            MovementController.SetFighter(this);
        }

        public HealthSystem GetHealthSystem()
        {
            if(!healthSystem)
                healthSystem = GetComponent<HealthSystem>();
                
            return healthSystem;
        }
        
        public BalanceSystem GetBalanceSystem()
        {
            if(!balanceSystem)
                balanceSystem = GetComponent<BalanceSystem>();
                
            return balanceSystem;
        }

        public FighterProperties GetBaseProperties()
        {
            return BaseProperties;
        }
    }
}
