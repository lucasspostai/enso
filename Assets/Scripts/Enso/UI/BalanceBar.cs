using System;
using System.Collections;
using Enso.Characters;
using Enso.Characters.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Enso.UI
{
    public class BalanceBar : Element
    {
        private bool updateDamageSlider;
        private bool isEnabled;
        private Coroutine damageSliderCoroutine;
        private Player player;
        private BalanceSystem balanceSystem;

        [SerializeField] private Slider[] BalanceSliders;
        [SerializeField] private string LoseBalanceHash = "LoseBalance";
        [SerializeField] private string EnableSpecialAttackHash = "EnableSpecialAttack";
        [SerializeField] private float DelayToUpdateDamageSlider = 1f;

        private void OnEnable()
        {
            foreach (var slider in BalanceSliders)
            {
                slider.maxValue = 1;
            }

            player = FindObjectOfType<Player>();

            if (player == null)
                return;

            balanceSystem = player.GetComponent<BalanceSystem>();

            isEnabled = true;

            if (balanceSystem != null)
            {
                balanceSystem.BalanceValueChanged += UpdateBalanceValue;
                balanceSystem.LoseBalance += LoseBalance;
                balanceSystem.RecoverBalance += Enable;
                balanceSystem.BreakBalance += Disable;
                balanceSystem.EnableSpecialAttack += EnableSpecialAttack;

                UpdateBalanceValue();
            }
        }

        private void OnDisable()
        {
            if (balanceSystem != null)
            {
                balanceSystem.BalanceValueChanged -= UpdateBalanceValue;
                balanceSystem.LoseBalance -= LoseBalance;
                balanceSystem.RecoverBalance -= Enable;
                balanceSystem.BreakBalance -= Disable;
                balanceSystem.EnableSpecialAttack -= EnableSpecialAttack;
            }
        }

        private void UpdateBalanceValue()
        {
            foreach (var slider in BalanceSliders)
            {
                slider.value = Mathf.InverseLerp(1, 0, balanceSystem.GetBalancePercentage());
            }
        }

        private void LoseBalance()
        {
            if(isEnabled)
                SetTrigger(LoseBalanceHash);

            if (damageSliderCoroutine != null)
                StopCoroutine(damageSliderCoroutine);

            damageSliderCoroutine = StartCoroutine(UpdateDamageSlider());
        }

        private void EnableSpecialAttack()
        {
            SetTrigger(EnableSpecialAttackHash);
        }

        private IEnumerator UpdateDamageSlider()
        {
            updateDamageSlider = false;

            yield return new WaitForSeconds(DelayToUpdateDamageSlider);

            updateDamageSlider = true;
        }
    }
}