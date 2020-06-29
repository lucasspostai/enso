using System.Collections;
using System.Linq;
using Framework;
using UnityEngine;

namespace Enso.CombatSystem
{
    public class CharacterAnimationHandler : MonoBehaviour
    {
        private bool isDamageNotNull;
        private bool isAttackNotNull;
        private bool isGuardNotNull;
        private int faceXHash;
        private int faceYHash;
        private Coroutine flashCoroutine;
        private Coroutine pauseCoroutine;

        [SerializeField] private DamageController DamageController;
        [SerializeField] private CustomAnimationController[] ActionAnimationControllers;
        [SerializeField] private AttackController Attack;
        [SerializeField] private GuardController Guard;
        [SerializeField] private CharacterMovementController MovementController;
        [SerializeField] private float FlashDuration = 0.05f;
        [SerializeField] private float PauseDuration = 0.02f;

        [HideInInspector] public Vector3 CurrentDirection;

        public Animator CharacterAnimator;
        public SpriteRenderer CharacterSpriteRenderer;

        private static readonly int ShaderFlashAmount = Shader.PropertyToID("_FlashAmount");

        private void Awake()
        {
            isDamageNotNull = DamageController != null;
            isAttackNotNull = Attack != null;
            isGuardNotNull = Guard != null;

            faceXHash = Animator.StringToHash(MovementController.Animations.FaceX);
            faceYHash = Animator.StringToHash(MovementController.Animations.FaceY);
        }

        public bool IsDamageAnimationPlaying()
        {
            return isDamageNotNull && DamageController.IsAnimationPlaying;
        }

        private bool IsActionAnimationPlaying()
        {
            return ActionAnimationControllers.Length > 0 && ActionAnimationControllers.Any(customAnimationController =>
                customAnimationController != null
                && customAnimationController.IsAnimationPlaying);
        }

        private bool IsAttackAnimationPlaying()
        {
            return isAttackNotNull && Attack.IsAnimationPlaying;
        }

        public bool CanCutAttackAnimation()
        {
            return Attack.CanCutAnimation;
        }

        public void InterruptAllGuardAnimations()
        {
            if (isGuardNotNull)
                Guard.OnInterrupted();
        }

        public bool IsAnyAnimationDifferentThanAttackPlaying()
        {
            return IsDamageAnimationPlaying() ||
                   IsActionAnimationPlaying() ||
                   isGuardNotNull && Guard.IsPlayingAnimationThatDoesNotAllowLocomotion();
        }

        public bool IsAnyGuardAnimationPlaying()
        {
            return isGuardNotNull && Guard.IsAnimationPlaying;
        }

        public bool IsAnyCustomAnimationPlaying()
        {
            return IsDamageAnimationPlaying() ||
                   IsActionAnimationPlaying() ||
                   IsAttackAnimationPlaying() ||
                   isGuardNotNull && Guard.IsPlayingAnimationThatDoesNotAllowLocomotion();
        }

        private void InterruptAnyAnimationPlaying(CustomAnimationController customAnimationController)
        {
            if (isAttackNotNull && Attack != customAnimationController && Attack.IsAnimationPlaying)
            {
                Attack.OnInterrupted();
            }

            if (ActionAnimationControllers.Length == 0)
                return;

            foreach (var controller in ActionAnimationControllers)
            {
                if (controller && controller != customAnimationController)
                {
                    controller.OnInterrupted();
                }
            }
        }

        public void Play(CustomAnimationController controller, string stateName, bool ignoreNormalizedTime = false,
            int layer = -1,
            float normalizedTime = 0f)
        {
            InterruptAnyAnimationPlaying(controller);

            if (ignoreNormalizedTime)
                CharacterAnimator.Play(stateName);
            else
                CharacterAnimator.Play(stateName, layer, normalizedTime);
        }

        public void SetFacingDirection(Vector3 direction)
        {
            CharacterAnimator.SetFloat(faceXHash, direction.x);
            CharacterAnimator.SetFloat(faceYHash, direction.y);

            CurrentDirection = direction;
        }

        public void MakeCharacterFlash()
        {
            if (flashCoroutine != null)
                StopCoroutine(flashCoroutine);

            flashCoroutine = StartCoroutine(FlashThenReturnToNormal());
        }

        private IEnumerator FlashThenReturnToNormal()
        {
            CharacterSpriteRenderer.material.SetFloat(ShaderFlashAmount, 1f);

            yield return new WaitForSeconds(FlashDuration);

            CharacterSpriteRenderer.material.SetFloat(ShaderFlashAmount, 0f);
        }

        public void PauseAnimationForAWhile()
        {
            if (pauseCoroutine != null)
                StopCoroutine(pauseCoroutine);

            pauseCoroutine = StartCoroutine(PauseAnimationThenPlay());
        }

        private IEnumerator PauseAnimationThenPlay()
        {
            CharacterAnimator.speed = 0;

            yield return new WaitForSeconds(PauseDuration);

            CharacterAnimator.speed = 1;
        }

        public void ChangeSortingOrderOnDeath()
        {
            CharacterSpriteRenderer.sortingOrder = -1;
        }
    }
}