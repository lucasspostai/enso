using System;
using System.Collections;
using Framework;
using UnityEngine;

namespace Enso
{
    public class ExperienceManager : Singleton<ExperienceManager>
    {
        private Coroutine increaseXpCoroutine;

        private int xpReceived;
        private int XpReceived
        {
            get => xpReceived;
            set
            {
                xpReceived = value;
                
                if (xpReceived < 0)
                    xpReceived = 0;
            }
        }

        public int MaxXp = 10;

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
            XpAmount += xpValue;

            if (XpAmount >= MaxXp)
            {
                GainPerk();
                XpAmount = 0;
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