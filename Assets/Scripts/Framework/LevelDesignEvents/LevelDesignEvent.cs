using UnityEngine;

namespace Framework.LevelDesignEvents
{
    public abstract class LevelDesignEvent : MonoBehaviour
    {
        public float DelayAfterExecution;
        
        public virtual void Execute()
        {
            
        }

        public virtual void Exit()
        {
            
        }
    }
}
