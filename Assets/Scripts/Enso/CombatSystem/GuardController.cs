using System;
using System.Collections;
using Enso.Characters.Player;
using Framework;
using Framework.Animations;
using Framework.Audio;
using Framework.Utils;
using UnityEngine;

namespace Enso.CombatSystem
{
    public class GuardController : CustomAnimationController
    {
        private Coroutine parryHitCoroutine;
        private FrameChecker defaultFrameChecker;
        private UniqueRandom uniqueRandom;

        [HideInInspector] public bool StartingGuard;
        [HideInInspector] public bool EndingGuard;
        [HideInInspector] public bool IsGuarding;
        [HideInInspector] public bool IsBlocking;
        [HideInInspector] public bool IsParrying;
        [HideInInspector] public bool GuardReleased;

        [Header("Animations")]
        [SerializeField] protected GuardAnimations Animations;
        [SerializeField] protected ActionAnimation BlockAnimation;
        [SerializeField] protected ActionAnimation ParryAnimation;
        [SerializeField] protected float ParryDuration = 2f;
        
        [Header("Feedback")]
        [SerializeField] protected GameObject[] BlockParticles;
        [SerializeField] protected SoundCue BlockSoundCue;
        
        [Header("Camera Shake")]
        [SerializeField] protected CameraShakeProfile BlockShakeProfile;

        public event Action ParryHit;
        public event Action DisableParryHit;

        protected override void Start()
        {
            base.Start();
            
            CurrentFrameChecker = new FrameChecker();
            defaultFrameChecker = CurrentFrameChecker;
            
            uniqueRandom = new UniqueRandom(0, BlockParticles.Length);

            foreach (var blockParticle in BlockParticles)
            {
                PoolManager.Instance.CreatePool(blockParticle, 3);
            }
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
            if (ThisFighter.AnimationHandler.IsAnyCustomAnimationPlaying())
                return;
            
            GuardReleased = false;

            StartingGuard = true;

            ThisFighter.MovementController.SetSpeed(ThisFighter.GetBaseProperties().GuardSpeed);

            SetAnimationPropertiesAndPlay(Animations.StartGuardAnimationClipHolder, CurrentFrameChecker);
        }

        public virtual void EndGuard()
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
                ThisFighter.AnimationHandler.Play(this, animationClipHolder.AnimatorStateName, true);
            else
            {
                ThisFighter.AnimationHandler.Play(this, animationClipHolder.AnimatorStateName, false,
                    animationClipHolder.LayerNumber,
                    nextFramePercentage);
            }
        }

        public virtual void Block()
        {
            if (!IsAnimationPlaying)
                return;

            IsBlocking = true;

            CurrentCharacterAnimation = BlockAnimation;

            SetAnimationPropertiesAndPlay(BlockAnimation.ClipHolder, BlockAnimation.AnimationFrameChecker);
            
            ThisFighter.AnimationHandler.PauseAnimationForAWhile();
            
            if(BlockParticles.Length > 0)
                SpawnParticle(BlockParticles[uniqueRandom.GetRandomInt()]);
            
            if(BlockSoundCue)
                AudioManager.Instance.Play(BlockSoundCue, transform.position, Quaternion.identity);
            
            if(BlockShakeProfile)
                PlayerCinemachineManager.Instance.ShakeController.Shake(BlockShakeProfile);
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