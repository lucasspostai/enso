using System;
using System.Collections;
using Enso.CombatSystem;
using UnityEngine;

namespace Enso.Characters.Player
{
    [RequireComponent(typeof(Player))]
    public class PlayerHealController : HealController
    {
        private void OnEnable()
        {
            PlayerInput.HealInputDown += TryHeal;
        }

        private void OnDisable()
        {
            PlayerInput.HealInputDown -= TryHeal;
        }
    }
}