using System;
using Enso.Enums;
using UnityEngine;

namespace Enso.Characters
{
    [RequireComponent(typeof(Fighter))]
    public sealed class HealthSystem : MonoBehaviour
    {
        private Fighter fighter;
        
        private int health;

        private int Health
        {
            get => health;
            set
            {
                if (IsDead)
                    return;
                
                health = value;

                if (health > maxHealth)
                    health = maxHealth;
                
                if (health <= 0)
                {
                    health = 0;
                    IsDead = true;
                    OnDeath();
                    return;
                }
                
                if (health < maxHealth)
                {
                    OnDamage();
                }
                
                OnHealthValueChanged();
            }
        }

        private int maxHealth;

        public event Action HealthValueChanged;
        public event Action Damage;
        public event Action Death;
        
        [HideInInspector] public AttackType CurrentAttackType;
        [HideInInspector] public bool IsDead;

        private void Awake()
        {
            fighter = GetComponent<Fighter>();
            maxHealth = fighter.GetBaseProperties().Health;
            Health = maxHealth;
        }

        public int GetHealth()
        {
            return Health;
        }

        public int GetMaxHealth()
        {
            return maxHealth;
        }

        public void IncreaseMaxHealth()
        {
            maxHealth += 1;
        }

        public float GetHealthPercentage()
        {
            return (float) Health / maxHealth;
        }

        public void TakeDamage(int damageAmount, AttackType attackType = AttackType.Light)
        {
            CurrentAttackType = attackType;
            Health -= damageAmount;
        }

        public void Heal(int healAmount)
        {
            Health += healAmount;
        }

        public bool CheckIfHealthIsAtMaximumValue()
        {
            return GetHealth() >= GetMaxHealth();
        }

        private void OnHealthValueChanged()
        {
            HealthValueChanged?.Invoke();
        }
        
        private void OnDamage()
        {
            Damage?.Invoke();
        }

        private void OnDeath()
        {
            Death?.Invoke();
        }
    }
}