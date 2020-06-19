using System.Collections;
using Enso.CombatSystem;
using UnityEngine;

namespace Enso.Characters.Enemies
{
    public class EnemyGuardController : GuardController
    {
        private bool mustWaitAfterStartGuard;
        private Coroutine waitAfterStartGuardCoroutine;
        private Coroutine waitAfterEndGuardCoroutine;
        private float waitAfterStartGuardTime = 1f;
        private float waitAfterEndGuardTime = 1f;

        [HideInInspector] public bool CanGuard;

        protected override void Start()
        {
            base.Start();

            CanGuard = true;
        }

        public override void StartGuard()
        {
            base.StartGuard();

            CanGuard = false;
        }

        public override void EndGuard()
        {
            StartingGuard = false;
            IsGuarding = false;
            
            base.EndGuard();
            
            if(waitAfterStartGuardCoroutine != null)
                StopCoroutine(waitAfterStartGuardCoroutine);
            
            if(waitAfterEndGuardCoroutine != null)
                StopCoroutine(waitAfterEndGuardCoroutine);
        }

        public void WaitAfterStartGuard(float time)
        {
            waitAfterStartGuardTime = time;
            
            if(waitAfterStartGuardCoroutine != null)
                StopCoroutine(waitAfterStartGuardCoroutine);

            waitAfterStartGuardCoroutine = StartCoroutine(WaitThenStopGuard());
        }

        private IEnumerator WaitThenStopGuard()
        {
            yield return new WaitForSeconds(waitAfterStartGuardTime);
            
            EndGuard();
        }
        
        public void WaitAfterEndGuard(float time)
        {
            waitAfterEndGuardTime = time;
        }
        
        private IEnumerator WaitThenEnableGuard()
        {
            yield return new WaitForSeconds(waitAfterEndGuardTime);

            CanGuard = true;
            waitAfterEndGuardTime = 0;
        }

        public override void OnLastFrameEnd()
        {
            if (EndingGuard)
            {
                if(waitAfterEndGuardCoroutine != null)
                    StopCoroutine(waitAfterEndGuardCoroutine);

                waitAfterEndGuardCoroutine = StartCoroutine(WaitThenEnableGuard());
            }

            base.OnLastFrameEnd();
        }
    }
}
