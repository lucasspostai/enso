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

            if (thisAnimator.isActiveAndEnabled)
            {
                string name = animator.GetLayerName(LayerNumber) + "." + AnimatorStateName;
                animationFullNameHash = Animator.StringToHash(name);
            }
        }

        public int GetTotalFrames()
        {
            return Mathf.RoundToInt(AnimationClips[0].length * AnimationClips[0].frameRate);
        }

        public bool IsActive()
        {
            return thisAnimator.IsPlayingOnLayer(animationFullNameHash, 0);
        }

        public int GetAnimationFullNameHash()
        {
            return animationFullNameHash;
        }

        private double PercentageOnFrame(int frameNumber)
        {
            return (double) frameNumber / GetTotalFrames();
        }

        public bool IsBiggerOrEqualThanFrame(int frameNumber)
        {
            double percentage = thisAnimator.NormalizedTime(LayerNumber);
            return percentage >= PercentageOnFrame(frameNumber);
        }

        public bool ItsOnLastFrame()
        {
            double percentage = thisAnimator.NormalizedTime(LayerNumber);
            return percentage > PercentageOnFrame(GetTotalFrames() - 1);
        }

        public float GetNextFramePercentage()
        {
            if (ItsOnLastFrame())
                return 0;
            
            int nextFrame = (int)(thisAnimator.NormalizedTime(LayerNumber) * AnimationClips[0].frameRate) + 1;
            Debug.Log(nextFrame);
            
            return (float)PercentageOnFrame(nextFrame);
        }
    }
}
