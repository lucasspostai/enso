using System.Collections.Generic;
using Enso.CombatSystem;
using UnityEngine;

namespace Enso.Characters.Player
{
    [RequireComponent(typeof(Player))]
    public class PlayerAttackController : AttackController
    {
        private bool attackQueued;
        private readonly List<Attack> lightAttacksAvailable = new List<Attack>();
        private Player player;

        [SerializeField] private List<Attack> LightAttacks = new List<Attack>();
        [SerializeField] private Attack StrongAttack;
        [SerializeField] private Attack SpecialAttack;

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
                        player.Movement.SetDirection(PlayerInput.Movement);
                    
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
    }
}