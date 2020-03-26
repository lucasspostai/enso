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
        private bool defenseInputDownCalled;
        private bool defenseInputUpCalled;
        private bool dodgeInputDownCalled;

        [SerializeField] private KeyCode SprintButton = KeyCode.Joystick1Button0;
        [SerializeField] private KeyCode AttackButton = KeyCode.Joystick1Button5;
        [SerializeField] private KeyCode DefenseButton = KeyCode.Joystick1Button4;
        [SerializeField] private KeyCode DodgeButton = KeyCode.Joystick1Button1;
    
        public static event Action SprintInputDown;
        public static event Action AttackInputDown;
        public static event Action AttackInputUp;
        public static event Action DefenseInputDown;
        public static event Action DefenseInputUp;
        public static event Action DodgeInputDown;
    
        public static Vector2 Movement;
        public static MovementState ActualMovementState;

        private void Update()
        {
            UpdateMovement();

            sprintInputDownCalled = Input.GetKeyDown(SprintButton);
            attackInputDownCalled = Input.GetKeyDown(AttackButton);
            attackInputUpCalled = Input.GetKeyUp(AttackButton);
            defenseInputDownCalled = Input.GetKeyDown(DefenseButton);
            defenseInputUpCalled = Input.GetKeyUp(DefenseButton);
            dodgeInputDownCalled = Input.GetKeyDown(DodgeButton);

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

            if (defenseInputDownCalled)
            {
                OnDefenseInputDown();
            }
        
            if (defenseInputUpCalled)
            {
                OnDefenseInputUp();
            }
        
            if (dodgeInputDownCalled)
            {
                OnDodgeInputDown();
            }
        }

        #region Movement
        
        private void UpdateMovement()
        {
            Movement.x = Input.GetAxisRaw("Horizontal");
            Movement.y = Input.GetAxisRaw("Vertical");
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
            DefenseInputDown?.Invoke();
        }

        private static void OnDodgeInputDown()
        {
            DodgeInputDown?.Invoke();
        }

        private static void OnDefenseInputUp()
        {
            DefenseInputUp?.Invoke();
        }

        private static void OnAttackInputUp()
        {
            AttackInputUp?.Invoke();
        }

        #endregion
    }
}