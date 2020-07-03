using System.Collections;
using Enso.UI;
using UnityEngine;

namespace Framework.LevelDesignEvents
{
    public class TutorialTrigger : LevelDesignEvent
    {
        private bool hasInteracted;
        
        [SerializeField] private Element TutorialElement;
        
        public override void Execute()
        {
            base.Execute();

            if (hasInteracted)
                return;
            
            TutorialElement.gameObject.SetActive(true);

            hasInteracted = true;

            StartCoroutine(WaitThenDisable());
        }

        private IEnumerator WaitThenDisable()
        {
            yield return new WaitForSeconds(10f);
            
            TutorialElement.Destroy();
            gameObject.SetActive(false);
        }
    }
}
