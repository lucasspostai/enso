using System.Collections.Generic;
using Enso.CombatSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enso.Characters.Player
{
    [RequireComponent(typeof(Player))]
    public class PlayerAttackController : AttackController
    {
        private bool attackQueued;
        private bool isHoldingAttackButton;
        private bool preparingStrongAttack;
        private bool holdingStrongAttack;
        private float holdingTime;
        private bool canUseSpecialAttack;
        private readonly List<AttackAnimation> lightAttacksAvailable = new List<AttackAnimation>();
        private Player player;

        [SerializeField] private List<AttackAnimation> LightAttackAnimations = new List<AttackAnimation>();
        [SerializeField] private AttackAnimation PrepareStrongAttackAnimation;
        [SerializeField] private AttackAnimation HoldStrongAttackAnimation;
        [SerializeField] private AttackAnimation ReleaseStrongAttackAnimation;
        [SerializeField] private AttackAnimation SpecialAttack;
        [SerializeField] private float StrongAttackDeadZoneTime;
        [SerializeField] [Range(0, 1)] private float SpecialAttackCost = 0.9f;

        #region Delegates

        private void OnEnable()
        {
            PlayerInput.AttackInputDown += PressAttackButton;
            PlayerInput.AttackInputUp += ReleaseAttackButton;
            PlayerInput.SpecialAttackInputDown += StartSpecialAttack;
            ThisFighter.GetBalanceSystem().EnableSpecialAttack += EnableSpecialAttack;
            ThisFighter.GetBalanceSystem().LoseBalance += DisableSpecialAttack;
        }

        private void OnDisable()
        {
            PlayerInput.AttackInputDown -= PressAttackButton;
            PlayerInput.AttackInputUp -= ReleaseAttackButton;
            PlayerInput.SpecialAttackInputDown -= StartSpecialAttack;
            ThisFighter.GetBalanceSystem().EnableSpecialAttack -= EnableSpecialAttack;
            ThisFighter.GetBalanceSystem().LoseBalance -= DisableSpecialAttack;
        }

        #endregion

        protected override void Start()
        {
            base.Start();

            player = GetComponent<Player>();

            ResetCombo();
        }

        protected override void Update()
        {
            base.Update();

            if (isHoldingAttackButton)
            {
                holdingTime += Time.deltaTime;

                if (holdingTime >= StrongAttackDeadZoneTime && CanCutAnimation && !preparingStrongAttack)
                    StartStrongAttack();
            }
        }

        private void PressAttackButton()
        {
            isHoldingAttackButton = true;

            StartLightAttack();
        }

        private void ReleaseAttackButton()
        {
            if (holdingStrongAttack)
            {
                ReleaseStrongAttack();
            }
            else
            {
                if (CanCutAnimation)
                    StartLightAttack();
            }

            ResetStrongAttackProperties();
        }

        private void StartLightAttack()
        {
            if (ThisFighter.AnimationHandler.IsAnyAnimationDifferentThanAttackPlaying() &&
                !ThisFighter.AnimationHandler.IsAnyGuardAnimationPlaying())
                return;

            if (!CanCutAnimation && IsAnimationPlaying)
            {
                attackQueued = true;
                return;
            }

            attackQueued = false;

            if (lightAttacksAvailable.Count == 0)
                ResetCombo();

            foreach (var attack in lightAttacksAvailable)
            {
                if (CurrentCharacterAnimation != attack)
                {
                    if (PlayerInput.Movement != Vector2.zero &&
                        !ThisFighter.AnimationHandler.IsAnyGuardAnimationPlaying())
                        player.AnimationHandler.SetFacingDirection(PlayerInput.Movement);

                    StartAttack(attack);
                    lightAttacksAvailable.Remove(attack);
                    break;
                }
            }
        }

        private void StartStrongAttack()
        {
            if (ThisFighter.AnimationHandler.IsAnyAnimationDifferentThanAttackPlaying() ||
                ThisFighter.AnimationHandler.IsAnyGuardAnimationPlaying())
                return;

            holdingTime = 0;

            preparingStrongAttack = true;

            CurrentCharacterAnimation = PrepareStrongAttackAnimation;

            SetAnimationPropertiesAndPlay(PrepareStrongAttackAnimation.ClipHolder,
                PrepareStrongAttackAnimation.AnimationFrameChecker);
        }

        private void HoldStrongAttack()
        {
            if (!IsAnimationPlaying)
                return;

            CanCutAnimation = false;

            holdingStrongAttack = true;

            CurrentCharacterAnimation = HoldStrongAttackAnimation;

            SetAnimationPropertiesAndPlay(HoldStrongAttackAnimation.ClipHolder,
                HoldStrongAttackAnimation.AnimationFrameChecker);
        }

        private void ReleaseStrongAttack()
        {
            if (!IsAnimationPlaying)
                return;

            if (PlayerInput.Movement != Vector2.zero)
                player.AnimationHandler.SetFacingDirection(PlayerInput.Movement);

            CanCutAnimation = true;

            StartAttack(ReleaseStrongAttackAnimation);

            isHoldingAttackButton = false;
        }

        private void EnableSpecialAttack()
        {
            canUseSpecialAttack = true;
        }

        private void DisableSpecialAttack()
        {
            canUseSpecialAttack = false;
        }

        private void StartSpecialAttack()
        {
            if (!canUseSpecialAttack || !CanCutAnimation &&
                (LightAttackAnimations.Count - lightAttacksAvailable.Count > 1 ||
                 ThisFighter.AnimationHandler.IsAnyAnimationDifferentThanAttackPlaying() ||
                 !ThisFighter.AnimationHandler.IsAnyGuardAnimationPlaying()))
                return;

            ResetCombo();
            ResetStrongAttackProperties();

            ThisFighter.AnimationHandler.InterruptAllGuardAnimations();

            StartAttack(SpecialAttack);

            //Special Attack Cost
            player.GetBalanceSystem()
                .TakeDamage(Mathf.RoundToInt(player.GetBalanceSystem().GetMaxBalance() * SpecialAttackCost));
        }

        private void ResetCombo()
        {
            lightAttacksAvailable.Clear();
            lightAttacksAvailable.AddRange(LightAttackAnimations);

            attackQueued = false;
        }

        public override void OnCanCutAnimation()
        {
            if (holdingStrongAttack || preparingStrongAttack)
                return;

            base.OnCanCutAnimation();

            if (!attackQueued || isHoldingAttackButton)
                return;

            attackQueued = false;
            StartLightAttack();
        }

        public override void OnLastFrameEnd()
        {
            if (holdingStrongAttack)
                return;

            if (preparingStrongAttack)
            {
                HoldStrongAttack();

                preparingStrongAttack = false;

                return;
            }

            base.OnLastFrameEnd();

            ResetCombo();

            if (PlayerInput.HoldingGuardInput)
            {
                player.GuardController.StartGuard();
            }
            else if (PlayerInput.HoldingHealInput)
            {
                player.HealController.TryHeal();
            }
        }

        public override void OnInterrupted()
        {
            base.OnInterrupted();

            ResetCombo();
        }

        private void ResetStrongAttackProperties()
        {
            isHoldingAttackButton = false;
            preparingStrongAttack = false;
            holdingStrongAttack = false;
            holdingTime = 0f;
        }

        protected override void ResetAllProperties()
        {
            base.ResetAllProperties();

            ResetStrongAttackProperties();

            ResetCombo();
        }
    }
}