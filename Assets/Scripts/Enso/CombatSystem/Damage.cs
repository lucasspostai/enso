using Framework.Animations;

namespace Enso.CombatSystem
{
    [System.Serializable]
    public class Damage
    {
        public AnimationClipHolder DamageAnimationClipHolder;
        public FrameChecker DamageFrameChecker;
        public float MovementOffset;
    }
}
