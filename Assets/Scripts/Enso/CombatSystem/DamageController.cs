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
        
        [SerializeField] protected DamageAnimations Animations;

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
            
            //if(mustMove)
                //characterMovement.Move(player.Movement.CurrentDirection * (CurrentAttack.MovementOffset * Time.deltaTime));
        }
        
        private void SetAnimationProperties(AnimationClipHolder animationClipHolder, FrameChecker frameChecker)
        {
            damageAnimationClipHolder = animationClipHolder;
            damageFrameChecker = frameChecker;

            damageAnimationClipHolder.Initialize(thisFighter.Animator);
            damageFrameChecker.Initialize(this, damageAnimationClipHolder);

            IsAnyDamageAnimationPlaying = true;
        }
        
        private void PlayDamageAnimation(AnimationClipHolder animationClipHolder)
        {
            if (IsAnyDamageAnimationPlaying)
                return;

            SetAnimationProperties(animationClipHolder, damageFrameChecker);
            thisFighter.Animator.Play(animationClipHolder.AnimatorStateName);
        }

        private void Damage()
        {
            switch (thisFighter.GetHealthSystem().CurrentAttackType)
            {
                case AttackType.Light:
                    PlayDamageAnimation(Animations.DamageAnimationClipHolder);
                    break;
                case AttackType.Strong:
                    PlayDamageAnimation(Animations.HeavyAnimationClipHolder);
                    break;
                case AttackType.Special:
                    PlayDamageAnimation(Animations.HeavyAnimationClipHolder);
                    break;
                default:
                    PlayDamageAnimation(Animations.DamageAnimationClipHolder);
                    break;
            }
        }
        
        private void Death()
        {
            IsDying = true;
            
            PlayDamageAnimation(Animations.DeathAnimationClipHolder);
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
        
        private void ResetAllProperties()
        {
            IsAnyDamageAnimationPlaying = false;
            IsDying = false;
        }
    }
}
