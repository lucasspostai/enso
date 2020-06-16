using Framework.Audio;
using UnityEngine;

namespace Framework.Animations
{
    [System.Serializable]
    public class FrameChecker
    {
        public FrameChecker Clone()
        {
            return MemberwiseClone() as FrameChecker;
        }
        
        private IFrameCheckHandler thisFrameCheckHandler;
        private AnimationClipHolder thisAnimationClipHolder;
        private bool checkedStartHitFrame;
        private bool checkedEndHitFrame;
        private bool checkedPlayAudioFrame;
        private bool checkedStartMovementFrame;
        private bool checkedEndMovementFrame;
        private bool checkedCanCutFrame;
        private bool checkedLastFrame;

        public bool DealDamage = true;
        public int StartHitFrame = 1;
        public int EndHitFrame = 2;
        public int CanCutFrame = 3;
        public int PlayAudioFrame = 0;
        public int StartMovementFrame = 0;
        public int EndMovementFrame = 2;
        public float MovementOffset;
        public SoundCue AnimationSoundCue;

        public void Initialize(IFrameCheckHandler frameCheckHandler, AnimationClipHolder animationClipHolder)
        {
            thisFrameCheckHandler = frameCheckHandler;
            thisAnimationClipHolder = animationClipHolder;

            ResetProperties();
        }

        public void ResetProperties()
        {
            checkedStartHitFrame = false;
            checkedEndHitFrame = false;
            checkedCanCutFrame = false;
            checkedLastFrame = false;
            checkedStartMovementFrame = false;
            checkedEndMovementFrame = false;
            checkedPlayAudioFrame = false;
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
            if (!checkedStartHitFrame && thisAnimationClipHolder.IsBiggerOrEqualThanFrame(StartHitFrame) && DealDamage)
            {
                thisFrameCheckHandler.OnHitFrameStart();
                checkedStartHitFrame = true;
            } // Hit frame end
            else if (!checkedEndHitFrame && thisAnimationClipHolder.IsBiggerOrEqualThanFrame(EndHitFrame))
            {
                thisFrameCheckHandler.OnHitFrameEnd();
                checkedEndHitFrame = true;
            }
            
            //Can Cut
            if (!checkedCanCutFrame && thisAnimationClipHolder.IsBiggerOrEqualThanFrame(CanCutFrame))
            {
                thisFrameCheckHandler.OnCanCutAnimation();
                checkedCanCutFrame = true;
            }
            
            //Play Audio
            if (!checkedPlayAudioFrame && thisAnimationClipHolder.IsBiggerOrEqualThanFrame(PlayAudioFrame))
            {
                thisFrameCheckHandler.OnPlayAudio();
                checkedPlayAudioFrame = true;
            }

            //Start Movement
            if (!checkedStartMovementFrame && thisAnimationClipHolder.IsBiggerOrEqualThanFrame(StartMovementFrame))
            {
                thisFrameCheckHandler.OnStartMovement();
                checkedStartMovementFrame = true;
            }
            
            //End Movement
            if (!checkedEndMovementFrame && thisAnimationClipHolder.IsBiggerOrEqualThanFrame(EndMovementFrame))
            {
                thisFrameCheckHandler.OnEndMovement();
                checkedEndMovementFrame = true;
            }

            //Last Frame
            if (!checkedLastFrame && thisAnimationClipHolder.ItsOnLastFrame())
            {
                thisFrameCheckHandler.OnLastFrameStart();
                checkedLastFrame = true; //This test is done to make sure the last frame will not be skipped
            }
        }
    }
}
