using System;
using Enso.Characters;
using Framework.Animations;
using UnityEngine;

namespace Enso.CombatSystem
{
    public class GuardController : MonoBehaviour, IFrameCheckHandler
    {
        private AnimationClipHolder guardAnimationClipHolder;
        private FrameChecker guardFrameChecker;
        
        protected Fighter ThisFighter;

        [HideInInspector] public bool IsGuardAnimationPlaying;
        
        [SerializeField] private GuardAnimations Animations;

        protected void Start()
        {
            guardFrameChecker = new FrameChecker();
            
            ThisFighter = GetComponent<Fighter>();
            
            ResetAllProperties();
        }

        protected virtual void Update()
        {
            if (!IsGuardAnimationPlaying)
                return;
            
            guardFrameChecker.CheckFrames();
        }
        
        private void SetAnimationProperties(AnimationClipHolder animationClipHolder, FrameChecker frameChecker)
        {
            guardAnimationClipHolder = animationClipHolder;
            guardFrameChecker = frameChecker;

            guardAnimationClipHolder.Initialize(ThisFighter.Animator);
            guardFrameChecker.Initialize(this, guardAnimationClipHolder);

            IsGuardAnimationPlaying = true;
        }
        
        protected void StartGuard(AnimationClipHolder animationClipHolder)
        {
            if (IsGuardAnimationPlaying)
                return;

            SetAnimationProperties(animationClipHolder, guardFrameChecker);
            ThisFighter.Animator.Play(animationClipHolder.AnimatorStateName);
        }

        public void OnHitFrameStart() { }

        public void OnHitFrameEnd() { }

        public void OnCanCutAnimation()
        {
            
        }

        public void OnLastFrameStart()
        {
            
        }

        public void OnLastFrameEnd()
        {
            
        }
        
        private void ResetAllProperties()
        {
            IsGuardAnimationPlaying = false;
        }
    }
}
