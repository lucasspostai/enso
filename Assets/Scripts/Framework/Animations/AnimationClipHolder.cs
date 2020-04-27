using System.Collections.Generic;
using UnityEngine;

namespace Framework.Animations
{
    [System.Serializable]
    public class AnimationClipHolder
    {
        private Animator thisAnimator;
        private int totalFrames;
        private int animationFullNameHash;

        public List<AnimationClip> AnimationClips = new List<AnimationClip>(8);
        public string AnimatorStateName = "AnimatorStateName";
        public int LayerNumber;

        public void Initialize(Animator animator)
        {
            thisAnimator = animator;
            
            totalFrames = Mathf.RoundToInt(AnimationClips[0].length * AnimationClips[0].frameRate);

            if (thisAnimator.isActiveAndEnabled)
            {
                string name = animator.GetLayerName(LayerNumber) + "." + AnimatorStateName;
                animationFullNameHash = Animator.StringToHash(name);
            }
        }

        public int GetTotalFrames()
        {
            return totalFrames;
        }

        public bool IsActive()
        {
            return thisAnimator.IsPlayingOnLayer(animationFullNameHash, 0);
        }

        private double PercentageOnFrame(int frameNumber)
        {
            return (double) frameNumber / totalFrames;
        }

        public bool IsBiggerOrEqualThanFrame(int frameNumber)
        {
            double percentage = thisAnimator.NormalizedTime(LayerNumber);
            return percentage >= PercentageOnFrame(frameNumber);
        }

        public bool ItsOnLastFrame()
        {
            double percentage = thisAnimator.NormalizedTime(LayerNumber);
            return percentage > PercentageOnFrame(totalFrames - 1);
        }
    }
}
