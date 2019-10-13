using System;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    public static readonly int FaceX = Animator.StringToHash("FaceX");
    public static readonly int FaceY = Animator.StringToHash("FaceY");
    public static readonly string IdleState = "Idle";
    public static readonly string RunningState = "Running";
    public static readonly string BasicAttackState = "BasicAttack";
    
    public static event Action EnableCollider;
    public static event Action DisableCollider;
    public static event Action EndAttackAnimation;

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
}
