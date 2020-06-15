using System.Collections;
using Enso.CombatSystem;
using UnityEngine;

namespace Enso.Characters.Enemies.Naosuke
{
    [RequireComponent(typeof(Naosuke))]
    public class NaosukeGuardController : GuardController
    {
        private bool mustWaitAfterStartGuard;
        private Coroutine waitAfterStartGuardCoroutine;
        private float waitTime = 1f;
        private Naosuke naosuke;

        [SerializeField] private ActionAnimation ParryStanceAnimation;

        protected override void Start()
        {
            base.Start();

            naosuke = GetComponent<Naosuke>();
        }

        protected override void PlayMovementAnimation()
        {
            base.PlayMovementAnimation();
            
            if (naosuke.MovementController.Velocity == Vector3.zero)
            {
                PlayGuardAnimation(Animations.GuardIdleAnimationClipHolder);
            }
            else
            {
                PlayGuardAnimation(Animations.ForwardGuardWalkAnimationClipHolder, true);
            }
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

            if (naosuke)
                naosuke.MovementController.SetSpeed(naosuke.GetBaseProperties().RunSpeed);
        }
    }
}
