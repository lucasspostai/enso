using System;
using System.Collections;
using Framework;
using UnityEngine;

namespace Enso
{
    public class ExperienceManager : Singleton<ExperienceManager>
    {
        private Coroutine increaseXpCoroutine;
        
        [SerializeField] private int MaxXp;
        
        private int xpAmount;
        public int XpAmount
        {
            get => xpAmount;
            set
            {
                xpAmount = value;

                if (xpAmount < 0)
                    xpAmount = 0;
                
                OnXpValueChanged();
            }
        }

        private int perksAvailable;
        public int PerksAvailable
        {
            get => perksAvailable;
            set
            {
                perksAvailable = value;

                if (perksAvailable < 0)
                    perksAvailable = 0;
            } 
        }

        public event Action XpValueChanged;
        public event Action PerkReceived;
        public event Action PerkUsed;
        public event Action NoPerksAvailable;
        
        private void GainPerk()
        {
            PerksAvailable++;

            OnPerkReceived();
        }

        public void GainXp(int xpValue)
        {
            if(increaseXpCoroutine != null)
                StopCoroutine(increaseXpCoroutine);

            increaseXpCoroutine = StartCoroutine(IncreaseXp(xpValue));
        }

        private IEnumerator IncreaseXp(int xpValue)
        {
            var xpGained = 0;

            while (xpGained < xpValue)
            {
                yield return  new WaitForSeconds(0.02f);

                xpGained++;
                XpAmount += xpGained;

                if (xpAmount >= MaxXp)
                {
                    GainPerk();
                    
                    yield return  new WaitForSeconds(3f);
                    
                    XpAmount = 0;
                }
            }
        }

        public void UsePerk(int cost)
        {
            if (PerksAvailable >= cost)
            {
                PerksAvailable -= cost;

                OnPerkUsed();
            }
            else
            {
                OnNoPerksAvailable();
            }
        }

        private void OnXpValueChanged()
        {
            XpValueChanged?.Invoke();
        }

        private void OnPerkReceived()
        {
            PerkReceived?.Invoke();
        }

        private void OnPerkUsed()
        {
            PerkUsed?.Invoke();
        }

        private void OnNoPerksAvailable()
        {
            NoPerksAvailable?.Invoke();
        }
    }
}
