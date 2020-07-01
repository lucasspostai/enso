using System;
using System.Collections;
using Enso.Characters.Player;
using TMPro;
using UnityEngine;

namespace Enso.UI.Shop
{
    public class ShopController : Element
    {
        private bool isPlayerNull;
        private Coroutine disableObjectCoroutine;
        private Player player;

        [Header("References")] 
        [SerializeField] private ShopProperties Properties;
        [SerializeField] private Element ExperienceAmountElement;
        [SerializeField] private TextMeshProUGUI ExperienceAmountText;

        [Header("References")] 
        [SerializeField] private ShopOption ExtraHealthShopOption;
        [SerializeField] private ShopOption ExtraHealingChargeShopOption;
        [SerializeField] private ShopOption ExtraBalanceShopOption;
        [SerializeField] private ShopOption StrongAttackShopOption;
        [SerializeField] private ShopOption SpecialAttackShopOption;

        private void OnEnable()
        {
            player = FindObjectOfType<Player>();

            isPlayerNull = player == null;
            
            if(disableObjectCoroutine != null)
                StopCoroutine(disableObjectCoroutine);
            
            UpdateButtonsInfo();
        }

        private void UpdateButtonsInfo()
        {
            ExperienceAmountText.text = ExperienceManager.Instance.PerksAvailable.ToString();
            
            ExtraHealthShopOption.SetProperties(
                HasEnoughPerks(Properties.ExtraHealthCost),
                Properties.ExtraHealthCost, 
                player.GetHealthSystem().GetMaxHealth() >= Properties.MaxHealth);

            ExtraHealingChargeShopOption.SetProperties(
                HasEnoughPerks(Properties.ExtraHealingChargeCost),
                Properties.ExtraHealingChargeCost,
                player.HealController.GetMaxHealingCharges() >= Properties.MaxHealingCharges);

            ExtraBalanceShopOption.SetProperties(
                HasEnoughPerks(Properties.ExtraBalanceCost),
                Properties.ExtraBalanceCost,
                player.GetBalanceSystem().GetMaxBalance() >= Properties.MaxBalance);

            StrongAttackShopOption.SetProperties(
                HasEnoughPerks(Properties.StrongAttackCost),
                Properties.StrongAttackCost,
                player.AttackController.StrongAttackUnlocked);

            SpecialAttackShopOption.SetProperties(
                HasEnoughPerks(Properties.SpecialAttackCost),
                Properties.SpecialAttackCost,
                player.AttackController.SpecialAttackUnlocked);
        }

        public void BuyExtraHealth()
        {
            if (HasEnoughPerks(Properties.ExtraHealthCost))
            {
                player.GetHealthSystem().IncreaseMaxHealth();
                ExperienceManager.Instance.UsePerk(Properties.ExtraHealthCost);

                UpdateButtonsInfo();
            }
        }

        public void BuyExtraHealingCharge()
        {
            if (HasEnoughPerks(Properties.ExtraHealingChargeCost))
            {
                player.HealController.IncreaseMaxHealingCharges();
                ExperienceManager.Instance.UsePerk(Properties.ExtraHealingChargeCost);
                
                UpdateButtonsInfo();
            }
        }

        public void BuyExtraBalance()
        {
            if (HasEnoughPerks(Properties.ExtraBalanceCost))
            {
                player.GetBalanceSystem().IncreaseMaxBalance();
                ExperienceManager.Instance.UsePerk(Properties.ExtraBalanceCost);
                
                UpdateButtonsInfo();
            }
        }

        public void BuyStrongAttack()
        {
            if (HasEnoughPerks(Properties.StrongAttackCost))
            {
                player.AttackController.StrongAttackUnlocked = true;
                ExperienceManager.Instance.UsePerk(Properties.StrongAttackCost);
                
                UpdateButtonsInfo();
            }
        }

        public void BuySpecialAttack()
        {
            if (HasEnoughPerks(Properties.SpecialAttackCost))
            {
                player.AttackController.SpecialAttackUnlocked = true;
                ExperienceManager.Instance.UsePerk(Properties.SpecialAttackCost);
                
                UpdateButtonsInfo();
            }
        }

        private bool HasEnoughPerks(int cost)
        {
            bool hasEnoughPerks = ExperienceManager.Instance.PerksAvailable - cost >= 0;

            if (!hasEnoughPerks)
                NotEnoughPerks();

            return !isPlayerNull && hasEnoughPerks;
        }

        private void NotEnoughPerks()
        {
            ExperienceAmountElement.Disable();
        }

        public override void Disable()
        {
            base.Disable();

            if (!gameObject.activeSelf)
                return;

            if(disableObjectCoroutine != null)
                StopCoroutine(disableObjectCoroutine);
            
            disableObjectCoroutine = StartCoroutine(WaitThenDisable());
        }

        private IEnumerator WaitThenDisable()
        {
            yield return new WaitForSeconds(0.5f);

            gameObject.SetActive(false);
        }
    }
}