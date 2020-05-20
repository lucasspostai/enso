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
        private bool sprintInputUpCalled;
        private bool attackInputDownCalled;
        private bool attackInputUpCalled;
        private bool guardInputDownCalled;
        private bool guardInputUpCalled;
        private bool rollInputDownCalled;
        private bool healInputDownCalled;
        private bool specialAttackInputDownCalled;
        private float specialAttackInputPressedTime;

        [SerializeField] private KeyCode SprintButton = KeyCode.Joystick1Button1;
        [SerializeField] private KeyCode AttackButton = KeyCode.Joystick1Button5;
        [SerializeField] private KeyCode GuardButton = KeyCode.Joystick1Button4;
        [SerializeField] private KeyCode RollButton = KeyCode.Joystick1Button2;
        [SerializeField] private KeyCode HealButton = KeyCode.Joystick1Button0;
        [SerializeField] private float SpecialAttackDeadZone = 0.1f;

        public static event Action SprintInputDown;
        public static event Action SprintInputUp;
        public static event Action AttackInputDown;
        public static event Action AttackInputUp;
        public static event Action GuardInputDown;
        public static event Action GuardInputUp;
        public static event Action RollInputDown;
        public static event Action HealInputDown;
        public static event Action SpecialAttackInputDown;

        public static Vector2 Movement;

        private void Update()
        {
            UpdateMovement();

            sprintInputDownCalled = Input.GetKeyDown(SprintButton);
            sprintInputUpCalled = Input.GetKeyUp(SprintButton);
            attackInputDownCalled = Input.GetKeyDown(AttackButton);
            attackInputUpCalled = Input.GetKeyUp(AttackButton);
            guardInputDownCalled = Input.GetKeyDown(GuardButton);
            guardInputUpCalled = Input.GetKeyUp(GuardButton);
            rollInputDownCalled = Input.GetKeyDown(RollButton);
            healInputDownCalled = Input.GetKeyDown(HealButton);
            specialAttackInputDownCalled = Input.GetKey(AttackButton) && Input.GetKey(GuardButton) &&
                                           specialAttackInputPressedTime < SpecialAttackDeadZone;

            if (sprintInputDownCalled)
            {
                OnSprintInputDown();
            }
            else if (sprintInputUpCalled)
            {
                OnSprintInputUp();
            }

            if (specialAttackInputDownCalled)
            {
                specialAttackInputPressedTime = 0;
                
                OnSpecialAttackInputDown();
            }
            else
            {
                if (Input.GetKey(AttackButton) || Input.GetKey(GuardButton))
                    specialAttackInputPressedTime += Time.deltaTime;
                else
                    specialAttackInputPressedTime = 0;
                
                if (attackInputDownCalled)
                {
                    OnAttackInputDown();
                }
                else if (attackInputUpCalled)
                {
                    OnAttackInputUp();
                }

                if (guardInputDownCalled)
                {
                    OnDefenseInputDown();
                }
                else if (guardInputUpCalled)
                {
                    OnDefenseInputUp();
                }
            }

            if (rollInputDownCalled)
            {
                OnRollInputDown();
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
        }

        private static void OnSprintInputDown()
        {
            SprintInputDown?.Invoke();
        }

        private static void OnSprintInputUp()
        {
            SprintInputUp?.Invoke();
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

        private static void OnRollInputDown()
        {
            RollInputDown?.Invoke();
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

        private static void OnSpecialAttackInputDown()
        {
            SpecialAttackInputDown?.Invoke();
        }

        #endregion
    }
}