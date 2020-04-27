using System;
using Enso.Characters.Player;
using UnityEngine;

public class PlayerDefense : MonoBehaviour
{
    [HideInInspector] public bool IsDefending;

    [SerializeField] private Animator PlayerAnimator;
    [SerializeField] private PlayerAttackController AttackController;
    
    private void OnEnable()
    {
        PlayerInput.DefenseInputDown += StartDefense;
        PlayerInput.DefenseInputUp += StopDefense;
    }

    private void OnDisable()
    {
        PlayerInput.DefenseInputDown -= StartDefense;
        PlayerInput.DefenseInputUp -= StopDefense;
    }

    private void StartDefense()
    {
        //if (Attack.IsPerformingSimpleAttack)
      //      return;
        
        Debug.Log("StartDefense");
        IsDefending = true;
        
        //PlayerAnimator.Play(CharacterAnimations.DefenseState);
    }

    private void StopDefense()
    {
        Debug.Log("StopDefense");
        IsDefending = false;
    }
}
