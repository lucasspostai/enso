using System.Collections;
using Enso.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Enso.Characters.Enemies
{
    public class EnemyBalanceBar : Element
    {
        private bool updateDamageSlider;
        private bool isEnabled;
        private Coroutine damageSliderCoroutine;
        private BalanceSystem balanceSystem;
        private HealthSystem healthSystem;

        [SerializeField] private Fighter ThisFighter;
        [SerializeField] private Image[] BalanceImages;
        [SerializeField] private Image[] DamageImages;
        [SerializeField] private string LoseBalanceHash = "LoseBalance";
        [SerializeField] private string EnableSpecialAttackHash = "EnableSpecialAttack";
        [SerializeField] private float DelayToUpdateDamageSlider = 1f;
        [SerializeField] private float TimeToLerpDamageSliderValue = 0.5f;

        private void OnEnable()
        {
            foreach (var slider in BalanceImages)
            {
                slider.fillAmount = 1;
            }

            if (ThisFighter == null)
                return;

            balanceSystem = ThisFighter.GetComponent<BalanceSystem>();
            healthSystem = ThisFighter.GetComponent<HealthSystem>();

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

            if (healthSystem != null)
            {
                healthSystem.Death += Disable;
            }
        }

        private void Update()
        {
            if (healthSystem.IsDead)
                return;
            
            if (updateDamageSlider)
                SetDamageSlidersValue(Mathf.Lerp(
                    DamageImages[0].fillAmount, 
                    BalanceImages[0].fillAmount,
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
            
            if (healthSystem != null)
            {
                healthSystem.Death -= Death;
            }
        }

        private void Death()
        {
            OnDisable();  
        }

        private void UpdateBalanceValue()
        {
            if (healthSystem.IsDead)
                return;
            
            foreach (var image in BalanceImages)
            {
                image.fillAmount = balanceSystem.GetBalancePercentage();
            }
        }

        private void LoseBalance()
        {
            if (healthSystem.IsDead)
                return;
            
            if(isEnabled)
                SetTrigger(LoseBalanceHash);

            if (damageSliderCoroutine != null)
                StopCoroutine(damageSliderCoroutine);

            damageSliderCoroutine = StartCoroutine(UpdateDamageSlider());
        }

        private void EnableSpecialAttack()
        {
            if (healthSystem.IsDead)
                return;
            
            SetTrigger(EnableSpecialAttackHash);
        }

        private void SetDamageSlidersValue(float value)
        {
            if (healthSystem.IsDead)
                return;
            
            foreach (var image in DamageImages)
            {
                image.fillAmount = value;
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
