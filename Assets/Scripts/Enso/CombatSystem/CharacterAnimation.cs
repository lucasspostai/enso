using Framework.Animations;
using UnityEngine;

namespace Enso.CombatSystem
{
    public abstract class CharacterAnimation : ScriptableObject
    {
        public AnimationClipHolder AttackAnimationClipHolder;
        public FrameChecker AttackFrameChecker;
        public bool CanBeCut = true;
    }
}
