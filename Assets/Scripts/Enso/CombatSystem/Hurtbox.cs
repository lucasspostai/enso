using Enso.Characters;
using Enso.Interfaces;
using UnityEngine;

namespace Enso.CombatSystem
{
    public class Hurtbox : MonoBehaviour, IDamageable
    {
        [SerializeField] private Fighter ThisFighter;
        [SerializeField] private Vector3 HurtboxSize;
        [SerializeField] private GuardController Guard;
        
        public void TakeDamage(int damageAmount)
        {
            if (Guard.IsGuarding)
            {
                ThisFighter.GetBalanceSystem().TakeDamage(damageAmount);

                if (ThisFighter.GetBalanceSystem().GetBalance() > 0)
                {
                    Guard.Block();
                }
                else
                {
                    // Lose Balance Animation
                }
            }
            else
            {
                ThisFighter.GetHealthSystem().TakeDamage(damageAmount);
                
                // Play Damage Animation
            }
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(transform.position, HurtboxSize);
        }
    }
}
