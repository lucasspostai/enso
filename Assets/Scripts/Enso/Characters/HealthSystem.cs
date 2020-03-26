using System;
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
                health = value;

                if (health < 0)
                    health = 0;

                if (health > maxHealth)
                    health = maxHealth;

                OnHealthValueChanged();
            }
        }

        private int maxHealth;

        public event Action HealthValueChanged;
        public event Action Death;

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

        public float GetHealthPercentage()
        {
            return (float) Health / maxHealth;
        }

        public void TakeDamage(int damageAmount)
        {
            Health -= damageAmount;
        }

        public void Heal(int healAmount)
        {
            Health += healAmount;
        }

        private void OnHealthValueChanged()
        {
            HealthValueChanged?.Invoke();
        }

        private void OnDeath()
        {
            Death?.Invoke();
        }
    }
}