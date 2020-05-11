using Enso.Characters.Player;
using UnityEngine;

public enum RollState
{
    Normal,
    Sliding
}

public class PlayerDodgeRoll : MonoBehaviour
{
    private float actualSlidingLength;

    [SerializeField] private Animator PlayerAnimator;
    [SerializeField] private PlayerProperties Properties;
    [SerializeField] private PlayerMovement Movement;
    
    [HideInInspector] public RollState ActualRollState;

    private void Start()
    {
        ActualRollState = RollState.Normal;
    }

    private void OnEnable()
    {
        PlayerInput.DodgeInputDown += Roll;
        //CharacterAnimations.EndRollAnimation += StopSliding;
    }

    private void OnDisable()
    {
        PlayerInput.DodgeInputDown -= Roll;
        //CharacterAnimations.EndRollAnimation -= StopSliding;
    }

    private void Update()
    {
        if (ActualRollState == RollState.Sliding)
            HandleSliding();
    }

    private void Roll()
    {
        if (ActualRollState == RollState.Normal)
        {
            ActualRollState = RollState.Sliding;
            //PlayerAnimator.Play(CharacterAnimations.DodgingState);
        }
    }

    private void HandleSliding()
    {
        Movement.Move(PlayerInput.Movement * Properties.SlidingSpeed * Time.deltaTime);
    }
    
    private void StopSliding()
    {
        ActualRollState = RollState.Normal;
    }
}
