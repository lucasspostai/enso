using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private bool attackInputDownCalled;
    private bool dodgeInputDownCalled;

    [SerializeField] private KeyCode AttackButton = KeyCode.Space;
    [SerializeField] private KeyCode DodgeButton = KeyCode.LeftShift;
    
    public static event Action AttackInputDown;
    public static event Action DodgeInputDown;
    
    public static Vector2 Movement;

    private void Update()
    {
        UpdateMovement();

        attackInputDownCalled = Input.GetKeyDown(AttackButton);
        dodgeInputDownCalled = Input.GetKeyDown(DodgeButton);

        if (attackInputDownCalled)
        {
            OnAttackInputDown();
        }
        
        if (dodgeInputDownCalled)
        {
            OnDodgeInputDown();
        }
    }

    private void UpdateMovement()
    {
        Movement = Vector2.zero;
        
        if(Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0)
            Movement.x = Input.GetAxisRaw("Horizontal") > 0 ? 1 : -1;
        
        if(Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0)
            Movement.y = Input.GetAxisRaw("Vertical") > 0 ? 1 : -1;
        
        Movement.Normalize();
    }

    private static void OnAttackInputDown()
    {
        AttackInputDown?.Invoke();
    }

    private static void OnDodgeInputDown()
    {
        DodgeInputDown?.Invoke();
    }
}