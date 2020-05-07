using Enso.Enums;
using Framework.Animations;
using UnityEngine;

namespace Enso.CombatSystem
{
    [CreateAssetMenu(fileName = "NewAttack", menuName = "Enso/Combat/Attack")]
    public class Attack : ScriptableObject
    {
        public AnimationClipHolder AttackAnimationClipHolder;
        public FrameChecker AttackFrameChecker;
        public Vector3 HitboxSize = new Vector3(1,1,0);
        public int Damage = 1;
        public bool CanBeCut = true;
        public float MovementOffset;
        public AttackType Type;
    }
}
