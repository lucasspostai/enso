using System.Collections;
using UnityEngine;

namespace Enso.Characters.Enemies.Komuso
{
    public class KomusoGuardController : EnemyGuardController
    {
        private Coroutine parryStanceCoroutine;
        private Coroutine parryCooldownCoroutine;

        [SerializeField] private float MaxTimeOnParryStance = 5f;
        [SerializeField] private float ParryCooldown = 10f;

        [HideInInspector] public bool CanParry = true;

        protected override void PlayMovementAnimation()
        {
            base.PlayMovementAnimation();
            
            if (ThisFighter.MovementController.Velocity == Vector3.zero)
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
            
            if(parryCooldownCoroutine != null)
                StopCoroutine(parryCooldownCoroutine);

            parryCooldownCoroutine = StartCoroutine(WaitThenEnableParry());
        }

        private IEnumerator StayOnParryStance()
        {
            yield return new WaitForSeconds(MaxTimeOnParryStance);
            
            if(IsParrying)
                base.OnLastFrameEnd();
            
            ThisFighter.MovementController.SetSpeed(ThisFighter.GetBaseProperties().RunSpeed);
        }
        
        private IEnumerator WaitThenEnableParry()
        {
            CanParry = false;
            
            yield return new WaitForSeconds(ParryCooldown);
            
            CanParry = true;
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

            ThisFighter.MovementController.SetSpeed(ThisFighter.GetBaseProperties().RunSpeed);
        }
    }
}
