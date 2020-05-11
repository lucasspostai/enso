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
    public abstract class AttackController : MonoBehaviour, IHitboxResponder, IFrameCheckHandler
    {
        private AnimationClipHolder attackAnimationClipHolder;
        private bool mustMove;
        private CharacterMovement characterMovement;
        private Fighter thisFighter;
        private FrameChecker attackFrameChecker;
        private int currentDamage;
        private List<Hurtbox> damagedHurtboxes = new List<Hurtbox>();

        protected Attack CurrentAttack;

        [HideInInspector] public bool IsAttackAnimationPlaying;
        [HideInInspector] public bool CanCutAnimation;
        
        [SerializeField] private Hitbox AttackHitbox;

        protected virtual void Start()
        {
            thisFighter = GetComponent<Fighter>();
            characterMovement = thisFighter.GetComponent<CharacterMovement>();
            
            AttackHitbox.SetHitboxResponder(this);
            
            ResetAllProperties();
        }

        protected virtual void Update()
        {
            if (!IsAttackAnimationPlaying)
                return;
            
            attackFrameChecker.CheckFrames();

            if (mustMove)
                characterMovement.Move(characterMovement.CurrentDirection * (CurrentAttack.AnimationFrameChecker.MovementOffset * Time.deltaTime));
        }

        private void SetAnimationProperties(AnimationClipHolder animationClipHolder, FrameChecker frameChecker, Vector3 hitboxSize, int damage)
        {
            attackAnimationClipHolder = animationClipHolder;
            attackFrameChecker = frameChecker;
            
            AttackHitbox.SetHitBoxSize(hitboxSize);

            currentDamage = damage;
            
            attackAnimationClipHolder.Initialize(thisFighter.Animator);
            attackFrameChecker.Initialize(this, attackAnimationClipHolder);

            IsAttackAnimationPlaying = true;
        }
        
        protected void StartAttack(Attack attack)
        {
            if (!CanCutAnimation && IsAttackAnimationPlaying)
                return;

            CanCutAnimation = false;

            CurrentAttack = attack;
            
            SetAnimationProperties(CurrentAttack.ClipHolder, CurrentAttack.AnimationFrameChecker, CurrentAttack.HitboxSize, CurrentAttack.Damage);
            thisFighter.Animator.Play(CurrentAttack.ClipHolder.AnimatorStateName);
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

        public void OnPlayAudio()
        {
            AudioManager.Instance.Play(attackFrameChecker.AnimationSoundCue, transform.position, transform.rotation);
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
        }

        public virtual void OnCanCutAnimation()
        {
            if (!CurrentAttack.CanBeCut)
                return;
            
            CanCutAnimation = true;
        }

        public void OnStartMovement()
        {
            mustMove = true;
        }

        public void OnEndMovement()
        {
            mustMove = false;
        }

        public virtual void OnLastFrameStart()
        {
            
        }

        public virtual void OnLastFrameEnd()
        {
            ResetAllProperties();
        }

        public void OnInterrupted()
        {
            ResetAllProperties();
        }

        private void ResetAllProperties()
        {
            IsAttackAnimationPlaying = false;
            CanCutAnimation = true;
            mustMove = false;
            CurrentAttack = null;
        }
    }
}
