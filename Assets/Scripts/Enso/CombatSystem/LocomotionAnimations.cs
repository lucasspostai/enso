using Framework.Animations;
using UnityEngine;

namespace Enso.CombatSystem
{
    [CreateAssetMenu(fileName = "NewLocomotionAnimations", menuName = "Enso/Combat/Locomotion Animations")]
    public class LocomotionAnimations : ScriptableObject
    {
        public string FaceX = "FaceX";
        public string FaceY = "FaceY";
        public AnimationClipHolder IdleAnimationClipHolder;
        public AnimationClipHolder WalkAnimationClipHolder;
        public AnimationClipHolder RunAnimationClipHolder;
        public AnimationClipHolder SprintAnimationClipHolder;
    }
}
