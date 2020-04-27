using System;
using UnityEngine;

public class CharacterAnimations : MonoBehaviour
{
    public static readonly int FaceX = Animator.StringToHash("FaceX");
    public static readonly int FaceY = Animator.StringToHash("FaceY");
    public static readonly string IdleState = "OneSword_Idle";
    public static readonly string WalkingState = "Walking";
    public static readonly string RunningState = "OneSword_Run";
    public static readonly string SprintingState = "Sprinting";
}
