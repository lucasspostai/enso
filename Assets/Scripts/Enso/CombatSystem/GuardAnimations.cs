using Framework.Animations;
using UnityEngine;

namespace Enso.CombatSystem
{
    [CreateAssetMenu(fileName = "NewGuardAnimations", menuName = "Enso/Combat/Guard Animations")]
    public class GuardAnimations : ScriptableObject
    {
        [Header("Guard Animations")] 
        public AnimationClipHolder StartGuardAnimationClipHolder;
        public AnimationClipHolder GuardIdleAnimationClipHolder;
        public AnimationClipHolder EndGuardAnimationClipHolder;
        
        [Header("Guard Walk Animations")] 
        public AnimationClipHolder ForwardGuardWalkAnimationClipHolder;
        public AnimationClipHolder BackwardGuardWalkAnimationClipHolder;
        public AnimationClipHolder LeftGuardWalkAnimationClipHolder;
        public AnimationClipHolder RightGuardWalkAnimationClipHolder;
        
        [Header("Guard Actions Animations")] 
        public AnimationClipHolder BlockAnimationClipHolder;
        public AnimationClipHolder ParryAnimationClipHolder;
    }
}
