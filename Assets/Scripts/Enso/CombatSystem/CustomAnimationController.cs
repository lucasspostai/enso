using System;
using Enso.Characters;
using Framework;
using Framework.Animations;
using Framework.Audio;
using UnityEngine;

namespace Enso.CombatSystem
{
    public abstract class CustomAnimationController : MonoBehaviour, IFrameCheckHandler
    {
        private bool mustMove;
        private CharacterMovement characterMovement;

        protected AnimationClipHolder CurrentAnimationClipHolder;
        protected CharacterAnimation CurrentCharacterAnimation;
        protected FrameChecker CurrentFrameChecker;
        protected Fighter ThisFighter;

        [HideInInspector] public bool IsAnimationPlaying;
        [HideInInspector] public bool CanCutAnimation;

        private void Awake()
        {
            ThisFighter = GetComponent<Fighter>();
            characterMovement = ThisFighter.GetComponent<CharacterMovement>();
        }

        protected virtual void Start()
        {
            ResetAllProperties();
        }
        
        protected virtual void Update()
        {
            if (!IsAnimationPlaying)
                return;
            
            CurrentFrameChecker.CheckFrames();

            if (mustMove && CurrentCharacterAnimation)
                characterMovement.Move(characterMovement.CurrentDirection * (CurrentCharacterAnimation.AnimationFrameChecker.MovementOffset * Time.deltaTime));
        }
        
        protected void SetAnimationPropertiesAndPlay(AnimationClipHolder animationClipHolder, FrameChecker frameChecker)
        {
            CurrentAnimationClipHolder = animationClipHolder;
            CurrentFrameChecker = frameChecker;

            CurrentAnimationClipHolder.Initialize(ThisFighter.Animator);
            CurrentFrameChecker.Initialize(this, CurrentAnimationClipHolder);

            IsAnimationPlaying = true;
            
            ThisFighter.Animator.Play(CurrentAnimationClipHolder.AnimatorStateName);
        }

        public virtual void OnPlayAudio()
        {
            AudioManager.Instance.Play(CurrentFrameChecker.AnimationSoundCue, transform.position, transform.rotation);
        }

        public virtual void OnHitFrameStart() { }

        public virtual void OnHitFrameEnd() { }

        public virtual void OnCanCutAnimation()
        {
            if (CurrentCharacterAnimation && !CurrentCharacterAnimation.CanBeCut)
                return;
            
            CanCutAnimation = true;
        }

        public virtual void OnStartMovement()
        {
            mustMove = true;
        }

        public virtual void OnEndMovement()
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

        public virtual void OnInterrupted()
        {
            ResetAllProperties();
        }

        protected virtual void ResetAllProperties()
        {
            mustMove = false;
            IsAnimationPlaying = false;
            CanCutAnimation = true;
            CurrentCharacterAnimation = null;
        }
    }
}
