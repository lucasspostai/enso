using Enso.Characters;
using Enso.Enums;
using Framework;
using Framework.Animations;
using UnityEngine;

namespace Enso.CombatSystem
{
    public class DamageController : MonoBehaviour, IFrameCheckHandler
    {
        private AnimationClipHolder damageAnimationClipHolder;
        private FrameChecker damageFrameChecker;
        private bool mustMove;

        private Fighter thisFighter;
        private CharacterMovement characterMovement;
        
        [HideInInspector] public bool IsAnyDamageAnimationPlaying;
        [HideInInspector] public bool IsDying;

        [SerializeField] protected Damage RegularDamageAnimation;
        [SerializeField] protected Damage HeavyDamageAnimation;
        [SerializeField] protected Damage SpecialDamageAnimation;
        [SerializeField] protected Damage LoseBalanceAnimation;
        [SerializeField] protected Damage DeathAnimation;

        private void Awake()
        {
            thisFighter = GetComponent<Fighter>();
            characterMovement = thisFighter.GetComponent<CharacterMovement>();
        }

        private void OnEnable()
        {
            thisFighter.GetHealthSystem().Damage += Damage;
            thisFighter.GetHealthSystem().Death += Death;
        }

        private void OnDisable()
        {
            thisFighter.GetHealthSystem().Damage -= Damage;
            thisFighter.GetHealthSystem().Death -= Death;
        }

        private void Start()
        {
            damageFrameChecker = new FrameChecker();

            ResetAllProperties();
        }
        
        private void Update()
        {
            if (!IsAnyDamageAnimationPlaying)
                return;
            
            damageFrameChecker.CheckFrames();
            
            if(mustMove)
                characterMovement.Move(characterMovement.CurrentDirection * (damageFrameChecker.MovementOffset * Time.deltaTime));
        }
        
        private void SetAnimationProperties(AnimationClipHolder animationClipHolder, FrameChecker frameChecker)
        {
            damageAnimationClipHolder = animationClipHolder;
            damageFrameChecker = frameChecker;

            damageAnimationClipHolder.Initialize(thisFighter.Animator);
            damageFrameChecker.Initialize(this, damageAnimationClipHolder);

            IsAnyDamageAnimationPlaying = true;
            
            thisFighter.GetComponent<AttackController>()?.OnInterrupted();
        }
        
        private void PlayDamageAnimation(Damage damage)
        {
            if (IsAnyDamageAnimationPlaying)
                return;

            SetAnimationProperties(damage.ClipHolder, damage.AnimationFrameChecker);
            thisFighter.Animator.Play(damage.ClipHolder.AnimatorStateName);
        }

        private void Damage()
        {
            switch (thisFighter.GetHealthSystem().CurrentAttackType)
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

        public void OnPlayAudio()
        {
            
        }

        public void OnHitFrameStart() { }

        public void OnHitFrameEnd() { }

        public void OnCanCutAnimation() { }
        
        public void OnStartMovement()
        {
            mustMove = true;
        }

        public void OnEndMovement()
        {
            mustMove = false;
        }

        public void OnLastFrameStart() { }

        public void OnLastFrameEnd()
        {
            if (IsDying)
                return;

            ResetAllProperties();
        }

        public void OnInterrupted()
        {
            
        }

        private void ResetAllProperties()
        {
            IsAnyDamageAnimationPlaying = false;
            IsDying = false;
        }
    }
}
