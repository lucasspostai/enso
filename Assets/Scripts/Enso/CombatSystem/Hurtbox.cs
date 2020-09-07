using System;
using Enso.Characters;
using Enso.Interfaces;
using UnityEngine;

namespace Enso.CombatSystem
{
    public class Hurtbox : MonoBehaviour, IDamageable
    {
        [SerializeField] private GuardController Guard;

        [HideInInspector] public Collider2D HurtboxCollider;

        public Fighter ThisFighter;

        private void OnEnable()
        {
            ThisFighter.GetHealthSystem().Death += DisableHurtbox;
        }

        private void OnDisable()
        {
            ThisFighter.GetHealthSystem().Death -= DisableHurtbox;
        }

        private void Start()
        {
            HurtboxCollider = GetComponent<Collider2D>();
        }

        public void TakeDamage(int damageAmount, Vector3 direction)
        {
            ThisFighter.AnimationHandler.SetFacingDirection((direction * -1)
                .normalized); //Opposite direction to damage dealer

            if (Guard && (Guard.IsGuarding || Guard.StartingGuard))
            {
                ThisFighter.GetBalanceSystem().TakeDamage(damageAmount);

                if (ThisFighter.GetBalanceSystem().GetBalance() > 0)
                {
                    Guard.Block();
                }
            }
            else
            {
                ThisFighter.GetHealthSystem().TakeDamage(damageAmount);
            }
        }

        private void DisableHurtbox()
        {
            gameObject.SetActive(false);
        }
    }
}