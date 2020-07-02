using System;
using System.Collections;
using System.Collections.Generic;
using Enso.Characters;
using Enso.Characters.Player;
using Enso.CombatSystem;
using Enso.UI.Shop;
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
        private List<Element> filledSpots = new List<Element>();
        private List<GameObject> emptySpots = new List<GameObject>();

        [SerializeField] private RectTransform FilledSpotsParent;
        [SerializeField] private RectTransform EmptySpotsParent;
        [SerializeField] private GameObject FilledHealthPrefab;
        [SerializeField] private GameObject EmptyHealthPrefab;
        [SerializeField] private float DelayToInstantiate = 0.1f;
        [SerializeField] private ShopProperties ThisShopProperties;

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

            SetupHealth();
        }

        private void OnDisable()
        {
            if (healthSystem != null)
            {
                healthSystem.HealthValueChanged -= UpdateHealthValue;
            }

            if (healController != null)
            {
                healController.HealingValueChanged -= UpdateHealingChargesValue;
                healController.NoHealingAvailable -= NoHealingChargesAvailable;
            }
        }

        public void SetupHealth()
        {
            if (healthSystem != null)
            {
                healthSystem.HealthValueChanged += UpdateHealthValue;
            }

            if (healController != null)
            {
                healController.HealingValueChanged += UpdateHealingChargesValue;
                healController.NoHealingAvailable += NoHealingChargesAvailable;
            }
            
            currentHealth = 0;
            
            filledSpots.Clear();
            emptySpots.Clear();
            
            ClearParent(FilledSpotsParent);
            ClearParent(EmptySpotsParent);
            
            for (int i = 0; i < ThisShopProperties.MaxHealth; i++)
            {
                //Filled
                var filledHealth = Instantiate(FilledHealthPrefab, FilledSpotsParent);
                filledHealth.SetActive(false);
                
                var element = filledHealth.GetComponent<Element>();
                
                if(element != null)
                    filledSpots.Add(element);
                
                //Empty
                var emptyHealth = Instantiate(EmptyHealthPrefab, EmptySpotsParent);
                emptyHealth.SetActive(false);
                
                emptySpots.Add(emptyHealth);
            }
            
            UpdateHealthValue();
        }

        private void ClearParent(Transform parentTransform)
        {
            foreach (Transform child in parentTransform) {
                Destroy(child.gameObject);
            }
        }

        private void SetupEmptySpots()
        {
            for (int i = 0; i < emptySpots.Count; i++)
            {
                emptySpots[i].SetActive(i < healthSystem.GetMaxHealth());
            }
        }

        private void UpdateHealthValue()
        {
            SetupEmptySpots();
            
            if (healthSystem.GetHealth() == currentHealth)
                return;
            
            if (healthSystem.GetHealth() > currentHealth)
            {
                IncreaseHealth();
            }
            else
            {
                DecreaseHealth();
            }

            currentHealth = healthSystem.GetHealth();
        }

        private void IncreaseHealth()
        {
            if (healthCoroutine != null)
                StopCoroutine(healthCoroutine);

            healthCoroutine = StartCoroutine(AddFilledHealth());
        }
        
        private void DecreaseHealth()
        {
            if (healthCoroutine != null)
                StopCoroutine(healthCoroutine);
                
            healthCoroutine = StartCoroutine(RemoveFilledHealth());
        }

        private IEnumerator AddFilledHealth()
        {
            for (int i = 0; i < filledSpots.Count; i++)
            {
                if (i < healthSystem.GetHealth())
                {
                    if (!filledSpots[i].gameObject.activeSelf)
                    {
                        filledSpots[i].gameObject.SetActive(true);
                        filledSpots[i].Enable();
                    }
                }
                else
                {
                    if (filledSpots[i].gameObject.activeSelf)
                    {
                        filledSpots[i].gameObject.SetActive(false);
                    }
                }
                
                yield return new WaitForSeconds(DelayToInstantiate);
            }
        }

        private IEnumerator RemoveFilledHealth()
        {
            for (int i = currentHealth - 1; i >= 0; i--)
            {
                if (filledSpots[i] && i >= healthSystem.GetHealth())
                {
                    if (filledSpots[i].gameObject.activeSelf)
                    {
                        filledSpots[i].Disable();
                    }
                }
                
                yield return new WaitForSeconds(DelayToInstantiate);
            }
        }

        #region Healing Charges

        private void UpdateHealingChargesValue()
        {
            HealingChargesText.text = healController.GetHealingValue().ToString();

            var element = HealingChargesText.GetComponent<Element>();

            if (element)
                element.UpdateInfo();
        }

        private void NoHealingChargesAvailable()
        {
            Amulet.Disable();
        }

        #endregion
    }
}