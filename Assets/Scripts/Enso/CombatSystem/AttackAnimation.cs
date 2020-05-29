using Enso.Enums;
using Framework.Animations;
using UnityEngine;

namespace Enso.CombatSystem
{
    [CreateAssetMenu(fileName = "NewAttackAnimation", menuName = "Enso/Combat/Attack Animation")]
    public class AttackAnimation : ActionAnimation
    {
        public Vector3 HitboxSize = new Vector3(1,1,0);
        public int Damage = 1;
        public AttackType Type;
    }
}
