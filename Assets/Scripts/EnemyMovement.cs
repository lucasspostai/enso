using System.Collections;
using System.Collections.Generic;
using Enso.Characters.Enemies;
using Enso.Characters.Player;
using Framework;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyMovement : MonoBehaviour
{
    private float currentSpeed;
    private Vector3 targetVelocity;
    private Vector3 currentVelocity;

    [Header("References")] 
    [SerializeField] private Animator EnemyAnimator;
    //[SerializeField] private PlayerAttack Attack;
    [SerializeField] private CharacterCollisions Collisions;
    //[SerializeField] private PlayerDodgeRoll DodgeRoll;
    [SerializeField] private EnemyProperties Properties;
    [SerializeField] private Transform Target;

    [HideInInspector] public Vector3 Velocity;

    private void Start()
    {
        //currentSpeed = Properties.MoveSpeed;
    }

    /*private void OnEnable()
    {
        PlayerInput.DefenseInputDown += StartDefending;
        PlayerInput.DefenseInputUp += StopDefending;
    }
    
    private void OnDisable()
    {
        PlayerInput.DefenseInputDown -= StartDefending;
        PlayerInput.DefenseInputUp -= StopDefending;
    }*/

    private void Update()
    {
        if (PlayerInput.Movement != Vector2.zero)
        {
            //EnemyAnimator.SetFloat(CharacterAnimations.FaceX, PlayerInput.Movement.x);
            //EnemyAnimator.SetFloat(CharacterAnimations.FaceY, PlayerInput.Movement.y);
        }
        
        if (Collisions.Info.Above || Collisions.Info.Below)
            Velocity.y = 0;

        if (Collisions.Info.Left || Collisions.Info.Right)
            Velocity.x = 0;

        /*if (DodgeRoll.ActualRollState == RollState.Sliding)
            return;*/
        
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

        /*if (Attack.IsPerformingSimpleAttack || DodgeRoll.ActualRollState == RollState.Sliding)
            return;*/

        if (PlayerInput.Movement == Vector2.zero)
        {
            //EnemyAnimator.Play(CharacterAnimations.IdleState);
            return;
        }

        //EnemyAnimator.Play(CharacterAnimations.RunningState);
    }

    /*private void StartDefending()
    {
        currentSpeed = Properties.MoveSpeedWhileDefending;
    }
    
    private void StopDefending()
    {
        currentSpeed = Properties.MoveSpeed;
    }*/
}
