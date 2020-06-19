using Enso.Characters;
using Enso.Characters.Player;
using Enso.Enums;
using Framework;
using Framework.Animations;
using UnityEngine;

namespace Enso.CombatSystem
{
    public class DamageController : CustomAnimationController
    {
        [HideInInspector] public bool IsDying;

        [Header("Animations")]
        [SerializeField] protected DamageAnimation RegularDamageAnimation;
        [SerializeField] protected DamageAnimation HeavyDamageAnimation;
        [SerializeField] protected DamageAnimation LoseBalanceAnimation;
        [SerializeField] protected DamageAnimation DeathAnimation;
        
        [Header("Particles")]
        [SerializeField] protected GameObject RegularDamageParticle;
        [SerializeField] protected GameObject HeavyDamageParticle;
        [SerializeField] protected GameObject LoseBalanceParticle;
        [SerializeField] protected GameObject DeathParticle;
        
        [Header("Camera Shake")]
        [SerializeField] protected CameraShakeProfile RegularDamageShakeProfile;
        [SerializeField] protected CameraShakeProfile HeavyDamageShakeProfile;
        [SerializeField] protected CameraShakeProfile LoseBalanceShakeProfile;
        [SerializeField] protected CameraShakeProfile DeathShakeProfile;
        
        [Header("Properties")]
        [SerializeField] protected float DeathTimeScale = 0.5f;
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

            PoolManager.Instance.CreatePool(RegularDamageParticle, 3);
            PoolManager.Instance.CreatePool(HeavyDamageParticle, 3);
            PoolManager.Instance.CreatePool(LoseBalanceParticle, 3);
            PoolManager.Instance.CreatePool(DeathParticle, 3);
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
            if (IsDying)
                return;

            switch (ThisFighter.GetHealthSystem().CurrentAttackType)
            {
                case AttackType.Light:
                    PlayDamageAnimation(RegularDamageAnimation);
                    PlayerCinemachineManager.Instance.ShakeController.Shake(RegularDamageShakeProfile);
                    SpawnParticle(RegularDamageParticle);
                    break;
                case AttackType.Strong:
                    PlayDamageAnimation(HeavyDamageAnimation);
                    PlayerCinemachineManager.Instance.ShakeController.Shake(HeavyDamageShakeProfile);
                    SpawnParticle(HeavyDamageParticle);
                    break;
                default:
                    PlayDamageAnimation(RegularDamageAnimation);
                    SpawnParticle(RegularDamageParticle);
                    break;
            }
        }

        private void Death()
        {
            if (IsDying)
                return;

            IsDying = true;
            SpawnParticle(HeavyDamageParticle);
            
            PlayerCinemachineManager.Instance.ShakeController.Shake(DeathShakeProfile);

            PlayDamageAnimation(DeathAnimation);
            
            SpawnParticle(DeathParticle);
            
            GameManager.Instance.ChangeTimeScale(DeathTimeScale, DeathTimeScaleDuration);
        }

        private void BreakBalance()
        {
            PlayDamageAnimation(LoseBalanceAnimation);
            
            SpawnParticle(LoseBalanceParticle);
            
            PlayerCinemachineManager.Instance.ShakeController.Shake(LoseBalanceShakeProfile);
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