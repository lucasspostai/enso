using System.Collections;
using UnityEngine;

namespace Framework.LevelDesignEvents
{
    public abstract class LevelDesignEventExecuter : MonoBehaviour
    {
        [SerializeField] protected LevelDesignEvent[] LevelDesignEvents;
        [SerializeField] protected bool DestroyAfterExecution;

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
