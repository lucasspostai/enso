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

        [SerializeField] protected Damage RegularDamageAnimation;
        [SerializeField] protected Damage HeavyDamageAnimation;
        [SerializeField] protected Damage SpecialDamageAnimation;
        [SerializeField] protected Damage LoseBalanceAnimation;
        [SerializeField] protected Damage DeathAnimation;

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
        
        private void PlayDamageAnimation(Damage damage)
        {
            if (IsAnimationPlaying)
                return;

            CurrentCharacterAnimation = damage;

            SetAnimationPropertiesAndPlay(damage.ClipHolder, damage.AnimationFrameChecker);
            
            ThisFighter.GetComponent<AttackController>()?.OnInterrupted();
        }

        private void Damage()
        {
            switch (ThisFighter.GetHealthSystem().CurrentAttackType)
            {
                case AttackType.Light:
                    PlayDamageAnimation(RegularDamageAnimation);
                    break;
                case AttackType.Strong:
                    PlayDamageAnimation(HeavyDamageAnimation);
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
