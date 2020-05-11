using System.Collections;
using UnityEngine;

namespace Framework.LevelDesignEvents
{
    public abstract class LevelDesignEventExecuter : MonoBehaviour
    {
        [SerializeField] private LevelDesignEvent[] LevelDesignEvents;
        [SerializeField] private bool DestroyAfterExecution;

        public void ExecuteEvents()
        {
            StartCoroutine(WaitForEventDelayThenExecute());
        }

        private IEnumerator WaitForEventDelayThenExecute()
        {
            foreach (var levelDesignEvent in LevelDesignEvents)
            {
                levelDesignEvent.Execute();
                yield return new WaitForSeconds(levelDesignEvent.DelayAfterExecution);
            }
            
            if(DestroyAfterExecution)
                Destroy(gameObject);
        }
    }
}
