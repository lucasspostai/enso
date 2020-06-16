using System;
using System.Collections;
using Enso.Characters;
using Framework.Animations;
using UnityEngine;

namespace Enso.CombatSystem
{
    public class GuardController : CustomAnimationController
    {
        private Coroutine parryHitCoroutine;
        private FrameChecker defaultFrameChecker;

        [HideInInspector] public bool StartingGuard;
        [HideInInspector] public bool EndingGuard;
        [HideInInspector] public bool IsGuarding;
        [HideInInspector] public bool IsBlocking;
        [HideInInspector] public bool IsParrying;
        [HideInInspector] public bool GuardReleased;

        [SerializeField] protected GuardAnimations Animations;
        [SerializeField] protected ActionAnimation BlockAnimation;
        [SerializeField] protected ActionAnimation ParryAnimation;
        [SerializeField] protected float ParryDuration = 2f;

        public event Action ParryHit;
        public event Action DisableParryHit;

        protected override void Start()
        {
            base.Start();

            CurrentFrameChecker = new FrameChecker();
            defaultFrameChecker = CurrentFrameChecker;
        }

        protected override void Update()
        {
            base.Update();

            if (IsGuarding && !EndingGuard && !IsBlocking)
            {
                PlayMovementAnimation();
            }
        }

        public virtual void StartGuard()
        {
            GuardReleased = false;

            if (ThisFighter.AnimationHandler.IsAnyCustomAnimationPlaying())
                return;

            StartingGuard = true;

            ThisFighter.MovementController.SetSpeed(ThisFighter.GetBaseProperties().GuardSpeed);

            SetAnimationPropertiesAndPlay(Animations.StartGuardAnimationClipHolder, CurrentFrameChecker);
        }

        protected void EndGuard()
        {
            GuardReleased = true;

            if (!IsAnimationPlaying || IsBlocking)
                return;

            EndingGuard = true;

            SetAnimationPropertiesAndPlay(Animations.EndGuardAnimationClipHolder, CurrentFrameChecker);
        }

        protected void PlayGuardAnimation(AnimationClipHolder animationClipHolder, bool atNextFrame = false)
        {
            if (!IsAnimationPlaying)
                return;

            var nextFramePercentage = CurrentAnimationClipHolder.GetNextFramePercentage();

            if (nextFramePercentage > animationClipHolder.PercentageOnFrame(animationClipHolder.GetTotalFrames()))
                atNextFrame = false;

            if (Animator.StringToHash(
                    ThisFighter.AnimationHandler.CharacterAnimator.GetLayerName(animationClipHolder.LayerNumber) + "." +
                    animationClipHolder.AnimatorStateName) ==
                CurrentAnimationClipHolder.GetAnimationFullNameHash())
                return;

            SetAnimationPropertiesAndPlay(animationClipHolder, CurrentFrameChecker);

            if (!atNextFrame)
                ThisFighter.AnimationHandler.Play(this, animationClipHolder.AnimatorStateName);
            else
            {
                ThisFighter.AnimationHandler.Play(this, animationClipHolder.AnimatorStateName,
                    animationClipHolder.LayerNumber,
                    nextFramePercentage);
            }
        }

        public void Block()
        {
            if (!IsAnimationPlaying)
                return;

            IsBlocking = true;

            CurrentCharacterAnimation = BlockAnimation;

            SetAnimationPropertiesAndPlay(BlockAnimation.ClipHolder, BlockAnimation.AnimationFrameChecker);
        }

        public virtual void Parry()
        {
            if (ThisFighter.AnimationHandler.IsDamageAnimationPlaying())
                return;

            IsParrying = true;

            CurrentCharacterAnimation = ParryAnimation;

            SetAnimationPropertiesAndPlay(ParryAnimation.ClipHolder, ParryAnimation.AnimationFrameChecker);
        }

        protected virtual void PlayMovementAnimation()
        {
        }

        public override void OnLastFrameEnd()
        {
            if (StartingGuard)
            {
                IsGuarding = true;
                StartingGuard = false;
            }

            if (IsBlocking)
            {
                CurrentCharacterAnimation = null;

                IsBlocking = false;
                CurrentFrameChecker = defaultFrameChecker;

                if (GuardReleased)
                    EndGuard();
            }

            if (IsParrying)
            {
                CurrentCharacterAnimation = null;

                IsParrying = false;
                CurrentFrameChecker = defaultFrameChecker;

                base.OnLastFrameEnd();
            }

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
            IsParrying = false;
            GuardReleased = false;
        }

        public bool IsPlayingAnimationThatDoesNotAllowLocomotion()
        {
            return StartingGuard || EndingGuard || IsBlocking;
        }

        private IEnumerator WaitThenDisableParry()
        {
            yield return new WaitForSeconds(ParryDuration);

            OnDisableParryHit();
        }

        public virtual void OnParryHit(Vector3 direction)
        {
            if (parryHitCoroutine != null)
                StopCoroutine(parryHitCoroutine);

            ParryHit?.Invoke();

            ThisFighter.AnimationHandler.SetFacingDirection((direction * -1).normalized);

            parryHitCoroutine = StartCoroutine(WaitThenDisableParry());
        }

        protected virtual void OnDisableParryHit()
        {
            DisableParryHit?.Invoke();
        }
    }
}