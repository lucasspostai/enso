using UnityEngine;
using UnityEngine.Serialization;

namespace LevelDesignEvents
{
    public abstract class LevelDesignEvent : MonoBehaviour
    {
        public float DelayAfterExecution;
        
        public virtual void Execute()
        {
            
        }
    }
}
