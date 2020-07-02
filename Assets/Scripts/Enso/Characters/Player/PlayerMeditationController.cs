using System;
using Enso.CombatSystem;
using UnityEngine;

namespace Enso.Characters.Player
{
    public class PlayerMeditationController : CustomAnimationController
    {
        private Shrine currentShrine;

        [SerializeField] private ActionAnimation MeditateAnimation;
        [SerializeField] private ActionAnimation MeditationLoopAnimation;
        [SerializeField] private ActionAnimation EndMeditationAnimation;

        public bool IsMeditating;

        private void OnDisable()
        {
            PlayerInput.AnyInputDown -= EndMeditation;
        }

        public void StartMeditation(Shrine shrine)
        {
            if (shrine != null)
            {
                ThisFighter.AnimationHandler.SetFacingDirection((shrine.transform.position - transform.position)
                    .normalized);

                currentShrine = shrine;
            }

            IsMeditating = true;

            SetAnimationPropertiesAndPlay(MeditateAnimation.ClipHolder, MeditateAnimation.AnimationFrameChecker);
        }

        public void StartMeditationLoop(Shrine shrine, bool getUpOnAnyButton)
        {
            if (shrine != null)
            {
                ThisFighter.AnimationHandler.SetFacingDirection((shrine.transform.position - transform.position)
                    .normalized);

                currentShrine = shrine;
            }

            if (getUpOnAnyButton)
                PlayerInput.AnyInputDown += EndMeditation;

            IsMeditating = true;

            SetAnimationPropertiesAndPlay(MeditationLoopAnimation.ClipHolder,
                MeditationLoopAnimation.AnimationFrameChecker);
        }

        public void EndMeditation()
        {
            if (currentShrine && currentShrine.IsInteracting)
                return;
            
            currentShrine = null;

            PlayerInput.AnyInputDown -= EndMeditation;

            IsMeditating = false;

            ResetAllProperties();

            SetAnimationPropertiesAndPlay(EndMeditationAnimation.ClipHolder,
                EndMeditationAnimation.AnimationFrameChecker);
        }

        public override void OnLastFrameEnd()
        {
            if (!IsMeditating)
                base.OnLastFrameEnd();
        }

        public override void OnInterrupted()
        {
            if (!IsMeditating)
                base.OnInterrupted();
        }
    }
}