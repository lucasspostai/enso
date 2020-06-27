using System.Collections.Generic;
using Enso.Characters;
using Enso.Characters.Player;
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
        private Transform riposteCharacterTransform;
        private bool isHitboxNull;
        private int currentDamage;
        private readonly List<Hurtbox> damagedHurtboxes = new List<Hurtbox>();

        [SerializeField] private Hitbox AttackHitbox;

        [Header("Particles")] 
        [SerializeField] protected GameObject ParryParticle;
        [SerializeField] protected GameObject RiposteParticle;

        [Header("Camera Shake")] 
        [SerializeField] protected CameraShakeProfile ParryShakeProfile;
        [SerializeField] protected CameraShakeProfile RiposteShakeProfile;
        [SerializeField] protected CameraShakeProfile AttackShakeProfile;

        protected override void Start()
        {
            base.Start();

            isHitboxNull = AttackHitbox == null;

            if (!isHitboxNull)
                AttackHitbox.SetHitboxResponder(this);

            if (ParryParticle)
                PoolManager.Instance.CreatePool(ParryParticle, 1);

            if (RiposteParticle)
                PoolManager.Instance.CreatePool(RiposteParticle, 1);
        }

        private void SetDamageProperties(Vector3 hitboxSize, int damage)
        {
            if (isHitboxNull)
                return;

            AttackHitbox.SetHitBoxSize(hitboxSize);
            currentDamage = damage;
        }

        protected void StartAttack(AttackAnimation attackAnimation)
        {
            if (ThisFighter.AnimationHandler.IsAnyAnimationDifferentThanAttackPlaying() &&
                !ThisFighter.AnimationHandler.IsAnyGuardAnimationPlaying() ||
                !CanCutAnimation && IsAnimationPlaying)
                return;

            CanCutAnimation = false;

            CurrentCharacterAnimation = attackAnimation;

            ThisFighter.AnimationHandler.InterruptAllGuardAnimations();

            SetDamageProperties(attackAnimation.HitboxSize, attackAnimation.Damage);
            SetAnimationPropertiesAndPlay(attackAnimation.ClipHolder, attackAnimation.AnimationFrameChecker);
        }

        public void CollidedWith(Collider2D otherCollider)
        {
            var hurtbox = otherCollider.GetComponent<Hurtbox>();

            if (hurtbox != null && !damagedHurtboxes.Contains(hurtbox) &&
                hurtbox.ThisFighter.FighterTeam != ThisFighter.FighterTeam)
            {
                damagedHurtboxes.Add(hurtbox);

                var guardController = hurtbox.ThisFighter.GetComponent<GuardController>();

                ThisFighter.AnimationHandler.PauseAnimationForAWhile();

                if (guardController && guardController.IsParrying)
                {
                    var attackController = hurtbox.ThisFighter.GetComponent<AttackController>();

                    if (attackController)
                        attackController.SetRipostePosition(transform);

                    ThisFighter.GetBalanceSystem()
                        .TakeDamage(Mathf.RoundToInt(ThisFighter.GetBalanceSystem().GetMaxBalance()));

                    guardController.OnParryHit(guardController.transform.position -
                                               ThisFighter.transform.position);

                    SpawnParticle(ParryParticle);

                    if(ParryShakeProfile)
                        PlayerCinemachineManager.Instance.ShakeController.Shake(ParryShakeProfile);

                    GameManager.Instance.ChangeTimeScale(0.5f, 1f);
                }
                else
                {
                    hurtbox.TakeDamage(currentDamage, ThisFighter.AnimationHandler.CurrentDirection);
                    OnCollision();
                }
            }
        }

        public override void OnHitFrameStart()
        {
            if(AttackShakeProfile)
                PlayerCinemachineManager.Instance.ShakeController.Shake(AttackShakeProfile);

            if (isHitboxNull)
                return;

            damagedHurtboxes.Clear();
            AttackHitbox.SetColliderState(ColliderState.Open);
        }

        public override void OnHitFrameEnd()
        {
            if (isHitboxNull)
                return;

            damagedHurtboxes.Clear();
            AttackHitbox.SetColliderState(ColliderState.Closed);
        }

        protected virtual void OnCollision()
        {
            ThisFighter.AnimationHandler.PauseAnimationForAWhile();
        }

        public void SetRipostePosition(Transform characterTransform)
        {
            riposteCharacterTransform = characterTransform;
        }

        protected void PlayRiposteParticle()
        {
            SpawnParticle(RiposteParticle, riposteCharacterTransform);

            if(AttackShakeProfile)
                PlayerCinemachineManager.Instance.ShakeController.Shake(RiposteShakeProfile);
        }
    }
}