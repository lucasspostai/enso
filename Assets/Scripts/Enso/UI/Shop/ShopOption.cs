using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Enso.UI.Shop
{
    public class ShopOption : MonoBehaviour
    {
        private static readonly int Bought = Animator.StringToHash("Bought");
        
        [SerializeField] private Animator ThisAnimator;
        [SerializeField] private TextMeshProUGUI CostText;
        
        public Button ThisButton;

        private void Awake()
        {
            ThisButton = GetComponent<Button>();
        }

        public void SetProperties(bool buttonActive, int cost, bool bought)
        {
            if (bought)
            {
                ThisButton.interactable = false;
                ThisAnimator.SetTrigger(Bought);
            }
            else
                ThisButton.interactable = buttonActive;
            
            CostText.text = cost.ToString();
        }
    }
}
