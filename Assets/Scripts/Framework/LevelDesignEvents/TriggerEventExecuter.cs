using UnityEngine;

namespace Framework.LevelDesignEvents
{
    [RequireComponent(typeof(Collider2D))]
    public class TriggerEventExecuter : LevelDesignEventExecuter
    {
        public void ExitTrigger()
        {
            foreach (var levelDesignEvent in LevelDesignEvents)
            {
                levelDesignEvent.Exit();
            }
        }
    }
}