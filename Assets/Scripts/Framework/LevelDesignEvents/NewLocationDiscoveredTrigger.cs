using System.Collections;
using Enso;
using Enso.UI;
using UnityEngine;

namespace Framework.LevelDesignEvents
{
    public class NewLocationDiscoveredTrigger : LevelDesignEvent
    {
        private bool hasInteracted;
        
        [SerializeField] private Element NewLocationDiscoveredElement;
        [SerializeField] private Shrine ThisShrine;
        
        public override void Execute()
        {
            base.Execute();

            if (ThisShrine && ThisShrine.PlayerStartedHere || hasInteracted)
                return;
            
            NewLocationDiscoveredElement.gameObject.SetActive(true);
            NewLocationDiscoveredElement.Enable();

            hasInteracted = true;

            StartCoroutine(WaitThenDisable());
        }

        private IEnumerator WaitThenDisable()
        {
            yield return new WaitForSeconds(3f);
            
            NewLocationDiscoveredElement.Disable();
            
            yield return new WaitForSeconds(1f);
            
            NewLocationDiscoveredElement.Destroy();
            gameObject.SetActive(false);
        }
    }
}
