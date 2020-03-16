using UnityEngine;

namespace Gameplay
{
    public class Fighter : MonoBehaviour, IDamageable
    {
        private bool canBeHurt;
    
        [HideInInspector] public float InvincibilityTime;
        [HideInInspector] public int Health;
        [HideInInspector] public int Balance;

        private void Update()
        {
            InvincibilityTime -= Time.deltaTime;
        }

        public virtual void TakeDamage(int damage)
        {
            /*if (!canBeHurt)
            return;*/

            Health -= damage;

            if (Health <= 0)
            {
                Health = 0;
                Death();
            }
        }

        private void Death()
        {
            Destroy(gameObject);
        }

        public void UpdateBalance(int damage)
        {
            Balance -= damage;

            if (Balance <= 0)
            {
                Balance = 0;
                canBeHurt = true;
            }
        }
    }
}
