using System.Collections;
using Framework;
using UnityEngine;

namespace Enso.Characters.Player
{
    [RequireComponent(typeof(Player))]
    public class PlayerMovementController : CharacterMovementController
    {
        private Coroutine sprintCoroutine;
        
        [SerializeField] private float SprintDeadZoneTime = 0.2f;

        private void OnEnable()
        {
            PlayerInput.SprintInputDown += TryToSprint;
            PlayerInput.SprintInputUp += CancelSprint;
        }

        private void OnDisable()
        {
            PlayerInput.SprintInputDown -= TryToSprint;
            PlayerInput.SprintInputUp -= CancelSprint;
        }

        protected override void Update()
        {
            SetMovement(PlayerInput.Movement);

            base.Update();
        }

        private void TryToSprint()
        {
            if (sprintCoroutine != null)
                StopCoroutine(sprintCoroutine);

            sprintCoroutine = StartCoroutine(Sprint());
        }

        private IEnumerator Sprint()
        {
            yield return new WaitForSeconds(SprintDeadZoneTime);

            SetSprintSpeed();
        }

        private void CancelSprint()
        {
            if (sprintCoroutine != null)
                StopCoroutine(sprintCoroutine);

            SetRegularRunSpeed();
        }
    }
}