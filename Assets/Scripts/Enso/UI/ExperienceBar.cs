using UnityEngine;
using UnityEngine.UI;

namespace Enso.UI
{
    public class ExperienceBar : Element
    {
        private float angle;

        [SerializeField] private Image EnsoImage;
        [SerializeField] private RectTransform EnsoFillPivot;

        private void OnEnable()
        {
            if (ExperienceManager.Instance != null)
                ExperienceManager.Instance.XpValueChanged += FillCircleValue;
        }

        private void OnDisable()
        {
            if (ExperienceManager.Instance != null)
                ExperienceManager.Instance.XpValueChanged += FillCircleValue;
        }

        protected override void Start()
        {
            base.Start();

            FillCircleValue();
        }

        private void FillCircleValue()
        {
            print(ExperienceManager.Instance.XpAmount);

            if (!ExperienceManager.Instance)
                return;

            EnsoImage.fillAmount = (float) ExperienceManager.Instance.XpAmount / ExperienceManager.Instance.MaxXp;

            angle = EnsoImage.fillAmount * -360f + 1;
            EnsoFillPivot.localEulerAngles = new Vector3(0, 0, angle);
        }
    }
}