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
        [SerializeField] private Hurtbox FighterHurtbox;

        public void SetHitboxResponder(IHitboxResponder responder)
        {
            hitboxResponder = responder;
        }

        public void SetColliderState(ColliderState state)
        {
            colliderState = state;
        }

        public void SetHitBoxSize(Vector3 hitboxSize)
        {
            HitboxSize = hitboxSize;
        }
        
        private void Update()
        {
            if (colliderState == ColliderState.Closed)
                return;

            var collidersOverlapped = Physics2D.OverlapBoxAll(transform.position, HitboxSize, 0, CollisionMask);

            foreach (var colliderOverlapped in collidersOverlapped)
            {
                if(colliderOverlapped != FighterHurtbox.HurtboxCollider) //Ignore own hurtbox
                    hitboxResponder?.CollidedWith(colliderOverlapped);
            }

            SetColliderState(collidersOverlapped.Length > 0 ? ColliderState.Colliding : ColliderState.Open);
        }
        
        private void OnDrawGizmosSelected()
        {
            var previousColor = Gizmos.color;
            var previousMatrix = Gizmos.matrix;

            Gizmos.color = Color.red;
            Gizmos.matrix = transform.localToWorldMatrix;
            
            Gizmos.DrawWireCube(Anchor.localPosition, HitboxSize);

            Gizmos.color = previousColor;
            Gizmos.matrix = previousMatrix;
        }
    }
}