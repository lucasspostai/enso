using UnityEngine;

namespace Enso.Characters.Enemies
{
    public abstract class Enemy : Fighter
    {
        [Header("Default References")]
        public Animator Animator;
    }
}
