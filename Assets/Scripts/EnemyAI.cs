using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private bool isAttacking;
    private bool isDefending;
    private float currentSpeed;
    private PlayerAttack playerAttack;
    private Transform player;
    private Vector2 movement;
    private Vector3 currentVelocity;
    private Vector3 targetVelocity;

    [SerializeField] private Animator EnemyAnimator;
    [SerializeField] private CharacterCollisions Collisions;
    [SerializeField] private EnemyProperties Properties;
    
    [HideInInspector] public Vector3 Velocity;
    
    private void Start()
    {
        currentSpeed = Properties.MoveSpeed;
        player = FindObjectOfType<Player>().transform;
        playerAttack = FindObjectOfType<PlayerAttack>();
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
        Defend();
        Attack();
        Movement();
    }

    private void Defend()
    {
        if (playerAttack.IsPerformingSimpleAttack)
        {
            StartDefending();
            isDefending = true;
            EnemyAnimator.Play(CharacterAnimations.DefenseState);
        }
        else
        {
            isDefending = false;
            StopDefending();
        }
    }

    private void Attack()
    {
        if (isDefending)
            return;
        
        if (Vector2.Distance(transform.position, player.position) < 4f)
        {
            isAttacking = true;
            
            EnemyAnimator.Play(CharacterAnimations.BasicAttackState);
        }
        else
        {
            isAttacking = false;
        }
    }

    private void Movement()
    {
        if (isAttacking || isDefending)
            return;
        
        movement = transform.position - player.position;

        var distance = movement.magnitude;
        var direction = movement / distance;
        direction.Normalize();
        
        Debug.Log(direction);

        if (direction.x > 0.1f)
            direction.x = 1;
        else if (direction.x < -0.1f)
            direction.x = -1;
        else
            direction.x = 0;
        
        if (direction.y > 0.1f)
            direction.y = 1;
        else if (direction.y < -0.1f)
            direction.y = -1;
        else
            direction.y = 0;

        EnemyAnimator.SetFloat(CharacterAnimations.FaceX, - direction.x);
        EnemyAnimator.SetFloat(CharacterAnimations.FaceY, - direction.y);

        if (Collisions.Info.Above || Collisions.Info.Below) 
            Velocity.y = 0;

        if (Collisions.Info.Left || Collisions.Info.Right)
            Velocity.x = 0;

        targetVelocity = direction * currentSpeed;
        Velocity = Vector3.SmoothDamp(Velocity, targetVelocity, ref currentVelocity, Properties.AccelerationTime);
        Move(Velocity * Time.deltaTime);
    }

    private void Move(Vector2 moveAmount)
    {
        Collisions.UpdateRaycastOrigins();
        Collisions.Info.Reset();

        if (moveAmount.x != 0)
            Collisions.GetHorizontalCollisions(ref moveAmount);

        if (moveAmount.y != 0)
            Collisions.GetVerticalCollisions(ref moveAmount);

        transform.Translate(moveAmount);

        /*if (Attack.IsPerformingSimpleAttack || Attack.IsPerformingHeavyAttack || DodgeRoll.ActualRollState == RollState.Sliding || Defense.IsDefending)
            return;*/

        /*if (PlayerInput.Movement == Vector2.zero)
        {
            EnemyAnimator.Play(CharacterAnimations.IdleState);
            return;
        }*/

        EnemyAnimator.Play(CharacterAnimations.RunningState);
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
