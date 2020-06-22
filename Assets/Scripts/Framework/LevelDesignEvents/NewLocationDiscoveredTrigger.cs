using System.Collections;
using Enso.UI;
using UnityEngine;

namespace Framework.LevelDesignEvents
{
    public class NewLocationDiscoveredTrigger : LevelDesignEvent
    {
        [SerializeField] private Element NewLocationDiscoveredElement;
        
        public override void Execute()
        {
            base.Execute();
            
            NewLocationDiscoveredElement.gameObject.SetActive(true);
            NewLocationDiscoveredElement.Enable();

            StartCoroutine(WaitThenDisable());
        }

        private IEnumerator WaitThenDisable()
        {
            yield return new WaitForSeconds(2f);
            
            NewLocationDiscoveredElement.Disable();
            
            yield return new WaitForSeconds(1f);
            
            NewLocationDiscoveredElement.Destroy();
            gameObject.SetActive(false);
        }
    }
}
