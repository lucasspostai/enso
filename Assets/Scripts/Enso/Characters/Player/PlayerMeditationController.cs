using Enso.CombatSystem;
using UnityEngine;

namespace Enso.Characters.Player
{
    public class PlayerMeditationController : CustomAnimationController
    {
        private bool isMeditating;
        
        [SerializeField] private ActionAnimation MeditateAnimation;
        [SerializeField] private ActionAnimation EndMeditationAnimation;
        
        public void StartMeditation(Shrine shrine)
        {
            //ThisFighter.MovementController.Velocity = Vector3.zero;
            
            if (shrine != null)
                ThisFighter.AnimationHandler.SetFacingDirection((shrine.transform.position - transform.position)
                    .normalized);

            isMeditating = true;

            SetAnimationPropertiesAndPlay(MeditateAnimation.ClipHolder, MeditateAnimation.AnimationFrameChecker);
        }

        public void EndMeditation()
        {
            isMeditating = false;
            
            ResetAllProperties();

            SetAnimationPropertiesAndPlay(EndMeditationAnimation.ClipHolder, EndMeditationAnimation.AnimationFrameChecker);
        }
        
        public override void OnLastFrameEnd()
        {
            if(!isMeditating)
                base.OnLastFrameEnd();
        }

        public override void OnInterrupted()
        {
            if(!isMeditating)
                base.OnInterrupted();
        }
    }
}
