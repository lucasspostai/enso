using System;
using Framework;
using Framework.LevelDesignEvents;
using UnityEngine;

namespace Enso.Characters.Player
{
    public class PlayerCollisions : CharacterCollisions
    {
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
                var levelDesignEventExecuter = trigger.gameObject.GetComponent<LevelDesignEventExecuter>();
                
                if (levelDesignEventExecuter)
                {
                    levelDesignEventExecuter.ExecuteEvents();
                }
            }
        }
    }
}
