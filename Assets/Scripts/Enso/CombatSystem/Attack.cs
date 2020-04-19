using System;
using Enso.Interfaces;
using Framework.Animations;
using UnityEngine;

namespace Enso.CombatSystem
{
    public class Attack : MonoBehaviour, IHitboxResponder, IFrameCheckHandler
    {
        [SerializeField] private Hitbox AttackHitbox;
        [SerializeField] private ExtendedAnimationClip AttackAnimationClip;
        [SerializeField] private FrameChecker AttackFrameChecker;

        public int Damage;

        private void Start()
        {
            AttackFrameChecker.Initialize(this, AttackAnimationClip);
        }

        private void Update()
        {
            AttackFrameChecker.CheckFrames();

            if (Input.GetKeyDown(KeyCode.K))
            {
                PerformAttack();
            }
        }

        public void PerformAttack()
        {
            AttackFrameChecker.ResetProperties();
            
            AttackHitbox.SetHitboxResponder(this);
        }
        
        public void CollidedWith(Collider2D otherCollider)
        {
            var hurtbox = otherCollider.GetComponent<Hurtbox>();

            if (hurtbox != null) 
                hurtbox.TakeDamage(Damage);
        }

        public void OnHitFrameStart()
        {
            print("HitFrameStart");
        }

        public void OnHitFrameEnd()
        {
            print("HitFrameEnd");
        }

        public void OnLastFrameStart()
        {
            print("LastFrameStart");
        }

        public void OnLastFrameEnd()
        {
            print("LastFrameStart");
        }
    }
}
