using System;
using Enso.CombatSystem;
using UnityEngine;

namespace Enso.Characters.Player
{
    public class PlayerMeditationController : CustomAnimationController
    {
        private bool isMeditating;
        private Shrine currentShrine;

        [SerializeField] private ActionAnimation MeditateAnimation;
        [SerializeField] private ActionAnimation EndMeditationAnimation;

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
            else
            {
                PlayerInput.AnyInputDown += EndMeditation;
            }

            isMeditating = true;

            SetAnimationPropertiesAndPlay(MeditateAnimation.ClipHolder, MeditateAnimation.AnimationFrameChecker);
        }

        public void EndMeditation()
        {
            currentShrine = null;

            PlayerInput.AnyInputDown -= EndMeditation;

            isMeditating = false;

            ResetAllProperties();

            SetAnimationPropertiesAndPlay(EndMeditationAnimation.ClipHolder,
                EndMeditationAnimation.AnimationFrameChecker);
        }

        public override void OnLastFrameEnd()
        {
            if (!isMeditating)
                base.OnLastFrameEnd();
        }

        public override void OnInterrupted()
        {
            if (!isMeditating)
                base.OnInterrupted();
        }
    }
}