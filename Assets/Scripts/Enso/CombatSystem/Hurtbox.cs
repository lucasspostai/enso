using Enso.Characters;
using Enso.Interfaces;
using UnityEngine;

namespace Enso.CombatSystem
{
    public class Hurtbox : MonoBehaviour, IDamageable
    {
        [SerializeField] private Fighter ThisFighter;
        [SerializeField] private Vector3 HurtboxSize;
        
        public void TakeDamage(int damageAmount)
        {
            if (ThisFighter.GetBalanceSystem().GetBalance() > 0)
            {
                ThisFighter.GetBalanceSystem().TakeDamage(damageAmount);
            }
            else
            {
                ThisFighter.GetBalanceSystem().TakeDamage(damageAmount);
            }
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, HurtboxSize);
        }
    }
}
