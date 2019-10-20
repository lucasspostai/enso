using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 targetVelocity;
    private Vector3 currentVelocity;

    [Header("References")] 
    [SerializeField] private Animator PlayerAnimator;
    [SerializeField] private PlayerAttack Attack;
    [SerializeField] private PlayerCollisions Collisions;
    [SerializeField] private PlayerDodgeRoll DodgeRoll;
    [SerializeField] private PlayerProperties Properties;

    [HideInInspector] public Vector3 Velocity;

    private void Update()
    {
        if (PlayerInput.Movement != Vector2.zero)
        {
            PlayerAnimator.SetFloat(PlayerAnimations.FaceX, PlayerInput.Movement.x);
            PlayerAnimator.SetFloat(PlayerAnimations.FaceY, PlayerInput.Movement.y);
        }
        
        if (Collisions.Info.Above || Collisions.Info.Below)
            Velocity.y = 0;

        if (Collisions.Info.Left || Collisions.Info.Right)
            Velocity.x = 0;

        if (DodgeRoll.ActualRollState == RollState.Sliding)
            return;
        
        targetVelocity = PlayerInput.Movement * Properties.MoveSpeed;
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

        if (Attack.IsAttacking || DodgeRoll.ActualRollState == RollState.Sliding)
            return;

        if (PlayerInput.Movement == Vector2.zero)
        {
            PlayerAnimator.Play(PlayerAnimations.IdleState);
            return;
        }

        PlayerAnimator.Play(PlayerAnimations.RunningState);
    }
}