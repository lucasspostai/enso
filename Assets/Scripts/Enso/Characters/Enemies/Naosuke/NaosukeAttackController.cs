using System.Collections.Generic;
using Enso.CombatSystem;
using UnityEngine;

namespace Enso.Characters.Enemies.Naosuke
{
    public class NaosukeAttackController : AttackController
    {
        private bool canUseSpecialAttack;
        private float maxComboAttacks;
        private readonly List<AttackAnimation> lightAttacksAvailable = new List<AttackAnimation>();
        private Naosuke naosuke;

        [SerializeField] private List<AttackAnimation> LightAttackAnimations = new List<AttackAnimation>();
        [SerializeField] private AttackAnimation StrongAttackAnimation;
        [SerializeField] private AttackAnimation SpecialAttackAnimation;
        [SerializeField] [Range(0, 1)] private float SpecialAttackCost = 0.9f;
        
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
        
        private void ResetCombo()
        {
            lightAttacksAvailable.Clear();

            for (int i = 0; i < maxComboAttacks; i++)
            {
                if(LightAttackAnimations[i])
                    lightAttacksAvailable.Add(LightAttackAnimations[i]);
            }
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
                    naosuke.AnimationHandler.SetFacingDirection((ThisFighter.Target.position - transform.position).normalized);
                    
                    StartAttack(attack);
                    lightAttacksAvailable.Remove(attack);
                    break;
                }
            }
        }

        public void StartStrongAttack()
        {
            if (ThisFighter.AnimationHandler.IsAnyAnimationDifferentThanAttackPlaying() ||
                ThisFighter.AnimationHandler.IsAnyGuardAnimationPlaying())
                return;

            //ROTATE TOWARDS PLAYER

            CanCutAnimation = true;

            StartAttack(StrongAttackAnimation);
        }

        private void EnableSpecialAttack()
        {
            canUseSpecialAttack = true;
        }

        private void DisableSpecialAttack()
        {
            canUseSpecialAttack = false;
        }

        public void StartSpecialAttack()
        {
            if (!canUseSpecialAttack || !CanCutAnimation &&
                (LightAttackAnimations.Count - lightAttacksAvailable.Count > 1 ||
                 ThisFighter.AnimationHandler.IsAnyAnimationDifferentThanAttackPlaying() ||
                 !ThisFighter.AnimationHandler.IsAnyGuardAnimationPlaying()))
                return;

            ResetCombo();

            ThisFighter.AnimationHandler.InterruptAllGuardAnimations();

            StartAttack(SpecialAttackAnimation);

            //Special Attack Cost
            naosuke.GetBalanceSystem()
                .TakeDamage(Mathf.RoundToInt(naosuke.GetBalanceSystem().GetMaxBalance() * SpecialAttackCost));
        }

        public void SetMaxCombo(int numberOfAttacks)
        {
            maxComboAttacks = numberOfAttacks;
        }

        public override void OnCanCutAnimation()
        {
            base.OnCanCutAnimation();

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

            ResetCombo();
        }

        protected override void ResetAllProperties()
        {
            base.ResetAllProperties();

            ResetCombo();
        }
    }
}
