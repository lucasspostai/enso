using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float currentSpeed;
    private Vector3 targetVelocity;
    private Vector3 currentVelocity;

    [Header("References")] 
    [SerializeField] private Animator PlayerAnimator;
    [SerializeField] private PlayerAttack Attack;
    [SerializeField] private PlayerDefense Defense;
    [SerializeField] private CharacterCollisions Collisions;
    [SerializeField] private PlayerDodgeRoll DodgeRoll;
    [SerializeField] private PlayerProperties Properties;

    [HideInInspector] public Vector3 Velocity;

    private void Start()
    {
        currentSpeed = Properties.MoveSpeed;
    }

    private void OnEnable()
    {
        PlayerInput.DefenseInputDown += StartDefending;
        PlayerInput.DefenseInputUp += StopDefending;
    }
    
    private void OnDisable()
    {
        PlayerInput.DefenseInputDown -= StartDefending;
        PlayerInput.DefenseInputUp -= StopDefending;
    }

    private void Update()
    {
        if (PlayerInput.Movement != Vector2.zero)
        {
            PlayerAnimator.SetFloat(CharacterAnimations.FaceX, PlayerInput.Movement.x);
            PlayerAnimator.SetFloat(CharacterAnimations.FaceY, PlayerInput.Movement.y);
        }
        
        if (Collisions.Info.Above || Collisions.Info.Below)
            Velocity.y = 0;

        if (Collisions.Info.Left || Collisions.Info.Right)
            Velocity.x = 0;

        if (DodgeRoll.ActualRollState == RollState.Sliding)
            return;
        
        targetVelocity = PlayerInput.Movement * currentSpeed;
        Velocity = Vector3.SmoothDamp(Velocity, targetVelocity, ref currentVelocity, Properties.AccelerationTime);
        Move(Velocity * Time.deltaTime);
    }

    public void Move(Vector2 moveAmount)
    {
        Collisions.UpdateRaycastOrigins();
        Collisions.Info.Reset();

        if (moveAmount.x != 0)
            Collisions.GetHorizontalCollisions(ref moveAmount);

        if (moveAmount.y != 0)
            Collisions.GetVerticalCollisions(ref moveAmount);

        transform.Translate(moveAmount);

        if (Attack.IsPerformingSimpleAttack || Attack.IsPerformingHeavyAttack || DodgeRoll.ActualRollState == RollState.Sliding || Defense.IsDefending)
            return;

        if (PlayerInput.Movement == Vector2.zero)
        {
            PlayerAnimator.Play(CharacterAnimations.IdleState);
            return;
        }

        PlayerAnimator.Play(CharacterAnimations.RunningState);
    }

    private void StartDefending()
    {
        currentSpeed = Properties.MoveSpeedWhileDefending;
    }
    
    private void StopDefending()
    {
        currentSpeed = Properties.MoveSpeed;
    }
}