using System;
using Framework;
using UnityEngine;

namespace Enso.Characters.Player
{
    [RequireComponent(typeof(Player))]
    public class PlayerMovementController : CharacterMovementController
    {
        private Player player;

        [SerializeField] private Transform HitboxAnchor;

        private void OnEnable()
        {
            PlayerInput.SprintInputDown += SetSprintSpeed;
            PlayerInput.SprintInputUp += SetRegularRunSpeed;
        }

        private void OnDisable()
        {
            PlayerInput.SprintInputDown -= SetSprintSpeed;
            PlayerInput.SprintInputUp -= SetRegularRunSpeed;
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

        private void StartSprint()
        {
            
        }
    }
}