using System;
using System.Collections;
using UnityEngine;

namespace Enso.Characters
{
    [RequireComponent(typeof(Fighter))]
    public sealed class BalanceSystem : MonoBehaviour
    {
        private bool canRecover;
        private Coroutine waitThenRecoverCoroutine;
        private Fighter fighter;
        private float valueOverTime;
        
        private float balance;
        private float Balance
        {
            get => balance;
            set
            {
                balance = value;

                if (balance <= 0)
                {
                    balance = 0;
                }

                if (balance > maxBalance)
                    balance = maxBalance;

                OnBalanceValueChanged();
            }
        }

        private float maxBalance;

        public event Action BalanceValueChanged;
        public event Action BreakBalance;
        public event Action LoseBalance;

        #region Unity Event Functions
        
        private void Awake()
        {
            fighter = GetComponent<Fighter>();
            maxBalance = fighter.GetBaseProperties().BalanceAmount;
            Balance = maxBalance;
        }

        private void Update()
        {
            if (canRecover && Balance <= maxBalance)
            {
                valueOverTime = Mathf.Lerp(
                    0, 
                    maxBalance,
                    Time.deltaTime / fighter.GetBaseProperties().TimeToFullyRecoverBalance);
                
                GainBalance(valueOverTime);
            }
        }
        
        #endregion
        
        #region Main Functions
        
        public float GetBalance()
        {
            return Balance;
        }

        public float GetMaxBalance()
        {
            return maxBalance;
        }

        public float GetBalancePercentage()
        {
            return Balance / maxBalance;
        }

        public void TakeDamage(int damageAmount)
        {
            Balance -= damageAmount;

            if (Balance <= 0) //Break Balance
            {
                if (waitThenRecoverCoroutine != null)
                    StopCoroutine (waitThenRecoverCoroutine);
                waitThenRecoverCoroutine = StartCoroutine(WaitThenRecover(fighter.GetBaseProperties().DelayToRecoverAfterLosingBalance));
                
                OnBreakBalance();
            }
            else //Lose Balance
            {
                if (waitThenRecoverCoroutine != null)
                    StopCoroutine (waitThenRecoverCoroutine);
                waitThenRecoverCoroutine = StartCoroutine(WaitThenRecover(fighter.GetBaseProperties().DelayToRecoverAfterDamage));

                OnLoseBalance();
            }
        }

        public void GainBalance(float balanceAmount)
        {
            Balance += balanceAmount;
            print(balanceAmount);
        }

        private IEnumerator WaitThenRecover(float delay)
        {
            canRecover = false;
            
            yield return new WaitForSeconds(delay);

            canRecover = true;
        }
        
        #endregion

        #region Delegates

        private void OnBalanceValueChanged()
        {
            BalanceValueChanged?.Invoke();
        }

        private void OnBreakBalance()
        {
            BreakBalance?.Invoke();
        }

        private void OnLoseBalance()
        {
            LoseBalance?.Invoke();
        }

        #endregion
    }
}
