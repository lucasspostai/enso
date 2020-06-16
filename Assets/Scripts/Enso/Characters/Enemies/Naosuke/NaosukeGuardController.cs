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
        private Coroutine parryStanceCoroutine;
        private float waitTime = 1f;
        private Naosuke naosuke;
        
        [SerializeField] private float MaxTimeOnParryStance = 5f;

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

        public override void Parry()
        {
            base.Parry();
            
            ThisFighter.MovementController.SetSpeed(0);
            
            if(parryStanceCoroutine != null)
                StopCoroutine(parryStanceCoroutine);

            parryStanceCoroutine = StartCoroutine(StayOnParryStance());
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

        private IEnumerator StayOnParryStance()
        {
            yield return new WaitForSeconds(MaxTimeOnParryStance);
            
            if(IsParrying)
                base.OnLastFrameEnd();
            
            ThisFighter.MovementController.SetSpeed(ThisFighter.GetBaseProperties().RunSpeed);
        }

        public override void OnLastFrameEnd()
        {
            if (!IsParrying)
                base.OnLastFrameEnd();
        }
        
        protected override void ResetAllProperties()
        {
            base.ResetAllProperties();

            IsParrying = false;

            if (naosuke)
                naosuke.MovementController.SetSpeed(naosuke.GetBaseProperties().RunSpeed);
        }
    }
}
