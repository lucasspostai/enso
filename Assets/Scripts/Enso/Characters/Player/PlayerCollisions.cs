using System.Collections.Generic;
using Framework;
using Framework.LevelDesignEvents;
using UnityEngine;

namespace Enso.Characters.Player
{
    public class PlayerCollisions : CharacterCollisions
    {
        private readonly List<TriggerEventExecuter> currentFrameTriggers = new List<TriggerEventExecuter>();
        private readonly List<TriggerEventExecuter> triggerEventExecuters = new List<TriggerEventExecuter>();
        
        [SerializeField] private LayerMask TriggerCollisionLayerMask;

        private void Update()
        {
            GetTriggerCollisions();
        }

        private void GetTriggerCollisions()
        {
            var triggers = Physics2D.OverlapBoxAll(transform.position, Collider.bounds.size, 0, TriggerCollisionLayerMask);
            
            foreach (var trigger in triggers)
            {
                var levelDesignEventExecuter = trigger.gameObject.GetComponent<TriggerEventExecuter>();
                
                if (levelDesignEventExecuter)
                {
                    if (!triggerEventExecuters.Contains(levelDesignEventExecuter))
                    {
                        triggerEventExecuters.Add(levelDesignEventExecuter);
                    
                        levelDesignEventExecuter.ExecuteEvents();
                    }
                    
                    currentFrameTriggers.Add(levelDesignEventExecuter);
                }
            }

            for (int i = triggerEventExecuters.Count - 1; i >= 0 ; i--)
            {
                if (!currentFrameTriggers.Contains(triggerEventExecuters[i]))
                {
                    triggerEventExecuters[i].ExitTrigger();
                    triggerEventExecuters.Remove(triggerEventExecuters[i]);
                }
            }
            
            currentFrameTriggers.Clear();
        }
    }
}
