using System;
using Framework;
using UnityEngine;

namespace Enso.Characters.Player
{
    public class PlayerInput : Singleton<PlayerInput>
    {
        public enum MovementState
        {
            Idle,
            Walking,
            Running,
            Sprinting
        }

        private bool pageLeftInputDownCalled;
        private bool pageRightInputDownCalled;
        private bool sprintInputDownCalled;
        private bool sprintInputUpCalled;
        private bool attackInputDownCalled;
        private bool attackInputUpCalled;
        private bool guardInputCalled;
        private bool guardInputDownCalled;
        private bool guardInputUpCalled;
        private bool rollInputDownCalled;
        private bool healInputDownCalled;
        private bool interactionInputDownCalled;
        private bool specialAttackInputDownCalled;

        private float guardInputHoldingTime;

        [SerializeField] private KeyCode SprintButton = KeyCode.Joystick1Button1;
        [SerializeField] private KeyCode AttackButton = KeyCode.Joystick1Button0;
        [SerializeField] private KeyCode GuardButton = KeyCode.Joystick1Button3;
        [SerializeField] private KeyCode RollButton = KeyCode.Joystick1Button2;
        [SerializeField] private KeyCode HealButton = KeyCode.Joystick1Button4;
        [SerializeField] private KeyCode SpecialAttackButton = KeyCode.Joystick1Button5;
        [SerializeField] private KeyCode InteractionButton = KeyCode.Joystick1Button1;
        [SerializeField] private KeyCode PageLeftButton = KeyCode.Joystick1Button4;
        [SerializeField] private KeyCode PageRightButton = KeyCode.Joystick1Button5;
        [SerializeField] private float ParryDeadZone = 0.1f;

        public static bool HoldingGuardInput;
        public static bool HoldingHealInput;

        public static event Action SprintInputDown;
        public static event Action SprintInputUp;
        public static event Action AttackInputDown;
        public static event Action AttackInputUp;
        public static event Action GuardInputDown;
        public static event Action GuardInputUp;
        public static event Action Parry;
        public static event Action RollInputDown;
        public static event Action HealInputDown;
        public static event Action SpecialAttackInputDown;
        public static event Action InteractionInputDown;
        public static event Action PageLeftInputDown;
        public static event Action PageRightInputDown;

        public static Vector2 Movement;

        private void Update()
        {
            UpdateMovement();

            sprintInputDownCalled = Input.GetKeyDown(SprintButton) || Input.GetKeyDown(KeyCode.LeftShift);
            sprintInputUpCalled = Input.GetKeyUp(SprintButton) || Input.GetKeyUp(KeyCode.LeftShift);
            attackInputDownCalled = Input.GetKeyDown(AttackButton) || Input.GetKeyDown(KeyCode.Mouse0);
            attackInputUpCalled = Input.GetKeyUp(AttackButton) || Input.GetKeyUp(KeyCode.Mouse0);
            guardInputDownCalled = Input.GetKeyDown(GuardButton) || Input.GetKeyDown(KeyCode.Mouse1);
            guardInputCalled = Input.GetKey(GuardButton) || Input.GetKey(KeyCode.Mouse1);
            guardInputUpCalled = Input.GetKeyUp(GuardButton) || Input.GetKeyUp(KeyCode.Mouse1);
            rollInputDownCalled = Input.GetKeyDown(RollButton) || Input.GetKeyDown(KeyCode.Space);
            healInputDownCalled = Input.GetKeyDown(HealButton) || Input.GetKeyDown(KeyCode.Q);
            specialAttackInputDownCalled = Input.GetKeyDown(SpecialAttackButton) || Input.GetKey(KeyCode.R);
            interactionInputDownCalled = Input.GetKeyDown(InteractionButton) || Input.GetKey(KeyCode.E);

            pageLeftInputDownCalled = Input.GetKeyDown(PageLeftButton) || Input.GetKeyDown(KeyCode.Q);
            pageRightInputDownCalled = Input.GetKeyDown(PageRightButton) || Input.GetKeyDown(KeyCode.E);

            HoldingGuardInput = Input.GetKey(GuardButton);
            HoldingHealInput = Input.GetKey(HealButton);

            if (pageLeftInputDownCalled)
            {
                OnPageLeftInputDown();
            }

            if (pageRightInputDownCalled)
            {
                OnPageRightInputDown();
            }

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
                OnSpecialAttackInputDown();
            }

            if (attackInputDownCalled)
            {
                OnAttackInputDown();
            }
            else if (attackInputUpCalled)
            {
                OnAttackInputUp();
            }

            if (guardInputCalled)
            {
                guardInputHoldingTime += Time.deltaTime;
            }

            if (guardInputDownCalled)
            {
                OnGuardInputDown();
            }
            else if (guardInputUpCalled)
            {
                if (guardInputHoldingTime < ParryDeadZone)
                {
                    OnParry();
                }
                else
                {
                    OnGuardInputUp();
                }

                guardInputHoldingTime = 0;
            }

            if (rollInputDownCalled)
            {
                OnRollInputDown();
            }

            if (healInputDownCalled)
            {
                OnHealInputDown();
            }

            if (interactionInputDownCalled)
            {
                OnInteractionInputDown();
            }
        }

        #region Movement

        private void UpdateMovement()
        {
            Movement.x = Input.GetAxisRaw("Horizontal");
            Movement.y = Input.GetAxisRaw("Vertical");

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
        
        private static void OnAttackInputUp()
        {
            AttackInputUp?.Invoke();
        }

        private static void OnGuardInputDown()
        {
            GuardInputDown?.Invoke();
        }
        
        private static void OnGuardInputUp()
        {
            GuardInputUp?.Invoke();
        }
        
        private static void OnParry()
        {
            Parry?.Invoke();
        }

        private static void OnRollInputDown()
        {
            RollInputDown?.Invoke();
        }

        private static void OnHealInputDown()
        {
            HealInputDown?.Invoke();
        }

        private static void OnSpecialAttackInputDown()
        {
            SpecialAttackInputDown?.Invoke();
        }

        private static void OnInteractionInputDown()
        {
            InteractionInputDown?.Invoke();
        }

        private static void OnPageLeftInputDown()
        {
            PageLeftInputDown?.Invoke();
        }

        private static void OnPageRightInputDown()
        {
            PageRightInputDown?.Invoke();
        }

        #endregion
    }
}