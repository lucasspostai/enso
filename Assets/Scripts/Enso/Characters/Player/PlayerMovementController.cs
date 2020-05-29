using System.Collections;
using Framework;
using UnityEngine;

namespace Enso.Characters.Player
{
    [RequireComponent(typeof(Player))]
    public class PlayerMovementController : CharacterMovementController
    {
        private Coroutine sprintCoroutine;
        private Player player;

        [SerializeField] private float SprintDeadZoneTime = 0.2f;
        [SerializeField] private Transform HitboxAnchor;

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

        protected override void Start()
        {
            base.Start();
            
            player = GetComponent<Player>();
        }

        protected override void Update()
        {
            SetMovement(PlayerInput.Movement);
            
            base.Update();

            UpdateHitBoxAnchorRotation();
        }

        private void UpdateHitBoxAnchorRotation()
        {
            float angle = Mathf.Atan2(Velocity.y, Velocity.x) * Mathf.Rad2Deg;
            HitboxAnchor.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        private void TryToSprint()
        {
            if(sprintCoroutine != null)
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
            if(sprintCoroutine != null)
                StopCoroutine(sprintCoroutine);

            SetRegularRunSpeed();
        }
    }
}