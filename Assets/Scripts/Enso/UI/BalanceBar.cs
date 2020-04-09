using Enso.Characters;
using Enso.Characters.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Enso.UI
{
    public class BalanceBar : MonoBehaviour
    {
        private Player player;
        private BalanceSystem balanceSystem;

        [SerializeField] private Slider[] BalanceSliders;

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

            if (balanceSystem != null)
            {
                balanceSystem.BalanceValueChanged += UpdateBalanceValue;
                UpdateBalanceValue();
            }
        }

        private void OnDisable()
        {
            if (balanceSystem != null)
            {
                balanceSystem.BalanceValueChanged -= UpdateBalanceValue;
            }
        }

        private void UpdateBalanceValue()
        {
            foreach (var slider in BalanceSliders)
            {
                slider.value = balanceSystem.GetBalancePercentage();
            }
        }
    }
}
