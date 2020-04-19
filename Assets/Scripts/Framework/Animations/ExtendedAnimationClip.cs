using UnityEngine;

namespace Framework.Animations
{
    [System.Serializable]
    public class ExtendedAnimationClip
    {
        private int totalFrames;
        private int animationFullNameHash;
        
        public Animator ThisAnimator;
        public AnimationClip ThisAnimationClip;
        public string AnimatorStateName;
        public int LayerNumber;

        public void Initialize()
        {
            totalFrames = Mathf.RoundToInt(ThisAnimationClip.length * ThisAnimationClip.frameRate);

            if (ThisAnimator.isActiveAndEnabled)
            {
                string name = ThisAnimator.GetLayerName(LayerNumber) + "." + AnimatorStateName;
                animationFullNameHash = Animator.StringToHash(name);
            }
        }

        public int GetTotalFrames()
        {
            return totalFrames;
        }

        public bool IsActive()
        {
            return ThisAnimator.IsPlayingOnLayer(animationFullNameHash, 0);
        }

        private double PercentageOnFrame(int frameNumber)
        {
            return (double) frameNumber / totalFrames;
        }

        public bool IsBiggerOrEqualThanFrame(int frameNumber)
        {
            double percentage = ThisAnimator.NormalizedTime(LayerNumber);
            return percentage >= PercentageOnFrame(frameNumber);
        }

        public bool ItsOnLastFrame()
        {
            double percentage = ThisAnimator.NormalizedTime(LayerNumber);
            return percentage > PercentageOnFrame(totalFrames - 1);
        }
    }
}
