using Framework.Animations;
using UnityEngine;

namespace Enso.CombatSystem
{
    [CreateAssetMenu(fileName = "NewDamageAnimations", menuName = "Enso/Combat/Damage Animations")]
    public class DamageAnimations : ScriptableObject
    {
        public Damage TestDamage;
        public AnimationClipHolder LoseBalanceAnimationClipHolder;
        public AnimationClipHolder DamageAnimationClipHolder;
        public AnimationClipHolder HeavyAnimationClipHolder;
        public AnimationClipHolder DeathAnimationClipHolder;
    }
}
