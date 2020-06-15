using UnityEngine;

namespace Enso.Interfaces
{
    public interface IDamageable
    {
        void TakeDamage(int damageAmount, Vector3 direction);
    }
}