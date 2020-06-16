using System.Collections;
using Enso.CombatSystem;
using UnityEngine;

namespace Enso.Characters.Enemies.AshigaruWarrior
{
    public class AshigaruWarriorGuardController : GuardController
    {
        private bool mustWaitAfterStartGuard;
        private Coroutine waitAfterStartGuardCoroutine;
        private float waitTime = 1f;
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

        public void WaitAfterStartGuard(float time)
        {
            waitTime = time;
            
            if(waitAfterStartGuardCoroutine != null)
                StopCoroutine(waitAfterStartGuardCoroutine);

            waitAfterStartGuardCoroutine = StartCoroutine(WaitThenStopGuard());
        }

        private IEnumerator WaitThenStopGuard()
        {
            yield return new WaitForSeconds(waitTime);
            
            EndGuard();
        }
        
        protected override void ResetAllProperties()
        {
            base.ResetAllProperties();

            if (ashigaruWarrior)
                ashigaruWarrior.MovementController.SetSpeed(ashigaruWarrior.GetBaseProperties().RunSpeed);
        }
    }
}
