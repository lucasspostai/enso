using System.Collections;
using Enso.CombatSystem;
using UnityEngine;

namespace Enso.Characters.Enemies
{
    public abstract class EnemyAttackController : AttackController
    {
        private bool mustWaitAfterCompletion;
        private Coroutine waitAfterCompletionCoroutine;
        private float waitTime = 1f;
        
        [HideInInspector] public bool CanAttack = true;
        
        protected virtual void Wait()
        {
            if (waitAfterCompletionCoroutine != null)
            {
                StopCoroutine(waitAfterCompletionCoroutine);
            }

            waitAfterCompletionCoroutine = StartCoroutine(WaitThenEnableAttack());
        }
        
        private IEnumerator WaitThenEnableAttack()
        {
            yield return mustWaitAfterCompletion ? new WaitForSeconds(waitTime) : null;

            mustWaitAfterCompletion = false;
            CanAttack = true;
        }
        
        public void WaitAfterAttack(float time)
        {
            mustWaitAfterCompletion = true;
            waitTime = time;
        }

        public override void OnInterrupted()
        {
            base.OnInterrupted();
            
            mustWaitAfterCompletion = false;
            CanAttack = true;
        }

        protected void RotateTowardsTarget()
        {
            ThisFighter.AnimationHandler.SetFacingDirection((ThisFighter.Target.position - transform.position)
                .normalized);
        }
    }
}
