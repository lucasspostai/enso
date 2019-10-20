using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private bool attackInputDownCalled;
    private bool defenseInputDownCalled;
    private bool defenseInputUpCalled;
    private bool dodgeInputDownCalled;

    [SerializeField] private KeyCode AttackButton = KeyCode.Mouse0;
    [SerializeField] private KeyCode DefenseButton = KeyCode.Mouse1;
    [SerializeField] private KeyCode DodgeButton = KeyCode.Space;
    
    public static event Action AttackInputDown;
    public static event Action DefenseInputDown;
    public static event Action DefenseInputUp;
    public static event Action DodgeInputDown;
    
    public static Vector2 Movement;

    private void Update()
    {
        UpdateMovement();

        attackInputDownCalled = Input.GetKeyDown(AttackButton);
        defenseInputDownCalled = Input.GetKeyDown(DefenseButton);
        defenseInputUpCalled = Input.GetKeyUp(DefenseButton);
        dodgeInputDownCalled = Input.GetKeyDown(DodgeButton);

        if (attackInputDownCalled)
        {
            OnAttackInputDown();
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

    private void UpdateMovement()
    {
        Movement.x = Input.GetAxisRaw("Horizontal");
        Movement.y = Input.GetAxisRaw("Vertical");
        Movement.Normalize();
    }

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
}