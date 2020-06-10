using Enso.Characters;
using Enso.Enums;
using Framework;
using Framework.Animations;
using UnityEngine;

namespace Enso.CombatSystem
{
    public class CharacterDamageController : CustomAnimationController
    {
        [HideInInspector] public bool IsDying;

        [SerializeField] protected DamageAnimation RegularDamageAnimation;
        [SerializeField] protected DamageAnimation HeavyDamageAnimation;
        [SerializeField] protected DamageAnimation LoseBalanceAnimation;
        [SerializeField] protected DamageAnimation DeathAnimation;
        [SerializeField] protected GameObject RegularDamageParticle;
        [SerializeField] protected GameObject HeavyDamageParticle;

        private void OnEnable()
        {
            ThisFighter.GetHealthSystem().Damage += SpawnDamageParticleAndPlayAnimation;
            ThisFighter.GetHealthSystem().Death += Death;
        }

        private void OnDisable()
        {
            ThisFighter.GetHealthSystem().Damage -= SpawnDamageParticleAndPlayAnimation;
            ThisFighter.GetHealthSystem().Death -= Death;
        }

        protected override void Start()
        {
            base.Start();

            PoolManager.Instance.CreatePool(RegularDamageParticle, 3);
            PoolManager.Instance.CreatePool(HeavyDamageParticle, 3);
        }

        private void PlayDamageAnimation(DamageAnimation damageAnimation)
        {
            CurrentCharacterAnimation = damageAnimation;

            SetAnimationPropertiesAndPlay(damageAnimation.ClipHolder, damageAnimation.AnimationFrameChecker);

            ThisFighter.GetComponent<AttackController>()?.OnInterrupted();
        }

        private void SpawnDamageParticleAndPlayAnimation()
        {
            if (IsDying)
                return;

            switch (ThisFighter.GetHealthSystem().CurrentAttackType)
            {
                case AttackType.Light:
                    PlayDamageAnimation(RegularDamageAnimation);
                    SpawnParticle(RegularDamageParticle);
                    break;
                case AttackType.Strong:
                    PlayDamageAnimation(HeavyDamageAnimation);
                    SpawnParticle(HeavyDamageParticle);
                    break;
                default:
                    PlayDamageAnimation(RegularDamageAnimation);
                    SpawnParticle(RegularDamageParticle);
                    break;
            }
        }

        private void SpawnParticle(GameObject particle)
        {
            PoolManager.Instance.ReuseObject(particle, transform.position, particle.transform.rotation);
        }

        private void Death()
        {
            if (IsDying)
                return;

            IsDying = true;
            SpawnParticle(HeavyDamageParticle);

            PlayDamageAnimation(DeathAnimation);
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