using System;
using Enso.Characters;
using Enso.Interfaces;
using UnityEngine;

namespace Enso.CombatSystem
{
    public class Hurtbox : MonoBehaviour, IDamageable
    {
        [SerializeField] private Fighter ThisFighter;
        [SerializeField] private GuardController Guard;

        [HideInInspector] public Collider2D HurtboxCollider;

        private void Start()
        {
            HurtboxCollider = GetComponent<Collider2D>();
        }

        public void TakeDamage(int damageAmount, Vector3 direction)
        {
            ThisFighter.AnimationHandler.SetFacingDirection((direction * -1).normalized); //Opposite direction to damage dealer

            if (Guard && Guard.IsGuarding)
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
            }
        }
    }
}