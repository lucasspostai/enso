using System;
using System.Collections;
using UnityEngine;

namespace Enso.Characters.Player
{
    [RequireComponent(typeof(Player))]
    public class PlayerHeal : MonoBehaviour
    {
        private bool healingEnabled;
        private Coroutine waitAndEnableHealing;

        private int healingCharges;

        private int HealingCharges
        {
            get
            {
                if (healingCharges <= 0)
                {
                    OnNoHealingAvailable();
                }
                
                return healingCharges;
            }
            set
            {
                healingCharges = value;

                if (healingCharges <= 0)
                {
                    healingCharges = 0;
                }

                if (healingCharges > maxHealingCharges)
                {
                    healingCharges = maxHealingCharges;
                    OnHealingAlreadyAtMaximum();
                }
                else
                {
                    OnHealingValueChanged();
                }
            }
        }

        private int maxHealingCharges;
        private Player player;

        public static event Action NoHealingAvailable;
        public static event Action HealingAlreadyAtMaximum;
        public static event Action HealingValueChanged;

        private void Awake()
        {
            player = GetComponent<Player>();
            maxHealingCharges = player.GetProperties().HealingCharges;
            healingCharges = player.GetProperties().HealingCharges;
            healingEnabled = true;
        }

        private void OnEnable()
        {
            PlayerInput.HealInputDown += Heal;
        }

        private void OnDisable()
        {
            PlayerInput.HealInputDown -= Heal;
        }

        private void Heal()
        {
            if (!healingEnabled)
                return;

            if (HealingCharges > 0)
            {
                player.GetHealthSystem().Heal(player.GetHealthSystem().GetMaxHealth());

                HealingCharges--;

                if (HealingCharges < maxHealingCharges)
                {
                    if (waitAndEnableHealing != null)
                        StopCoroutine(waitAndEnableHealing);
                    
                    waitAndEnableHealing = StartCoroutine(WaitAndEnableHealing());
                }
            }
        }

        private void RechargeHealing()
        {
            HealingCharges = maxHealingCharges;
        }

        private IEnumerator WaitAndEnableHealing()
        {
            healingEnabled = false;

            yield return new WaitForSeconds(player.GetProperties().DelayToHealAgain);

            healingEnabled = true;
        }

        private static void OnNoHealingAvailable()
        {
            NoHealingAvailable?.Invoke();
        }

        private static void OnHealingAlreadyAtMaximum()
        {
            HealingAlreadyAtMaximum?.Invoke();
        }

        private static void OnHealingValueChanged()
        {
            HealingValueChanged?.Invoke();
        }
    }
}