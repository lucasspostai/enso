using System.Collections;
using Enso.UI;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;

namespace Enso.Characters.Enemies
{
    public class EnemyHealthBar : Element
    {
        private Coroutine healthCoroutine;
        private int currentHealth;
        private HealthSystem healthSystem;

        [SerializeField] private Fighter ThisFighter;
        [SerializeField] private Image HealthImage;
        
        private void OnEnable()
        {
            if (ThisFighter == null)
                return;
            
            healthSystem = ThisFighter.GetComponent<HealthSystem>();

            if (healthSystem != null)
            {
                healthSystem.HealthValueChanged += UpdateHealthValue;
                healthSystem.Death += Disable;
                
                UpdateHealthValue();
            }
        }

        private void OnDisable()
        {
            if (healthSystem != null)
            {
                healthSystem.HealthValueChanged -= UpdateHealthValue;
                healthSystem.Death -= Disable;
            }
        }
        
        public override void Disable()
        {
            base.Disable();

            OnDisable();
        }

        private void UpdateHealthValue()
        {
            HealthImage.fillAmount = (float)healthSystem.GetHealth() / healthSystem.GetMaxHealth();
            
            UpdateInfo();
        }
    }
}
