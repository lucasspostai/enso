using UnityEngine;

namespace Framework.Animations
{
    [System.Serializable]
    public class FrameChecker
    {
        private IFrameCheckHandler thisFrameCheckHandler;
        private AnimationClipHolder thisAnimationClipHolder;
        private bool checkedHitFrameStart;
        private bool checkedHitFrameEnd;
        private bool checkedCanCut;
        private bool checkedLastFrame;
        
        public int HitFrameStart = 1;
        public int HitFrameEnd = 2;
        public int CanCutFrame;

        public void Initialize(IFrameCheckHandler frameCheckHandler, AnimationClipHolder animationClipHolder)
        {
            thisFrameCheckHandler = frameCheckHandler;
            thisAnimationClipHolder = animationClipHolder;

            ResetProperties();
        }

        public void ResetProperties()
        {
            checkedHitFrameStart = false;
            checkedHitFrameEnd = false;
            checkedCanCut = false;
            checkedLastFrame = false;
        }

        public void CheckFrames()
        {
            if (checkedLastFrame)
            {
                checkedLastFrame = false;
                
                thisFrameCheckHandler.OnLastFrameEnd();
            }

            if (!thisAnimationClipHolder.IsActive())
                return;

            // Hit frame start
            if (!checkedHitFrameStart && thisAnimationClipHolder.IsBiggerOrEqualThanFrame(HitFrameStart))
            {
                thisFrameCheckHandler.OnHitFrameStart();
                checkedHitFrameStart = true;
            } // Hit frame end
            else if (!checkedHitFrameEnd && thisAnimationClipHolder.IsBiggerOrEqualThanFrame(HitFrameEnd))
            {
                thisFrameCheckHandler.OnHitFrameEnd();
                checkedHitFrameEnd = true;
            }
            else if (!checkedCanCut && thisAnimationClipHolder.IsBiggerOrEqualThanFrame(CanCutFrame))
            {
                thisFrameCheckHandler.OnCanCutAnimation();
                checkedCanCut = true;
            }

            if (!checkedLastFrame && thisAnimationClipHolder.ItsOnLastFrame())
            {
                thisFrameCheckHandler.OnLastFrameStart();
                checkedLastFrame = true; //This test is done to make sure the last frame will not be skipped
            }
        }
    }
}
