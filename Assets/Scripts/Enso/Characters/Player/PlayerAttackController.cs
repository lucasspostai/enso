using System.Collections.Generic;
using Enso.CombatSystem;
using UnityEngine;

namespace Enso.Characters.Player
{
    [RequireComponent(typeof(Player))]
    public class PlayerAttackController : AttackController
    {
        private bool attackQueued;
        private readonly List<AttackAnimation> lightAttacksAvailable = new List<AttackAnimation>();
        private Player player;

        [SerializeField] private List<AttackAnimation> LightAttacks = new List<AttackAnimation>();
        [SerializeField] private AttackAnimation StrongAttack;
        [SerializeField] private AttackAnimation SpecialAttack;

        #region Delegates

        private void OnEnable()
        {
            PlayerInput.AttackInputDown += StartLightAttack;
        }

        private void OnDisable()
        {
            PlayerInput.AttackInputDown -= StartLightAttack;
        }

        #endregion

        protected override void Start()
        {
            base.Start();

            player = GetComponent<Player>();

            ResetCombo();
        }

        private void StartLightAttack()
        {
            if (ThisFighter.AnimationHandler.IsAnyAnimationDifferentThanAttackPlaying())
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
                    if(PlayerInput.Movement != Vector2.zero)
                        player.AnimationHandler.SetFacingDirection(PlayerInput.Movement);
                    
                    StartAttack(attack);
                    lightAttacksAvailable.Remove(attack);
                    break;
                }
            }
        }

        private void StartStrongAttack()
        {
            StartAttack(StrongAttack);
        }

        private void StartSpecialAttack()
        {
            StartAttack(SpecialAttack);
        }

        private void ResetCombo()
        {
            lightAttacksAvailable.Clear();
            lightAttacksAvailable.AddRange(LightAttacks);
        }

        public override void OnCanCutAnimation()
        {
            base.OnCanCutAnimation();

            if (!attackQueued)
                return;

            attackQueued = false;
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
    }
}