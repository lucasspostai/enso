using UnityEngine;

namespace Framework.Animations
{
    [System.Serializable]
    public class FrameChecker
    {
        private IFrameCheckHandler thisFrameCheckHandler;
        private ExtendedAnimationClip thisExtendedAnimationClip;
        private bool checkedHitFrameStart;
        private bool checkedHitFrameEnd;
        private bool lastFrame;
        
        public int HitFrameStart;
        public int HitFrameEnd;
        public int TotalFrames;

        public void Initialize(IFrameCheckHandler frameCheckHandler, ExtendedAnimationClip extendedAnimationClip)
        {
            thisFrameCheckHandler = frameCheckHandler;
            thisExtendedAnimationClip = extendedAnimationClip;
            TotalFrames = extendedAnimationClip.GetTotalFrames();

            ResetProperties();
        }

        public void ResetProperties()
        {
            checkedHitFrameStart = false;
            checkedHitFrameEnd = false;
            lastFrame = false;
        }

        public void CheckFrames()
        {
            if (lastFrame)
            {
                lastFrame = false;
                
                thisFrameCheckHandler.OnLastFrameEnd();
            }

            if (!thisExtendedAnimationClip.IsActive())
                return;

            // Hit frame start
            if (!checkedHitFrameStart && thisExtendedAnimationClip.IsBiggerOrEqualThanFrame(HitFrameStart))
            {
                thisFrameCheckHandler.OnHitFrameStart();
                checkedHitFrameStart = true;
            } // Hit frame end
            else if (!checkedHitFrameEnd && thisExtendedAnimationClip.IsBiggerOrEqualThanFrame(HitFrameEnd))
            {
                thisFrameCheckHandler.OnHitFrameEnd();
                checkedHitFrameEnd = true;
            }

            if (!lastFrame && thisExtendedAnimationClip.ItsOnLastFrame())
            {
                thisFrameCheckHandler.OnLastFrameStart();
                lastFrame = true; //This test is done to make sure the last frame will not be skipped
            }
        }
    }
}
