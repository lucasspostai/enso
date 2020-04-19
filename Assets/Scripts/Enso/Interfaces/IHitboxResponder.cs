using UnityEngine;

namespace Enso.Interfaces
{
    public interface IHitboxResponder
    {
        void CollidedWith(Collider2D otherCollider);
    }
}
