using Enso.Interfaces;
using UnityEngine;

namespace Enso.Characters
{
    [RequireComponent(typeof(HealthSystem))]
    [RequireComponent(typeof(BalanceSystem))]
    public abstract class Fighter : MonoBehaviour
    {
        private HealthSystem healthSystem;
        private BalanceSystem balanceSystem;
        
        [SerializeField] protected FighterProperties BaseProperties;

        protected virtual void Awake()
        {
            healthSystem = GetComponent<HealthSystem>();
            balanceSystem = GetComponent<BalanceSystem>();
        }

        public HealthSystem GetHealthSystem()
        {
            return healthSystem;
        }
        
        public BalanceSystem GetBalanceSystem()
        {
            return balanceSystem;
        }


        public FighterProperties GetBaseProperties()
        {
            return BaseProperties;
        }
    }
}
