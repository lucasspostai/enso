using System;
using System.Collections;
using Enso.CombatSystem;
using UnityEngine;

namespace Enso.Characters.Player
{
    [RequireComponent(typeof(Player))]
    public class PlayerHealController : HealController
    {
        [SerializeField] private ParticleSystem Particle;
        
        private void OnEnable()
        {
            PlayerInput.HealInputDown += TryHeal;
        }

        private void OnDisable()
        {
            PlayerInput.HealInputDown -= TryHeal;
        }

        public override void OnPlayAudio()
        {
            base.OnPlayAudio();
            
            Particle.Play();
        }
    }
}