using System;
using UnityEngine;

namespace Enso.Characters.Player
{
    public class PlayerInput : MonoBehaviour
    {
        public enum MovementState
        {
            Idle,
            Walking,
            Running,
            Sprinting
        }
        
        private bool sprintInputDownCalled;
        private bool attackInputDownCalled;
        private bool attackInputUpCalled;
        private bool guardInputDownCalled;
        private bool guardInputUpCalled;
        private bool dodgeInputDownCalled;
        private bool healInputDownCalled;

        [SerializeField] private KeyCode SprintButton = KeyCode.Joystick1Button1;
        [SerializeField] private KeyCode AttackButton = KeyCode.Joystick1Button5;
        [SerializeField] private KeyCode GuardButton = KeyCode.Joystick1Button4;
        [SerializeField] private KeyCode DodgeButton = KeyCode.Joystick1Button2;
        [SerializeField] private KeyCode HealButton = KeyCode.Joystick1Button0;
    
        public static event Action SprintInputDown;
        public static event Action AttackInputDown;
        public static event Action AttackInputUp;
        public static event Action GuardInputDown;
        public static event Action GuardInputUp;
        public static event Action DodgeInputDown;
        public static event Action HealInputDown;
    
        public static Vector2 Movement;
        public static MovementState ActualMovementState;

        private void Update()
        {
            UpdateMovement();

            sprintInputDownCalled = Input.GetKeyDown(SprintButton);
            attackInputDownCalled = Input.GetKeyDown(AttackButton);
            attackInputUpCalled = Input.GetKeyUp(AttackButton);
            guardInputDownCalled = Input.GetKeyDown(GuardButton);
            guardInputUpCalled = Input.GetKeyUp(GuardButton);
            dodgeInputDownCalled = Input.GetKeyDown(DodgeButton);
            healInputDownCalled = Input.GetKeyDown(HealButton);

            if (sprintInputDownCalled)
            {
                OnSprintInputDown();
            }
            
            if (attackInputDownCalled)
            {
                OnAttackInputDown();
            }

            if (attackInputUpCalled)
            {
                OnAttackInputUp();
            }

            if (guardInputDownCalled)
            {
                OnDefenseInputDown();
            }
        
            if (guardInputUpCalled)
            {
                OnDefenseInputUp();
            }
        
            if (dodgeInputDownCalled)
            {
                OnDodgeInputDown();
            }

            if (healInputDownCalled)
            {
                OnHealInputDown();
            }
        }

        #region Movement
        
        private void UpdateMovement()
        {
            Movement.x = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal"));
            Movement.y = Mathf.RoundToInt(Input.GetAxisRaw("Vertical"));
            Movement.Normalize();
            
            if (Movement != Vector2.zero)
            {
                if (sprintInputDownCalled)
                {
                    ActualMovementState = MovementState.Sprinting;
                }
                else if (Math.Abs(Movement.x) > 0.5f || Math.Abs(Movement.y) > 0.5f)
                {
                    ActualMovementState = MovementState.Running;
                }
                else
                {
                    ActualMovementState = MovementState.Walking;
                }
            }
            else
            {
                ActualMovementState = MovementState.Idle;
            }
        }
        
        private static void OnSprintInputDown()
        {
            SprintInputDown?.Invoke();
        }
        
        #endregion

        #region Combat

        private static void OnAttackInputDown()
        {
            AttackInputDown?.Invoke();
        }

        private static void OnDefenseInputDown()
        {
            GuardInputDown?.Invoke();
        }

        private static void OnDodgeInputDown()
        {
            DodgeInputDown?.Invoke();
        }

        private static void OnDefenseInputUp()
        {
            GuardInputUp?.Invoke();
        }

        private static void OnAttackInputUp()
        {
            AttackInputUp?.Invoke();
        }

        private static void OnHealInputDown()
        {
            HealInputDown?.Invoke();
        }

        #endregion
    }
}