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
        [SerializeField] private Slider[] DamageSliders;
        [SerializeField] private string LoseBalanceHash = "LoseBalance";
        [SerializeField] private string EnableSpecialAttackHash = "EnableSpecialAttack";
        [SerializeField] private float DelayToUpdateDamageSlider = 1f;
        [SerializeField] private float TimeToLerpDamageSliderValue = 0.5f;

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

        private void Update()
        {
            if (updateDamageSlider)
                SetDamageSlidersValue(Mathf.Lerp(
                    DamageSliders[0].value, 
                    BalanceSliders[0].value,
                    Time.deltaTime / TimeToLerpDamageSliderValue));
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
                slider.value = balanceSystem.GetBalancePercentage();
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

        private void SetDamageSlidersValue(float value)
        {
            foreach (var slider in DamageSliders)
            {
                slider.value = value;
            }
        }

        private IEnumerator UpdateDamageSlider()
        {
            updateDamageSlider = false;

            yield return new WaitForSeconds(DelayToUpdateDamageSlider);

            updateDamageSlider = true;
        }
    }
}