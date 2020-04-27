using System;
using System.Collections.Generic;
using Enso.Characters;
using Enso.Enums;
using Enso.Interfaces;
using Framework.Animations;
using UnityEngine;

namespace Enso.CombatSystem
{
    public abstract class AttackController : MonoBehaviour, IHitboxResponder, IFrameCheckHandler
    {
        private int currentDamage;
        private AnimationClipHolder attackAnimationClipHolder;
        private FrameChecker attackFrameChecker;
        private List<Hurtbox> damagedHurtboxes = new List<Hurtbox>();

        protected Attack CurrentAttack;
        protected bool MustMove;
        protected Fighter ThisFighter;

        [HideInInspector] public bool IsAttackAnimationPlaying;
        [HideInInspector] public bool CanCutAnimation;
        
        [SerializeField] private Hitbox AttackHitbox;

        protected virtual void Start()
        {
            ThisFighter = GetComponent<Fighter>();
            AttackHitbox.SetHitboxResponder(this);
            
            ResetAllProperties();
        }

        protected virtual void Update()
        {
            if (!IsAttackAnimationPlaying)
                return;
            
            attackFrameChecker.CheckFrames();
        }

        private void SetAnimationProperties(AnimationClipHolder animationClipHolder, FrameChecker frameChecker, Vector3 hitboxSize, int damage)
        {
            attackAnimationClipHolder = animationClipHolder;
            attackFrameChecker = frameChecker;
            
            AttackHitbox.SetHitBoxSize(hitboxSize);

            currentDamage = damage;
            
            attackAnimationClipHolder.Initialize(ThisFighter.Animator);
            attackFrameChecker.Initialize(this, attackAnimationClipHolder);
            
            MustMove = true;
            IsAttackAnimationPlaying = true;
        }
        
        protected void StartAttack(Attack attack)
        {
            if (!CanCutAnimation && IsAttackAnimationPlaying)
                return;

            CanCutAnimation = false;

            CurrentAttack = attack;
            
            SetAnimationProperties(CurrentAttack.AttackAnimationClipHolder, CurrentAttack.AttackFrameChecker, CurrentAttack.HitboxSize, CurrentAttack.Damage);
            ThisFighter.Animator.Play(CurrentAttack.AttackAnimationClipHolder.AnimatorStateName);
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

        protected virtual void Move()
        {
            
        }

        public virtual void OnHitFrameStart()
        {
            damagedHurtboxes.Clear();
            AttackHitbox.SetColliderState(ColliderState.Open);
        }

        public virtual void OnHitFrameEnd()
        {
            damagedHurtboxes.Clear();
            AttackHitbox.SetColliderState(ColliderState.Closed);

            MustMove = false;
        }

        public virtual void OnCanCutAnimation()
        {
            if (!CurrentAttack.CanBeCut)
                return;
            
            CanCutAnimation = true;
        }

        public virtual void OnLastFrameStart()
        {
            
        }

        public virtual void OnLastFrameEnd()
        {
            ResetAllProperties();
        }

        private void ResetAllProperties()
        {
            IsAttackAnimationPlaying = false;
            CanCutAnimation = true;
            MustMove = false;
            CurrentAttack = null;
        }
    }
}
