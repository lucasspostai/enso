using System.Collections;
using Enso;
using Enso.UI;
using UnityEngine;

namespace Framework.LevelDesignEvents
{
    public class NewLocationDiscoveredTrigger : LevelDesignEvent
    {
        [SerializeField] private Element NewLocationDiscoveredElement;
        [SerializeField] private Shrine ThisShrine;
        
        public override void Execute()
        {
            base.Execute();

            if (ThisShrine && ThisShrine.PlayerStartedHere)
                return;
            
            NewLocationDiscoveredElement.gameObject.SetActive(true);
            NewLocationDiscoveredElement.Enable();

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
