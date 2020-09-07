using System;
using Framework;
using UnityEngine;
using Rewired;

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
        private bool returnInputDownCalled;
        private bool specialAttackInputDownCalled;
        private bool statusInputDownCalled;
        private bool pauseInputDownCalled;
        private bool anyKeyDownCalled;

        private bool startingGuard;
        
        private Rewired.Player rewiredPlayer;

        private float guardInputHoldingTime;

        [SerializeField] private string MoveHorizontalAction = "Move Horizontal";
        [SerializeField] private string MoveVerticalAction = "Move Vertical";
        [SerializeField] private string SprintAction = "Sprint";
        [SerializeField] private string AttackAction = "Attack";
        [SerializeField] private string GuardAction = "Guard";
        [SerializeField] private string RollAction = "Roll";
        [SerializeField] private string HealAction = "Heal";
        [SerializeField] private string SpecialAttackAction = "Special Attack";
        [SerializeField] private string InteractionAction = "Interaction";
        [SerializeField] private string CancelAction = "Cancel";
        [SerializeField] private string PageLeftAction = "Page Left";
        [SerializeField] private string PageRightAction = "Page Right";
        [SerializeField] private string PauseAction = "Pause";
        
        [SerializeField] private float ParryDeadZone = 0.2f;
        [SerializeField] private int PlayerId = 0;

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
        public static event Action CancelInputDown;
        public static event Action PageLeftInputDown;
        public static event Action PageRightInputDown;
        public static event Action PauseInputDown;
        public static event Action AnyInputDown;

        public static Vector2 Movement;

        private void Start()
        {
            rewiredPlayer = ReInput.players.GetPlayer(PlayerId);
        }

        private void Update()
        {
            pauseInputDownCalled = rewiredPlayer.GetButtonDown(PauseAction);
            pageLeftInputDownCalled = rewiredPlayer.GetButtonDown(PageLeftAction);
            pageRightInputDownCalled = rewiredPlayer.GetButtonDown(PageRightAction);
            returnInputDownCalled = rewiredPlayer.GetButtonDown(CancelAction);

            if (pauseInputDownCalled)
            {
                OnPauseInputDown();
            }
            
            if (pageLeftInputDownCalled)
            {
                OnPageLeftInputDown();
            }

            if (pageRightInputDownCalled)
            {
                OnPageRightInputDown();
            }
            
            if (returnInputDownCalled)
            {
                OnReturnInputDown();
            }
            
            if (GameManager.Instance && GameManager.Instance.GamePaused)
                return;
            
            UpdateMovement();

            sprintInputDownCalled = rewiredPlayer.GetButtonDown(SprintAction);
            sprintInputUpCalled = rewiredPlayer.GetButtonUp(SprintAction);
            attackInputDownCalled = rewiredPlayer.GetButtonDown(AttackAction);
            attackInputUpCalled = rewiredPlayer.GetButtonUp(AttackAction);
            guardInputDownCalled = rewiredPlayer.GetButtonDown(GuardAction);
            guardInputCalled = rewiredPlayer.GetButton(GuardAction);
            guardInputUpCalled = rewiredPlayer.GetButtonUp(GuardAction);
            rollInputDownCalled = rewiredPlayer.GetButtonDown(RollAction);
            healInputDownCalled = rewiredPlayer.GetButtonDown(HealAction);
            specialAttackInputDownCalled = rewiredPlayer.GetButtonDown(SpecialAttackAction);
            interactionInputDownCalled = rewiredPlayer.GetButtonDown(InteractionAction);
            anyKeyDownCalled = rewiredPlayer.GetAnyButton();

            HoldingGuardInput = rewiredPlayer.GetButton(GuardAction);
            HoldingHealInput = rewiredPlayer.GetButton(HealAction);

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

                if (startingGuard && guardInputHoldingTime >= ParryDeadZone)
                {
                    startingGuard = false;
                    OnGuardInputDown();
                }
            }

            if (guardInputDownCalled)
            {
                startingGuard = true;
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

            if (anyKeyDownCalled)
            {
                OnAnyInputDown();
            }
        }

        #region Movement

        private void UpdateMovement()
        {
            Movement.x = rewiredPlayer.GetAxisRaw(MoveHorizontalAction);
            Movement.y = rewiredPlayer.GetAxisRaw(MoveVerticalAction);
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

        #endregion

        #region Menus

        private static void OnInteractionInputDown()
        {
            InteractionInputDown?.Invoke();
        }
        
        private static void OnReturnInputDown()
        {
            CancelInputDown?.Invoke();
        }

        private static void OnPageLeftInputDown()
        {
            PageLeftInputDown?.Invoke();
        }

        private static void OnPageRightInputDown()
        {
            PageRightInputDown?.Invoke();
        }

        private static void OnPauseInputDown()
        {
            PauseInputDown?.Invoke();
        }
        
        #endregion

        private static void OnAnyInputDown()
        {
            AnyInputDown?.Invoke();
        }

        public void Rumble(float intensity, float duration)
        {
            rewiredPlayer?.SetVibration(0, intensity, duration);
        }
    }
}