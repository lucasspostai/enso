﻿using System.Collections;
using UnityEngine;

namespace Enso.Characters.Enemies.Naosuke
{
    [RequireComponent(typeof(Naosuke))]
    public class NaosukeGuardController : EnemyGuardController
    {
        private bool canParry;
        private Coroutine parryStanceCoroutine;
        private Naosuke naosuke;
        
        [SerializeField] private float MaxTimeOnParryStance = 5f;

        protected override void Start()
        {
            base.Start();

            naosuke = GetComponent<Naosuke>();
        }

        protected override void PlayMovementAnimation()
        {
            base.PlayMovementAnimation();

            if (naosuke.MovementController.Velocity == Vector3.zero)
            {
                PlayGuardAnimation(Animations.GuardIdleAnimationClipHolder);
            }
            else
            {
                PlayGuardAnimation(Animations.ForwardGuardWalkAnimationClipHolder, true);
            }
        }

        public override void Parry()
        {
            base.Parry();

            IsParrying = false;

            canParry = true;
            
            ThisFighter.MovementController.SetSpeed(0);
            
            if(parryStanceCoroutine != null)
                StopCoroutine(parryStanceCoroutine);

            parryStanceCoroutine = StartCoroutine(StayOnParryStance());
        }

        private IEnumerator StayOnParryStance()
        {
            yield return new WaitForSeconds(MaxTimeOnParryStance);
            
            if(IsParrying)
                base.OnLastFrameEnd();
            
            ThisFighter.MovementController.SetSpeed(ThisFighter.GetBaseProperties().RunSpeed);
        }

        public override void OnCanCutAnimation()
        {
            base.OnCanCutAnimation();

            if (canParry)
            {
                IsParrying = true;
                canParry = false;
            }
        }

        public override void OnLastFrameEnd()
        {
            if (!IsParrying)
                base.OnLastFrameEnd();
        }
        
        protected override void ResetAllProperties()
        {
            base.ResetAllProperties();

            IsParrying = false;
            canParry = false;

            if (naosuke)
                naosuke.MovementController.SetSpeed(naosuke.GetBaseProperties().RunSpeed);
        }

        public void StopGuarding()
        {
            ResetAllProperties();
        }
    }
}
