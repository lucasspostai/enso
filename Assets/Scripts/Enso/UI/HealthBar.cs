using System;
using System.Collections;
using System.Collections.Generic;
using Enso.Characters;
using Enso.Characters.Player;
using Enso.CombatSystem;
using Enso.UI.Shop;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Enso.UI
{
    public class HealthBar : MonoBehaviour
    {
        private Coroutine healthCoroutine;
        private int currentHealth;
        private Player player;
        private HealthSystem healthSystem;
        private HealController healController;

        [SerializeField] List<Vitality> VitalityPoints = new List<Vitality>();
        [SerializeField] private float DelayToInstantiate = 0.1f;
        [SerializeField] private ShopProperties ThisShopProperties;

        [Header("Healing Charges")] [SerializeField]
        private Element Amulet;

        [SerializeField] private Image AmuletImage;
        [SerializeField] private float TimeToLerpMana;

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

        private void Update()
        {
            UpdateHealingChargesValue();
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

            // for (int i = 0; i < ThisShopProperties.MaxHealth; i++)
            // {
            //     VitalityPoints[i].Enable();
            // }

            UpdateHealthValue();
        }

        private void SetupEmptySpots()
        {
            for (int i = 0; i < VitalityPoints.Count; i++)
            {
                VitalityPoints[i].gameObject.SetActive(i < healthSystem.GetMaxHealth());
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
            for (int i = 0; i < VitalityPoints.Count; i++)
            {
                if (i < healthSystem.GetHealth())
                {
                    VitalityPoints[i].gameObject.SetActive(true);
                    VitalityPoints[i].Point.gameObject.SetActive(true);
                    VitalityPoints[i].Point.Enable();
                }
                else
                {
                    VitalityPoints[i].Point.gameObject.SetActive(false);
                }

                yield return new WaitForSeconds(DelayToInstantiate);
            }
        }

        private IEnumerator RemoveFilledHealth()
        {
            for (int i = currentHealth - 1; i >= 0; i--)
            {
                if (VitalityPoints[i] && i >= healthSystem.GetHealth())
                {
                    if (VitalityPoints[i].gameObject.activeSelf)
                    {
                        VitalityPoints[i].Point.Disable();
                    }
                }

                yield return new WaitForSeconds(DelayToInstantiate);
            }
        }

        #region Healing Charges

        private void UpdateHealingChargesValue()
        {
            float desiredValue = (float)healController.GetHealingValue() / healController.GetMaxHealingCharges();
            
            AmuletImage.fillAmount =
                Mathf.Lerp(
                    AmuletImage.fillAmount,
                    desiredValue,
                    Time.deltaTime / TimeToLerpMana);

            // var element = HealingChargesText.GetComponent<Element>();
            //
            // if (element)
            //     element.UpdateInfo();
        }

        private void NoHealingChargesAvailable()
        {
            Amulet.Disable();
        }

        #endregion
    }
}