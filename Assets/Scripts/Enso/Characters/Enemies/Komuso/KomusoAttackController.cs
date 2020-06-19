using System.Collections.Generic;
using Enso.CombatSystem;
using UnityEngine;

namespace Enso.Characters.Enemies.Komuso
{
    public class KomusoAttackController : EnemyAttackController
    {
        private bool performingStrongAttack;
        private float currentStrongAttackCounter;
        private readonly List<AttackAnimation> lightAttacksAvailable = new List<AttackAnimation>();
        private readonly List<AttackAnimation> strongAttacksAvailable = new List<AttackAnimation>();
        private KomusoGuardController komusoGuardController;

        [SerializeField] private List<AttackAnimation> LightAttackAnimations = new List<AttackAnimation>();
        [SerializeField] private List<AttackAnimation> StrongAttackAnimations = new List<AttackAnimation>();
        [SerializeField] private AttackAnimation RiposteAnimation;
        [SerializeField] [Range(0, 10f)] private float StrongAttackCooldown = 8f;

        [HideInInspector] public bool CanUseStrongAttack;

        protected override void Start()
        {
            base.Start();

            komusoGuardController = GetComponent<KomusoGuardController>();

            komusoGuardController.ParryHit += EnableParry;

            ResetLightAttackCombo();
            ResetStrongAttackCombo();
        }

        protected override void Update()
        {
            base.Update();

            currentStrongAttackCounter += Time.deltaTime;

            if (currentStrongAttackCounter >= StrongAttackCooldown)
            {
                CanUseStrongAttack = true;
            }
        }

        private void EnableParry()
        {
            StartRiposte();
        }

        private void StartRiposte()
        {
            StartAttack(RiposteAnimation);
            PlayRiposteParticle();
        }

        private void ResetLightAttackCombo()
        {
            lightAttacksAvailable.Clear();

            for (int i = 0; i < LightAttackAnimations.Count; i++)
            {
                if (LightAttackAnimations[i])
                    lightAttacksAvailable.Add(LightAttackAnimations[i]);
            }

            Wait();
        }

        private void ResetStrongAttackCombo()
        {
            strongAttacksAvailable.Clear();

            for (int i = 0; i < StrongAttackAnimations.Count; i++)
            {
                if (StrongAttackAnimations[i])
                    strongAttacksAvailable.Add(StrongAttackAnimations[i]);
            }

            performingStrongAttack = false;

            Wait();
        }

        public void StartLightAttack()
        {
            if (ThisFighter.AnimationHandler.IsAnyAnimationDifferentThanAttackPlaying() &&
                !ThisFighter.AnimationHandler.IsAnyGuardAnimationPlaying())
                return;

            if (lightAttacksAvailable.Count == 0)
                ResetLightAttackCombo();

            foreach (var attack in lightAttacksAvailable)
            {
                if (CurrentCharacterAnimation != attack)
                {
                    RotateTowardsTarget();

                    StartAttack(attack);
                    lightAttacksAvailable.Remove(attack);

                    CanAttack = false;

                    break;
                }
            }
        }

        public void StartStrongAttack()
        {
            if (ThisFighter.AnimationHandler.IsAnyAnimationDifferentThanAttackPlaying() ||
                ThisFighter.AnimationHandler.IsAnyGuardAnimationPlaying())
                return;

            if (strongAttacksAvailable.Count == 0)
                ResetStrongAttackCombo();

            foreach (var attack in strongAttacksAvailable)
            {
                if (CurrentCharacterAnimation != attack)
                {
                    RotateTowardsTarget();
                    
                    StartAttack(attack);
                    strongAttacksAvailable.Remove(attack);

                    performingStrongAttack = true;

                    CanAttack = false;

                    currentStrongAttackCounter = 0;
                    CanUseStrongAttack = false;

                    break;
                }
            }
        }

        public override void OnCanCutAnimation()
        {
            base.OnCanCutAnimation();

            if (performingStrongAttack)
            {
                if (strongAttacksAvailable.Count > 0)
                    StartStrongAttack();
            }
            else
            {
                if (lightAttacksAvailable.Count > 0)
                    StartLightAttack();
            }
        }

        public override void OnLastFrameStart()
        {
            base.OnLastFrameStart();

            if (strongAttacksAvailable.Count > 0 && performingStrongAttack)
            {
                StartStrongAttack();
            }
        }

        public override void OnLastFrameEnd()
        {
            base.OnLastFrameEnd();

            ResetLightAttackCombo();
            ResetStrongAttackCombo();
            
            ThisFighter.AnimationHandler.Play(this,
                ThisFighter.MovementController.Animations.IdleAnimationClipHolder.AnimatorStateName);
        }

        public override void OnInterrupted()
        {
            base.OnInterrupted();

            CurrentCharacterAnimation = null;

            ResetStrongAttackCombo();
            ResetLightAttackCombo();
        }

        protected override void ResetAllProperties()
        {
            base.ResetAllProperties();

            ResetStrongAttackCombo();
            ResetLightAttackCombo();
        }
    }
}