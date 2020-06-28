using System.Collections;
using Enso.CombatSystem;
using UnityEngine;

namespace Enso.Characters.Enemies
{
    public class EnemyRollController : RollController
    {
        private bool mustWaitAfterCompletion;
        private Coroutine waitAfterCompletionCoroutine;
        private float waitTime = 1f;
        
        [HideInInspector] public bool CanRoll = true;
        
        protected override void SetDirection()
        {
            base.SetDirection();

            RotateTowardsTarget(false);
        }
        
        public override void OnLastFrameEnd()
        {
            base.OnLastFrameEnd();

            RotateTowardsTarget(true);
        }

        public override void PlayRollAnimation()
        {
            base.PlayRollAnimation();

            CanRoll = false;
        }

        private void RotateTowardsTarget(bool towardsTarget)
        {
            var characterAnimationHandler = ThisFighter.Target.GetComponent<CharacterAnimationHandler>();

            if (!characterAnimationHandler)
                return;
            
            if (towardsTarget)
            {
                ThisFighter.AnimationHandler.SetFacingDirection((characterAnimationHandler.CurrentDirection * -1).normalized);
            }
            else
            {
                ThisFighter.AnimationHandler.SetFacingDirection(new Vector3(
                    characterAnimationHandler.CurrentDirection.y, -characterAnimationHandler.CurrentDirection.x));
            }
        }
        
        protected virtual void Wait()
        {
            if (waitAfterCompletionCoroutine != null)
            {
                StopCoroutine(waitAfterCompletionCoroutine);
            }

            waitAfterCompletionCoroutine = StartCoroutine(WaitThenEnableRoll());
        }

        private IEnumerator WaitThenEnableRoll()
        {
            yield return mustWaitAfterCompletion ? new WaitForSeconds(waitTime) : null;

            mustWaitAfterCompletion = false;
            CanRoll = true;
        }
        
        public void WaitAfterRoll(float time)
        {
            mustWaitAfterCompletion = true;
            waitTime = time;
        }
        
        public override void OnInterrupted()
        {
            base.OnInterrupted();

            mustWaitAfterCompletion = false;
            CanRoll = true;
        }

        public override void OnLastFrameStart()
        {
            base.OnLastFrameStart();
            
            Wait();
        }
    }
}