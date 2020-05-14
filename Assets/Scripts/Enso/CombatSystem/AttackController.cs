using System.Collections.Generic;
using Enso.Characters;
using Enso.Enums;
using Enso.Interfaces;
using Framework;
using Framework.Animations;
using Framework.Audio;
using UnityEngine;

namespace Enso.CombatSystem
{
    public abstract class AttackController : CustomAnimationController, IHitboxResponder
    {
        private int currentDamage;
        private readonly List<Hurtbox> damagedHurtboxes = new List<Hurtbox>();

        [SerializeField] private Hitbox AttackHitbox;

        protected override void Start()
        {
            base.Start();

            AttackHitbox.SetHitboxResponder(this);
        }

        private void SetDamageProperties(Vector3 hitboxSize, int damage)
        {
            AttackHitbox.SetHitBoxSize(hitboxSize);
            currentDamage = damage;
        }

        protected void StartAttack(AttackAnimation attackAnimation)
        {
            if (ThisFighter.AnimationHandler.IsAnyAnimationDifferentThanAttackPlaying() ||
                !CanCutAnimation && IsAnimationPlaying)
                return;

            CanCutAnimation = false;

            CurrentCharacterAnimation = attackAnimation;

            SetDamageProperties(attackAnimation.HitboxSize, attackAnimation.Damage);
            SetAnimationPropertiesAndPlay(attackAnimation.ClipHolder, attackAnimation.AnimationFrameChecker);
        }

        public void CollidedWith(Collider2D otherCollider)
        {
            var hurtbox = otherCollider.GetComponent<Hurtbox>();

            if (hurtbox != null && !damagedHurtboxes.Contains(hurtbox))
            {
                damagedHurtboxes.Add(hurtbox);
                hurtbox.TakeDamage(currentDamage);
            }
        }

        public override void OnHitFrameStart()
        {
            damagedHurtboxes.Clear();
            AttackHitbox.SetColliderState(ColliderState.Open);
        }

        public override void OnHitFrameEnd()
        {
            damagedHurtboxes.Clear();
            AttackHitbox.SetColliderState(ColliderState.Closed);
        }
    }
}