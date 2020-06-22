using System;
using UnityEngine;
using UnityEngine.UI;

namespace Enso.UI
{
    public class ExperienceBar : Element
    {
        private float angle;
        
        [SerializeField] private Image EnsoImage;
        [SerializeField] private RectTransform EnsoEnd;
        
        private void Update()
        {
            FillCircleValue();
        }

        private void FillCircleValue()
        {
            angle = EnsoImage.fillAmount * -360f + 1;
            
            EnsoEnd.localEulerAngles = new Vector3(0, 0, angle);
        }
    }
}
