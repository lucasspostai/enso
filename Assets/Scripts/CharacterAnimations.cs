using System;
using UnityEngine;

public class CharacterAnimations : MonoBehaviour
{
    public static readonly int FaceX = Animator.StringToHash("FaceX");
    public static readonly int FaceY = Animator.StringToHash("FaceY");
    public static readonly string IdleState = "Idle";
    public static readonly string RunningState = "Running";
    public static readonly string DodgingState = "Dodge";
    public static readonly string BasicAttackState = "BasicAttack";
    public static readonly string HeavyAttackState = "HeavyAttack";
    public static readonly string DefenseState = "Defense";
    
    public static event Action EnableCollider;
    public static event Action DisableCollider;
    public static event Action EndAttackAnimation;
    public static event Action EndRollAnimation;
    public static event Action EndHeavyAttackAnimation;

    public void OnEnableCollider()
    {
        EnableCollider?.Invoke();
    }

    public void OnDisableCollider()
    {
        DisableCollider?.Invoke();
    }

    public void OnEndAttackAnimation()
    {
        EndAttackAnimation?.Invoke();
    }

    public void OnEndRollAnimation()
    {
        EndRollAnimation?.Invoke();
    }

    public void OnEndHeavyAttackAnimation()
    {
	    EndHeavyAttackAnimation?.Invoke();
    }
}
