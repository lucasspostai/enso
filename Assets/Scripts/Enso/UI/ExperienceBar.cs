using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Enso.UI
{
    public class ExperienceBar : Element
    {
        private bool playingPerkAnimation;
        private Coroutine updateThenStopCoroutine;
        private Coroutine receivePerkCoroutine;
        private float angle;

        [Header("Enso")] [SerializeField] private string GainPerkHash = "GainPerk";
        [SerializeField] private Image EnsoImage;
        [SerializeField] private RectTransform EnsoFillPivot;
        [SerializeField] private float TimeToFill = 2f;

        [Header("Perk")] [SerializeField] private TextMeshProUGUI PerkText;

        private void OnEnable()
        {
            if (ExperienceManager.Instance != null)
            {
                ExperienceManager.Instance.XpValueChanged += FillCircleValue;
                ExperienceManager.Instance.PerkReceived += ReceivePerk;
                FillCircleValue();
            }
        }

        private void OnDisable()
        {
            if (ExperienceManager.Instance != null)
            {
                ExperienceManager.Instance.XpValueChanged -= FillCircleValue;
                ExperienceManager.Instance.PerkReceived -= ReceivePerk;
            }
        }

        private void Update()
        {
            if (ExperienceManager.Instance.XpAmount > 0)
            {
                EnsoImage.fillAmount = Mathf.Clamp01(
                    Mathf.Lerp(
                        EnsoImage.fillAmount,
                        (float) ExperienceManager.Instance.XpAmount / ExperienceManager.Instance.MaxXp,
                        Time.deltaTime / TimeToFill));

                angle = EnsoImage.fillAmount * -360f + 1;
                EnsoFillPivot.localEulerAngles = new Vector3(0, 0, angle);
            }
        }

        private void FillCircleValue()
        {
            if (updateThenStopCoroutine != null)
                StopCoroutine(updateThenStopCoroutine);

            updateThenStopCoroutine = StartCoroutine(UpdateThenStop());
        }

        private IEnumerator UpdateThenStop()
        {
            if (!IsEnabled)
                Enable();

            yield return new WaitForSeconds(TimeToFill);

            UpdateInfo();

            yield return new WaitForSeconds(5f);

            if (!playingPerkAnimation)
                Disable();
        }

        private void GainPerk()
        {
            playingPerkAnimation = true;

            if (receivePerkCoroutine != null)
                StopCoroutine(receivePerkCoroutine);

            receivePerkCoroutine = StartCoroutine(PlayPerkAnimation());
        }

        private IEnumerator PlayPerkAnimation()
        {
            SetTrigger(GainPerkHash);

            PerkText.text = ExperienceManager.Instance.PerksAvailable.ToString();

            yield return new WaitForSeconds(1f);

            playingPerkAnimation = false;

            FillCircleValue();
        }

        private void ReceivePerk()
        {
            GainPerk();
        }
    }
}