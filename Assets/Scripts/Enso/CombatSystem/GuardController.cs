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

        [HideInInspector] public bool IsAnyGuardAnimationPlaying;
        [HideInInspector] public bool StartingGuard;
        [HideInInspector] public bool EndingGuard;
        [HideInInspector] public bool IsGuarding;
        [HideInInspector] public bool IsBlocking;

        [SerializeField] protected GuardAnimations Animations;

        protected virtual void Start()
        {
            guardFrameChecker = new FrameChecker();

            ThisFighter = GetComponent<Fighter>();

            ResetAllProperties();
        }

        protected virtual void Update()
        {
            if (!IsAnyGuardAnimationPlaying)
                return;

            guardFrameChecker.CheckFrames();

            if (IsGuarding && !EndingGuard && !IsBlocking)
                PlayMovementAnimation();
        }

        private void SetAnimationProperties(AnimationClipHolder animationClipHolder, FrameChecker frameChecker)
        {
            guardAnimationClipHolder = animationClipHolder;
            guardFrameChecker = frameChecker;

            guardAnimationClipHolder.Initialize(ThisFighter.Animator);
            guardFrameChecker.Initialize(this, guardAnimationClipHolder);

            IsAnyGuardAnimationPlaying = true;
        }

        protected virtual void StartGuard()
        {
            if (IsAnyGuardAnimationPlaying)
                return;

            StartingGuard = true;

            SetAnimationProperties(Animations.StartGuardAnimationClipHolder, guardFrameChecker);
            ThisFighter.Animator.Play(Animations.StartGuardAnimationClipHolder.AnimatorStateName);
        }

        protected void EndGuard()
        {
            if (!IsAnyGuardAnimationPlaying)
                return;

            EndingGuard = true;

            SetAnimationProperties(Animations.EndGuardAnimationClipHolder, guardFrameChecker);
            ThisFighter.Animator.Play(Animations.EndGuardAnimationClipHolder.AnimatorStateName);
        }

        protected void PlayGuardAnimation(AnimationClipHolder animationClipHolder, bool atNextFrame = false)
        {
            if (!IsAnyGuardAnimationPlaying)
                return;

            float nextFramePercentage = guardAnimationClipHolder.GetNextFramePercentage();
            
            if (Animator.StringToHash(ThisFighter.Animator.GetLayerName(animationClipHolder.LayerNumber) + "." +
                                      animationClipHolder.AnimatorStateName) ==
                guardAnimationClipHolder.GetAnimationFullNameHash())
                return;

            SetAnimationProperties(animationClipHolder, guardFrameChecker);

            if (!atNextFrame)
                ThisFighter.Animator.Play(animationClipHolder.AnimatorStateName);
            else
            {
                ThisFighter.Animator.Play(animationClipHolder.AnimatorStateName, animationClipHolder.LayerNumber,
                    nextFramePercentage);
            }
        }

        public void Block()
        {
            if (!IsAnyGuardAnimationPlaying)
                return;

            IsBlocking = true;

            SetAnimationProperties(Animations.BlockAnimationClipHolder, guardFrameChecker);
            ThisFighter.Animator.Play(Animations.BlockAnimationClipHolder.AnimatorStateName);
        }

        protected virtual void PlayMovementAnimation()
        {
        }

        public void OnPlayAudio()
        {
        }

        public void OnHitFrameStart()
        {
        }

        public void OnHitFrameEnd()
        {
        }

        public void OnCanCutAnimation()
        {
        }

        public void OnStartMovement()
        {
        }

        public void OnEndMovement()
        {
        }

        public void OnLastFrameStart()
        {
        }

        public void OnLastFrameEnd()
        {
            if (StartingGuard)
            {
                IsGuarding = true;
                StartingGuard = false;
            }

            if (IsBlocking)
                IsBlocking = false;

            if (EndingGuard)
                ResetAllProperties();
        }

        public void OnInterrupted()
        {
        }

        protected virtual void ResetAllProperties()
        {
            IsAnyGuardAnimationPlaying = false;
            IsGuarding = false;
            StartingGuard = false;
            EndingGuard = false;
            IsBlocking = false;
        }
    }
}