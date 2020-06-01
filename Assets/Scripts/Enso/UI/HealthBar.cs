using System.Collections;
using Enso.Characters;
using Enso.Characters.Player;
using Enso.CombatSystem;
using TMPro;
using UnityEngine;

namespace Enso.UI
{
    public class HealthBar : MonoBehaviour
    {
        private Coroutine healthCoroutine;
        private int currentHealth;
        private Player player;
        private HealthSystem healthSystem;
        private HealController healController;
        
        [SerializeField] private RectTransform FilledSpotsParent;
        [SerializeField] private GameObject FilledHealthPrefab;
        [SerializeField] private RectTransform EmptySpotsParent;
        [SerializeField] private GameObject EmptyHealthPrefab;
        [SerializeField] private float DelayToInstantiate = 0.1f;
        
        [Header("Healing Charges")]
        [SerializeField] private Element Amulet;
        [SerializeField] private TextMeshProUGUI HealingChargesText;
        
        private void OnEnable()
        {
            player = FindObjectOfType<Player>();

            if (player == null) 
                return;
            
            healthSystem = player.GetComponent<HealthSystem>();
            healController = player.GetComponent<HealController>();

            if (healthSystem != null)
            {
                healthSystem.HealthValueChanged += UpdateHealthValue;

                UpdateHealthValue();
            }
            
            if (healController != null)
            {
                healController.HealingValueChanged += UpdateHealingValue;
                healController.NoHealingAvailable += NoHealingChargesAvailable;
            }
        }

        private void OnDisable()
        {
            if (healthSystem != null)
            {
                healthSystem.HealthValueChanged -= UpdateHealthValue;
            }

            if (healController != null)
            {
                healController.HealingValueChanged -= UpdateHealingValue;
                healController.NoHealingAvailable -= NoHealingChargesAvailable;
            }
        }

        private void CreateEmptySpots()
        {
            for (int i = 0; i < healthSystem.GetMaxHealth(); i++)
            {
                Instantiate(EmptyHealthPrefab, EmptySpotsParent);
            }
        }

        private void UpdateHealingValue()
        {
            HealingChargesText.text = healController.GetHealingValue().ToString();

            var element = HealingChargesText.GetComponent<Element>();
            
            if(element)
                element.UpdateInfo();
        }

        private void NoHealingChargesAvailable()
        {
            Amulet.Disable();
        }

        private void UpdateHealthValue()
        {
            if (healthSystem == null)
                return;
            
            if(EmptySpotsParent.childCount == 0)
                CreateEmptySpots();

            if (healthSystem.GetHealth() > currentHealth)
            {
                if(healthCoroutine != null)
                    StopCoroutine(healthCoroutine);
                
                healthCoroutine = StartCoroutine(AddFilledHealth());
            }
            else
            {
                if(healthCoroutine != null)
                    StopCoroutine(healthCoroutine);
                
                healthCoroutine = StartCoroutine(RemoveFilledHealth());
                
            }

            currentHealth = healthSystem.GetHealth();
        }

        private IEnumerator AddFilledHealth()
        {
            for (int i = currentHealth; i < healthSystem.GetHealth(); i++)
            {
                Instantiate(FilledHealthPrefab, FilledSpotsParent);
                
                yield return new WaitForSeconds(DelayToInstantiate);
            }

            yield return null;
        }

        private IEnumerator RemoveFilledHealth()
        {
            if (FilledSpotsParent.childCount > 0)
            {
                for (int i = currentHealth - 1; i >= healthSystem.GetHealth(); i--)
                {
                    if (FilledSpotsParent.GetChild(i))
                    {
                        var uiElement = FilledSpotsParent.GetChild(i).GetComponent<Element>();
                
                        if(uiElement)
                            uiElement.Disable();
                
                        yield return new WaitForSeconds(DelayToInstantiate);
                    }
                }
            }
 
            yield return null;
        }
    }
}
