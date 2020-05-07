using Enso.CombatSystem;
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

        public Animator Animator;
        public DamageController DamageController;

        protected virtual void Awake()
        {
            healthSystem = GetComponent<HealthSystem>();
            balanceSystem = GetComponent<BalanceSystem>();
        }

        public HealthSystem GetHealthSystem()
        {
            if(!healthSystem)
                healthSystem = GetComponent<HealthSystem>();
                
            return healthSystem;
        }
        
        public BalanceSystem GetBalanceSystem()
        {
            if(!balanceSystem)
                balanceSystem = GetComponent<BalanceSystem>();
                
            return balanceSystem;
        }

        public FighterProperties GetBaseProperties()
        {
            return BaseProperties;
        }
    }
}
