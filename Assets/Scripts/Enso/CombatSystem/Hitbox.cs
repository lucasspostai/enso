using Enso.Characters.Player;
using Enso.Enums;
using Enso.Interfaces;
using UnityEngine;

namespace Enso.CombatSystem
{
    public class Hitbox : MonoBehaviour
    {
        private IHitboxResponder hitboxResponder;
        private ColliderState colliderState;
        
        [SerializeField] private LayerMask CollisionMask;
        [SerializeField] private Transform Anchor;
        [SerializeField] private Vector3 HitboxSize;

        public void SetHitboxResponder(IHitboxResponder responder)
        {
            hitboxResponder = responder;
        }
        
        private void Update()
        {
            if (colliderState == ColliderState.Closed)
                return;

            var collidersOverlapped = Physics2D.OverlapBoxAll(transform.position, HitboxSize, 0, CollisionMask);

            foreach (var colliderOverlapped in collidersOverlapped)
            {
                hitboxResponder?.CollidedWith(colliderOverlapped);
            }

            colliderState = collidersOverlapped.Length > 0 ? ColliderState.Colliding : ColliderState.Open;
        }
        
        private void OnDrawGizmosSelected()
        {
            var previousColor = Gizmos.color;
            var previousMatrix = Gizmos.matrix;

            Gizmos.color = Color.green;
            Gizmos.matrix = transform.localToWorldMatrix;
            
            Gizmos.DrawWireCube(Anchor.position, HitboxSize);

            Gizmos.color = previousColor;
            Gizmos.matrix = previousMatrix;
        }
    }
}