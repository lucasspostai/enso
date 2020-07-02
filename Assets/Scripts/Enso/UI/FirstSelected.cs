using UnityEngine;
using UnityEngine.EventSystems;

namespace Enso.UI
{
    public class FirstSelected : MonoBehaviour
    {
        private void OnEnable()
        {
            var eventSystem = EventSystem.current;

            if (eventSystem)
            {
                eventSystem.SetSelectedGameObject(null);
                eventSystem.SetSelectedGameObject(gameObject, new BaseEventData(eventSystem));
            }
        }
    }
}