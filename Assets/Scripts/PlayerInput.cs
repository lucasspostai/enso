using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private bool attackInputDownCalled;

    [SerializeField] private KeyCode AttackButton = KeyCode.Space;
    
    public static event Action AttackInputDown;
    
    public static Vector2 Movement;

    private void Update()
    {
        UpdateMovement();

        attackInputDownCalled = Input.GetKeyDown(AttackButton);

        if (attackInputDownCalled)
        {
            OnAttackInputDown();
        }
    }

    private void UpdateMovement()
    {
        Movement = Vector2.zero;
        
        if(Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0)
            Movement.x = Input.GetAxisRaw("Horizontal") > 0 ? 1 : -1;
        
        if(Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0)
            Movement.y = Input.GetAxisRaw("Vertical") > 0 ? 1 : -1;
    }

    private static void OnAttackInputDown()
    {
        AttackInputDown?.Invoke();
    }
}