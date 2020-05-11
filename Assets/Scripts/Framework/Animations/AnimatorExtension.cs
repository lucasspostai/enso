using UnityEngine;

namespace Framework.Animations
{
    public static class AnimatorExtension
    {
        public static bool IsPlayingOnLayer(this Animator animator, int fullPathHash, int layer)
        {
            return animator.GetCurrentAnimatorStateInfo(layer).fullPathHash == fullPathHash;
        }

        public static double NormalizedTime(this Animator animator, int layer)
        {
            double time = animator.GetCurrentAnimatorStateInfo(layer).normalizedTime;
            return time > 1 ? 1 : time;
        }
    }
}
