using System.Linq;
using Framework;
using UnityEngine;

namespace Enso.CombatSystem
{
    public class CharacterAnimationHandler : MonoBehaviour
    {
        private bool isDamageNotNull;
        private bool isAttackNotNull;
        private int faceXHash;
        private int faceYHash;

        [SerializeField] private DamageController Damage;
        [SerializeField] private CustomAnimationController[] ActionAnimationControllers;
        [SerializeField] private AttackController Attack;
        [SerializeField] private GuardController Guard;
        [SerializeField] private CharacterMovementController MovementController;
        
        [HideInInspector] public Vector3 CurrentDirection;
        
        public Animator CharacterAnimator;

        private void Awake()
        {
            isDamageNotNull = Damage != null;
            isAttackNotNull = Attack != null;

            faceXHash = Animator.StringToHash(MovementController.Animations.FaceX);
            faceYHash = Animator.StringToHash(MovementController.Animations.FaceY);
        }

        private bool IsDamageAnimationPlaying()
        {
            return isDamageNotNull && Damage.IsAnimationPlaying;
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
            Guard.OnInterrupted();
        }

        public bool IsAnyAnimationDifferentThanAttackPlaying()
        {
            return IsDamageAnimationPlaying() ||
                   IsActionAnimationPlaying() ||
                   Guard.IsPlayingAnimationThatDoesNotAllowLocomotion();
        }

        public bool IsAnyGuardAnimationPlaying()
        {
            return Guard.IsAnimationPlaying;
        }

        public bool IsAnyCustomAnimationPlaying()
        {
            return IsDamageAnimationPlaying() ||
                   IsActionAnimationPlaying() ||
                   IsAttackAnimationPlaying() ||
                   Guard.IsPlayingAnimationThatDoesNotAllowLocomotion();
        }

        private void InterruptAnyAnimationPlaying(CustomAnimationController customAnimationController)
        {
            if (isAttackNotNull && Attack != customAnimationController)
            {
                Attack.OnInterrupted();
            }

            if (ActionAnimationControllers.Length == 0)
                return;

            foreach (var controller in ActionAnimationControllers)
            {
                if (controller != customAnimationController)
                {
                    controller.OnInterrupted();
                }
            }
        }

        public void Play(CustomAnimationController controller, string stateName, int layer = -1,
            float normalizedTime = 0f)
        {
            InterruptAnyAnimationPlaying(controller);

            if (normalizedTime > 0f)
                CharacterAnimator.Play(stateName, layer, normalizedTime);
            else
                CharacterAnimator.Play(stateName);
        }

        public void SetFacingDirection(Vector3 direction)
        {
            CharacterAnimator.SetFloat(faceXHash, direction.x);
            CharacterAnimator.SetFloat(faceYHash, direction.y);

            CurrentDirection = direction;
        }
    }
}