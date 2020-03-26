using UnityEngine;

namespace Enso.Characters
{
    [RequireComponent(typeof(HealthSystem))]
    [RequireComponent(typeof(BalanceSystem))]
    public abstract class Fighter : MonoBehaviour, IDamageable
    {
        private HealthSystem healthSystem;
        private BalanceSystem balanceSystem;
        
        [SerializeField] protected FighterProperties BaseProperties;

        protected virtual void Awake()
        {
            healthSystem = GetComponent<HealthSystem>();
            balanceSystem = GetComponent<BalanceSystem>();
        }

        protected virtual void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                TakeDamage(2);
            }
        }

        public FighterProperties GetBaseProperties()
        {
            return BaseProperties;
        }

        public void TakeDamage(int damageAmount)
        {
            if (balanceSystem.GetBalance() > 0)
            {
                balanceSystem.TakeDamage(damageAmount);
            }
            else
            {
                healthSystem.TakeDamage(damageAmount);
            }
        }
    }
}
