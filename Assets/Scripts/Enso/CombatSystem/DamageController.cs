using Enso.Characters;
using Enso.Enums;
using Framework;
using Framework.Animations;
using UnityEngine;

namespace Enso.CombatSystem
{
    public class DamageController : CustomAnimationController
    {
        [HideInInspector] public bool IsDying;

        [SerializeField] protected DamageAnimation RegularDamageAnimation;
        [SerializeField] protected DamageAnimation HeavyDamageAnimation;
        [SerializeField] protected DamageAnimation SpecialDamageAnimation;
        [SerializeField] protected DamageAnimation LoseBalanceAnimation;
        [SerializeField] protected DamageAnimation DeathAnimation;
        [SerializeField] protected GameObject RegularDamageParticle;
        [SerializeField] protected GameObject HeavyDamageParticle;

        private void OnEnable()
        {
            ThisFighter.GetHealthSystem().Damage += Damage;
            ThisFighter.GetHealthSystem().Death += Death;
        }

        private void OnDisable()
        {
            ThisFighter.GetHealthSystem().Damage -= Damage;
            ThisFighter.GetHealthSystem().Death -= Death;
        }
        
        private void PlayDamageAnimation(DamageAnimation damageAnimation)
        {
            if (IsAnimationPlaying)
                return;

            CurrentCharacterAnimation = damageAnimation;

            SetAnimationPropertiesAndPlay(damageAnimation.ClipHolder, damageAnimation.AnimationFrameChecker);
            
            ThisFighter.GetComponent<AttackController>()?.OnInterrupted();
        }

        private void Damage()
        {
            switch (ThisFighter.GetHealthSystem().CurrentAttackType)
            {
                case AttackType.Light:
                    PlayDamageAnimation(RegularDamageAnimation);
                    Instantiate(RegularDamageParticle, transform.position, RegularDamageParticle.transform.rotation);
                    break;
                case AttackType.Strong:
                    PlayDamageAnimation(HeavyDamageAnimation);
                    Instantiate(HeavyDamageParticle, transform.position, HeavyDamageParticle.transform.rotation);
                    break;
                case AttackType.Special:
                    PlayDamageAnimation(SpecialDamageAnimation);
                    break;
                default:
                    PlayDamageAnimation(RegularDamageAnimation);
                    break;
            }
        }
        
        private void Death()
        {
            IsDying = true;
            
            Instantiate(HeavyDamageParticle, transform.position, HeavyDamageParticle.transform.rotation);
            
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
