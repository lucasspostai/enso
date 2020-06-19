using System.Collections;
using Enso.CombatSystem;
using UnityEngine;

namespace Enso.Characters.Enemies.AshigaruWarrior
{
    public class AshigaruWarriorGuardController : EnemyGuardController
    {
        private AshigaruWarrior ashigaruWarrior;

        protected override void Start()
        {
            base.Start();

            ashigaruWarrior = GetComponent<AshigaruWarrior>();
        }

        protected override void PlayMovementAnimation()
        {
            base.PlayMovementAnimation();
                
            PlayGuardAnimation(Animations.GuardIdleAnimationClipHolder);
        }

        protected override void ResetAllProperties()
        {
            base.ResetAllProperties();

            if (ashigaruWarrior)
                ashigaruWarrior.MovementController.SetSpeed(ashigaruWarrior.GetBaseProperties().RunSpeed);
        }
    }
}
