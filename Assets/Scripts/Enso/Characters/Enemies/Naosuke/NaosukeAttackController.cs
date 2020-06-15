using System.Collections;
using System.Collections.Generic;
using Enso.CombatSystem;
using UnityEngine;

namespace Enso.Characters.Enemies.Naosuke
{
    public class NaosukeAttackController : AttackController
    {
        private bool mustWaitAfterCompletion;
        private Coroutine waitAfterCompletionCoroutine;
        private float waitTime = 1f;
        private float currentStrongAttackCounter;
        private float maxComboAttacks;
        private readonly List<AttackAnimation> lightAttacksAvailable = new List<AttackAnimation>();
        private Naosuke naosuke;

        [SerializeField] private List<AttackAnimation> LightAttackAnimations = new List<AttackAnimation>();
        [SerializeField] private AttackAnimation StrongAttackAnimation;
        [SerializeField] private AttackAnimation SpecialAttackAnimation;
        [SerializeField] [Range(0, 10f)] private float StrongAttackCooldown = 8f;
        [SerializeField] [Range(0, 0.99f)] private float SpecialAttackCost = 0.9f;

        [HideInInspector] public bool CanAttack = true;
        [HideInInspector] public bool CanUseStrongAttack;
        [HideInInspector] public bool CanUseSpecialAttack;

        #region Delegates

        private void OnEnable()
        {
            ThisFighter.GetBalanceSystem().EnableSpecialAttack += EnableSpecialAttack;
            ThisFighter.GetBalanceSystem().LoseBalance += DisableSpecialAttack;
        }

        private void OnDisable()
        {
            ThisFighter.GetBalanceSystem().EnableSpecialAttack -= EnableSpecialAttack;
            ThisFighter.GetBalanceSystem().LoseBalance -= DisableSpecialAttack;
        }

        #endregion

        protected override void Start()
        {
            base.Start();

            naosuke = GetComponent<Naosuke>();

            ResetCombo();
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

        private void ResetCombo()
        {
            lightAttacksAvailable.Clear();

            for (int i = 0; i < maxComboAttacks; i++)
            {
                if (LightAttackAnimations[i])
                    lightAttacksAvailable.Add(LightAttackAnimations[i]);
            }

            if(waitAfterCompletionCoroutine != null)
                StopCoroutine(waitAfterCompletionCoroutine);

            waitAfterCompletionCoroutine = StartCoroutine(WaitThenEnableAttack());
        }

        private IEnumerator WaitThenEnableAttack()
        {
            yield return mustWaitAfterCompletion ? new WaitForSeconds(waitTime) : null;

            mustWaitAfterCompletion = false;
            CanAttack = true;
        }

        public void StartLightAttack()
        {
            if (ThisFighter.AnimationHandler.IsAnyAnimationDifferentThanAttackPlaying() &&
                !ThisFighter.AnimationHandler.IsAnyGuardAnimationPlaying())
                return;

            if (lightAttacksAvailable.Count == 0)
                ResetCombo();

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

            RotateTowardsTarget();

            CanCutAnimation = true;

            StartAttack(StrongAttackAnimation);

            currentStrongAttackCounter = 0;
            CanUseStrongAttack = false;
        }

        private void EnableSpecialAttack()
        {
            CanUseSpecialAttack = true;
        }

        private void DisableSpecialAttack()
        {
            CanUseSpecialAttack = false;
        }

        public void StartSpecialAttack()
        {
            if (!CanUseSpecialAttack || !CanCutAnimation &&
                (LightAttackAnimations.Count - lightAttacksAvailable.Count > 1 ||
                 ThisFighter.AnimationHandler.IsAnyAnimationDifferentThanAttackPlaying() ||
                 !ThisFighter.AnimationHandler.IsAnyGuardAnimationPlaying()))
                return;

            ResetCombo();

            RotateTowardsTarget();

            ThisFighter.AnimationHandler.InterruptAllGuardAnimations();

            StartAttack(SpecialAttackAnimation);

            //Special Attack Cost
            naosuke.GetBalanceSystem()
                .TakeDamage(Mathf.RoundToInt(naosuke.GetBalanceSystem().GetMaxBalance() * SpecialAttackCost));
        }

        private void RotateTowardsTarget()
        {
            naosuke.AnimationHandler.SetFacingDirection((ThisFighter.Target.position - transform.position)
                .normalized);
        }

        public void SetMaxCombo(int numberOfAttacks)
        {
            maxComboAttacks = numberOfAttacks;
        }

        public void WaitAfterAttack(float time)
        {
            mustWaitAfterCompletion = true;
            waitTime = time;
        }

        public override void OnCanCutAnimation()
        {
            base.OnCanCutAnimation();

            if (lightAttacksAvailable.Count > 0)
                StartLightAttack();
        }

        public override void OnLastFrameEnd()
        {
            base.OnLastFrameEnd();

            ResetCombo();
        }

        public override void OnInterrupted()
        {
            base.OnInterrupted();

            CurrentCharacterAnimation = null;
            
            ResetCombo();
        }

        protected override void ResetAllProperties()
        {
            base.ResetAllProperties();

            ResetCombo();

        }
    }
}