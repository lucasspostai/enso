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
            if (!CanCutAnimation && IsAttackAnimationPlaying)
            {
                attackQueued = true;
                return;
            }
            
            attackQueued = false;

            if (lightAttacksAvailable.Count == 0)
                ResetCombo();

            foreach (var attack in lightAttacksAvailable)
            {
                if (CurrentAttack != attack)
                {
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
        }

        protected override void Update()
        {
            base.Update();

            Move();
        }

        protected override void Move()
        {
            base.Move();

            if (MustMove)
                player.Movement.Move(player.Movement.CurrentDirection * (CurrentAttack.MovementOffset * Time.deltaTime));
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