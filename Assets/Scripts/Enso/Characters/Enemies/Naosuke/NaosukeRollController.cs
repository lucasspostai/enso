using Enso.CombatSystem;
using UnityEngine;

namespace Enso.Characters.Enemies.Naosuke
{
    public class NaosukeRollController : RollController
    {
        private Naosuke naosuke;

        protected override void Start()
        {
            base.Start();

            naosuke = GetComponent<Naosuke>();
        }

        protected override void SetDirection()
        {
            base.SetDirection();

            RotateTowardsTarget(false);
        }
        
        public override void OnLastFrameEnd()
        {
            base.OnLastFrameEnd();

            RotateTowardsTarget(true);
        }

        private void RotateTowardsTarget(bool towardsTarget)
        {
            var characterAnimationHandler = naosuke.Target.GetComponent<CharacterAnimationHandler>();

            if (!characterAnimationHandler)
                return;
            
            if (towardsTarget)
            {
                ThisFighter.AnimationHandler.SetFacingDirection((characterAnimationHandler.CurrentDirection * -1).normalized);
            }
            else
            {
                ThisFighter.AnimationHandler.SetFacingDirection(new Vector3(
                    characterAnimationHandler.CurrentDirection.y, -characterAnimationHandler.CurrentDirection.x));
            }
        }
    }
}