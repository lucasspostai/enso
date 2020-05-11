using Enso.Characters;
using Enso.Characters.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Enso.UI
{
    public class HealthBar : MonoBehaviour
    {
        private Player player;
        private HealthSystem healthSystem;
        
        [SerializeField] private Image[] HealthImages;
        [SerializeField] private Sprite FullHealth;
        [SerializeField] private Sprite EmptyHealth;
        
        private void OnEnable()
        {
            player = FindObjectOfType<Player>();

            if (player == null) 
                return;
            
            healthSystem = player.GetComponent<HealthSystem>();

            if (healthSystem != null)
            {
                healthSystem.HealthValueChanged += UpdateHealthValue;
                UpdateHealthValue();
            }
        }

        private void OnDisable()
        {
            if (healthSystem != null)
            {
                healthSystem.HealthValueChanged -= UpdateHealthValue;
            }
        }

        private void UpdateHealthValue()
        {
            if (healthSystem == null)
                return;
            
            for (int i = 0; i < healthSystem.GetMaxHealth(); i++)
            {
                HealthImages[i].sprite = i < healthSystem.GetHealth() ? FullHealth : EmptyHealth;
            }
        }
    }
}
