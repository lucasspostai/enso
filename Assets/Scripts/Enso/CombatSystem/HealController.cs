using System;
using System.Collections;
using UnityEngine;

namespace Enso.CombatSystem
{
    public class HealController : CustomAnimationController
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

        public event Action NoHealingAvailable;
        public event Action HealingAlreadyAtMaximum;
        public event Action HealingValueChanged;

        [SerializeField] protected ActionAnimation HealAnimation;

        protected override void Start()
        {
            base.Start();

            healingEnabled = true;
            
            maxHealingCharges = ThisFighter.GetBaseProperties().HealingCharges;
            HealingCharges = ThisFighter.GetBaseProperties().HealingCharges;
        }

        private void PlayHealAnimation()
        {
            if (ThisFighter.AnimationHandler.IsAnyAnimationDifferentThanAttackPlaying() ||
                !ThisFighter.AnimationHandler.CanCutAttackAnimation())
                return;

            CurrentCharacterAnimation = HealAnimation;

            SetAnimationPropertiesAndPlay(HealAnimation.ClipHolder, HealAnimation.AnimationFrameChecker, false);
        }

        public override void OnLastFrameEnd()
        {
            base.OnLastFrameEnd();

            Heal();
        }

        public void TryHeal()
        {
            if (!healingEnabled || HealingCharges <= 0 || ThisFighter.GetHealthSystem().CheckIfHealthIsAtMaximumValue())
                return;

            PlayHealAnimation();
        }

        private void Heal()
        {
            ThisFighter.GetHealthSystem().Heal(ThisFighter.GetHealthSystem().GetMaxHealth());

            HealingCharges--;

            if (HealingCharges < maxHealingCharges)
            {
                if (waitAndEnableHealing != null)
                    StopCoroutine(waitAndEnableHealing);

                waitAndEnableHealing = StartCoroutine(WaitAndEnableHealing());
            }
        }

        public void RechargeHealing()
        {
            HealingCharges = maxHealingCharges;
        }

        public void IncreaseMaxHealingCharges()
        {
            maxHealingCharges += 1;
        }

        public int GetMaxHealingCharges()
        {
            return maxHealingCharges;
        }

        public int GetHealingValue()
        {
            return healingCharges;
        }

        private IEnumerator WaitAndEnableHealing()
        {
            healingEnabled = false;

            yield return new WaitForSeconds(ThisFighter.GetBaseProperties().DelayToHealAgain);

            healingEnabled = true;
        }

        private void OnNoHealingAvailable()
        {
            NoHealingAvailable?.Invoke();
        }

        private void OnHealingAlreadyAtMaximum()
        {
            HealingAlreadyAtMaximum?.Invoke();
        }

        private void OnHealingValueChanged()
        {
            HealingValueChanged?.Invoke();
        }
    }
}