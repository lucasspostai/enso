using Enso.Characters;
using Framework.Animations;
using UnityEngine;

namespace Enso.CombatSystem
{
    public class GuardController : CustomAnimationController
    {
        [HideInInspector] public bool StartingGuard;
        [HideInInspector] public bool EndingGuard;
        [HideInInspector] public bool IsGuarding;
        [HideInInspector] public bool IsBlocking;

        [SerializeField] protected GuardAnimations Animations;

        protected override void Start()
        {
            base.Start();
            
            CurrentFrameChecker = new FrameChecker();
        }

        protected override void Update()
        {
            base.Update();

            if (IsGuarding && !EndingGuard && !IsBlocking)
                PlayMovementAnimation();
        }

        protected virtual void StartGuard()
        {
            if (ThisFighter.AnimationHandler.IsAnyCustomAnimationPlaying())
                return;

            StartingGuard = true;
            
            ThisFighter.MovementController.SetSpeed(ThisFighter.GetBaseProperties().GuardSpeed);

            SetAnimationPropertiesAndPlay(Animations.StartGuardAnimationClipHolder, CurrentFrameChecker);
        }

        protected void EndGuard()
        {
            if (!IsAnimationPlaying)
                return;

            EndingGuard = true;

            SetAnimationPropertiesAndPlay(Animations.EndGuardAnimationClipHolder, CurrentFrameChecker);
            ThisFighter.AnimationHandler.Play(this ,Animations.EndGuardAnimationClipHolder.AnimatorStateName);
        }

        protected void PlayGuardAnimation(AnimationClipHolder animationClipHolder, bool atNextFrame = false)
        {
            if (!IsAnimationPlaying)
                return;

            var nextFramePercentage = CurrentAnimationClipHolder.GetNextFramePercentage();

            if (nextFramePercentage > animationClipHolder.PercentageOnFrame(animationClipHolder.GetTotalFrames()))
                atNextFrame = false;
            
            if (Animator.StringToHash(ThisFighter.AnimationHandler.CharacterAnimator.GetLayerName(animationClipHolder.LayerNumber) + "." +
                                      animationClipHolder.AnimatorStateName) ==
                CurrentAnimationClipHolder.GetAnimationFullNameHash())
                return;

            SetAnimationPropertiesAndPlay(animationClipHolder, CurrentFrameChecker);

            if (!atNextFrame)
                ThisFighter.AnimationHandler.Play(this, animationClipHolder.AnimatorStateName);
            else
            {
                ThisFighter.AnimationHandler.Play(this, animationClipHolder.AnimatorStateName, animationClipHolder.LayerNumber,
                    nextFramePercentage);
            }
        }

        public void Block()
        {
            if (!IsAnimationPlaying)
                return;

            IsBlocking = true;

            SetAnimationPropertiesAndPlay(Animations.BlockAnimationClipHolder, CurrentFrameChecker);
        }

        protected virtual void PlayMovementAnimation() { }

        public override void OnLastFrameEnd()
        {
            if (StartingGuard)
            {
                IsGuarding = true;
                StartingGuard = false;
            }

            if (IsBlocking)
                IsBlocking = false;

            if (EndingGuard)
                base.OnLastFrameEnd();
        }

        protected override void ResetAllProperties()
        {
            base.ResetAllProperties();
            
            IsGuarding = false;
            StartingGuard = false;
            EndingGuard = false;
            IsBlocking = false;
        }

        public bool IsPlayingAnimationThatDoesNotAllowLocomotion()
        {
            return StartingGuard || EndingGuard || IsBlocking; 
        }
    }
}