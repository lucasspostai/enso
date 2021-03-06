﻿using Enso.Characters;
using Enso.Characters.Player;
using Enso.Enums;
using Framework;
using Framework.Animations;
using Framework.Audio;
using UnityEngine;

namespace Enso.CombatSystem
{
    public class DamageController : CustomAnimationController
    {
        [HideInInspector] public bool IsDying;
        [HideInInspector] public bool IsReceivingParry;

        [Header("Animations")] [SerializeField]
        protected DamageAnimation RegularDamageAnimation;

        [SerializeField] protected DamageAnimation HeavyDamageAnimation;
        [SerializeField] protected DamageAnimation LoseBalanceAnimation;
        [SerializeField] protected DamageAnimation DeathAnimation;

        [Header("Particles")] [SerializeField] protected GameObject RegularDamageParticle;
        [SerializeField] protected GameObject HeavyDamageParticle;
        [SerializeField] protected GameObject LoseBalanceParticle;
        [SerializeField] protected GameObject DeathParticle;
        [SerializeField] protected GameObject BloodPoolParticle;
        [SerializeField] protected Transform DeathBloodPoolLocation;
        [SerializeField] protected Transform BloodPoolLocation;
        [SerializeField] protected float DeathParticleDelay;

        [Header("Camera Shake")] [SerializeField]
        protected CameraShakeProfile RegularDamageShakeProfile;

        [SerializeField] protected CameraShakeProfile HeavyDamageShakeProfile;
        [SerializeField] protected CameraShakeProfile LoseBalanceShakeProfile;
        [SerializeField] protected CameraShakeProfile DeathShakeProfile;

        [Header("Audio")] [SerializeField] protected SoundCue RegularDamageSoundCue;
        [SerializeField] protected SoundCue StrongDamageSoundCue;
        [SerializeField] protected SoundCue LoseBalanceSoundCue;
        [SerializeField] protected SoundCue ParrySoundCue;

        [Header("Properties")] [SerializeField]
        protected float DeathTimeScale = 0.5f;

        [SerializeField] protected float DeathTimeScaleDuration = 0.2f;

        private void OnEnable()
        {
            ThisFighter.GetHealthSystem().Damage += SpawnDamageParticleAndPlayAnimation;
            ThisFighter.GetHealthSystem().Death += Death;
            ThisFighter.GetBalanceSystem().BreakBalance += BreakBalance;
        }

        private void OnDisable()
        {
            ThisFighter.GetHealthSystem().Damage -= SpawnDamageParticleAndPlayAnimation;
            ThisFighter.GetHealthSystem().Death -= Death;
            ThisFighter.GetBalanceSystem().BreakBalance -= BreakBalance;
        }

        protected override void Start()
        {
            base.Start();

            if (RegularDamageParticle)
                PoolManager.Instance.CreatePool(RegularDamageParticle, 3);

            if (HeavyDamageParticle)
                PoolManager.Instance.CreatePool(HeavyDamageParticle, 3);

            if (LoseBalanceParticle)
                PoolManager.Instance.CreatePool(LoseBalanceParticle, 3);

            if (DeathParticle)
                PoolManager.Instance.CreatePool(DeathParticle, 3);

            if (BloodPoolParticle)
                PoolManager.Instance.CreatePool(BloodPoolParticle, 3);
        }

        private void PlayDamageAnimation(DamageAnimation damageAnimation)
        {
            ThisFighter.AnimationHandler.MakeCharacterFlash();
            ThisFighter.AnimationHandler.PauseAnimationForAWhile();

            CurrentCharacterAnimation = damageAnimation;

            SetAnimationPropertiesAndPlay(damageAnimation.ClipHolder, damageAnimation.AnimationFrameChecker);

            ThisFighter.GetComponent<AttackController>()?.OnInterrupted();
            ThisFighter.GetComponent<GuardController>()?.OnInterrupted();
        }

        private void SpawnDamageParticleAndPlayAnimation()
        {
            switch (ThisFighter.GetHealthSystem().CurrentAttackType)
            {
                case AttackType.Light:
                    if (!IsDying)
                    {
                        PlayDamageAnimation(RegularDamageAnimation);

                        if (RegularDamageShakeProfile)
                            PlayerCinemachineManager.Instance.ShakeController.Shake(RegularDamageShakeProfile);
                    }

                    if (RegularDamageSoundCue)
                        AudioManager.Instance.Play(RegularDamageSoundCue, transform.position, Quaternion.identity);
                    
                    PlayerInput.Instance.Rumble(1f, 0.3f);

                    SpawnParticle(RegularDamageParticle);
                    SpawnParticle(BloodPoolParticle, BloodPoolLocation);
                    break;

                case AttackType.Strong:
                    if (!IsDying)
                    {
                        PlayDamageAnimation(HeavyDamageAnimation);

                        if (HeavyDamageShakeProfile)
                            PlayerCinemachineManager.Instance.ShakeController.Shake(HeavyDamageShakeProfile);
                    }

                    if (StrongDamageSoundCue)
                        AudioManager.Instance.Play(StrongDamageSoundCue, transform.position, Quaternion.identity);
                    
                    PlayerInput.Instance.Rumble(1f, 0.5f);

                    SpawnParticle(HeavyDamageParticle);
                    SpawnParticle(BloodPoolParticle, BloodPoolLocation);
                    break;

                default:
                    PlayDamageAnimation(RegularDamageAnimation);
                    SpawnParticle(RegularDamageParticle);
                    SpawnParticle(BloodPoolParticle);
                    break;
            }
        }

        private void Death()
        {
            if (IsDying)
                return;

            IsDying = true;
            
            PlayerInput.Instance.Rumble(1f, 1f);

            if (DeathShakeProfile)
                PlayerCinemachineManager.Instance.ShakeController.Shake(DeathShakeProfile);

            PlayDamageAnimation(DeathAnimation);

            SpawnParticle(DeathParticle, DeathBloodPoolLocation, DeathParticleDelay);

            GameManager.Instance.ChangeTimeScale(DeathTimeScale, DeathTimeScaleDuration);
        }

        private void BreakBalance()
        {
            PlayDamageAnimation(LoseBalanceAnimation);

            SpawnParticle(LoseBalanceParticle);

            if (LoseBalanceSoundCue && !IsReceivingParry)
                AudioManager.Instance.Play(LoseBalanceSoundCue, transform.position, Quaternion.identity);
            else if (ParrySoundCue)
                AudioManager.Instance.Play(ParrySoundCue, transform.position, Quaternion.identity);

            if (LoseBalanceShakeProfile)
                PlayerCinemachineManager.Instance.ShakeController.Shake(LoseBalanceShakeProfile);

            IsReceivingParry = false;
        }

        public override void OnLastFrameEnd()
        {
            if (IsDying)
                return;

            base.OnLastFrameEnd();
        }

        protected override void ResetAllProperties()
        {
            base.ResetAllProperties();

            IsDying = false;
        }
    }
}