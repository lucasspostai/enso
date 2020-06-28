using System.Collections;
using Enso.CombatSystem;
using Framework;
using UnityEngine;

namespace Enso.Characters.Enemies.AshigaruArcher
{
    public class AshigaruArcherAttackController : EnemyAttackController
    {
        private bool raisingBow;
        private bool mustRotate;
        private Coroutine disableLineCoroutine;

        private const float ArrowMissDistance = 100f;
        private const float ArrowLineDuration = 0.1f;

        [SerializeField] private AttackAnimation RaiseBowAnimation;
        [SerializeField] private AttackAnimation ShootArrowAnimation;
        [SerializeField] private Transform ArrowSpawn;
        [SerializeField] private LayerMask HitLayerMask;

        [Header("Effects")] [SerializeField] private LineRenderer ArrowLineRenderer;
        [SerializeField] private GameObject AimParticle;
        [SerializeField] private GameObject ShootArrowParticlePrefab;

        protected override void Start()
        {
            base.Start();

            PoolManager.Instance.CreatePool(ShootArrowParticlePrefab, 3);
        }

        protected override void Update()
        {
            base.Update();

            if (raisingBow && mustRotate)
                RotateTowardsTarget();
        }

        public void RaiseBow()
        {
            if (ThisFighter.AnimationHandler.IsAnyAnimationDifferentThanAttackPlaying() ||
                ThisFighter.AnimationHandler.IsAnyGuardAnimationPlaying())
                return;

            RotateTowardsTarget();

            raisingBow = true;
            mustRotate = true;

            StartAttack(RaiseBowAnimation);

            CanAttack = false;

            AimParticle.SetActive(true);
        }

        private void ShootArrow()
        {
            PoolManager.Instance.ReuseObject(ShootArrowParticlePrefab, ArrowSpawn.position, ArrowSpawn.rotation);

            StartAttack(ShootArrowAnimation);

            GetCollisions();
        }

        private void GetCollisions()
        {
            var hitInfo = Physics2D.Raycast(ArrowSpawn.position, ArrowSpawn.right, ArrowMissDistance, HitLayerMask);

            ArrowLineRenderer.SetPosition(0, ArrowSpawn.position);

            var hitTarget = false;
            
            if (hitInfo)
            {
                var hurtbox = hitInfo.transform.GetComponent<Hurtbox>();

                if (hurtbox && hurtbox.ThisFighter.FighterTeam != ThisFighter.FighterTeam &&
                    !hurtbox.ThisFighter.GetHealthSystem().IsInvincible)
                {
                    hurtbox.TakeDamage(ShootArrowAnimation.Damage, ArrowSpawn.right);
                    
                    hitTarget = true;
                }
            }

            if (hitTarget)
            {
                ArrowLineRenderer.SetPosition(1, hitInfo.point);
            }
            else
            {
                ArrowLineRenderer.SetPosition(1, ArrowSpawn.position + ArrowSpawn.right * ArrowMissDistance);
            }

            if (disableLineCoroutine != null)
                StopCoroutine(disableLineCoroutine);

            disableLineCoroutine = StartCoroutine(WaitThenDisableLine());
        }

        private IEnumerator WaitThenDisableLine()
        {
            SetLineRendererVisible(true);

            yield return new WaitForSeconds(ArrowLineDuration);

            SetLineRendererVisible(false);
        }

        private void SetLineRendererVisible(bool visible)
        {
            ArrowLineRenderer.enabled = visible;
        }

        public override void OnCanCutAnimation()
        {
            base.OnCanCutAnimation();

            mustRotate = false;
        }

        public override void OnInterrupted()
        {
            base.OnInterrupted();

            mustRotate = false;
            raisingBow = false;
        }

        public override void OnLastFrameEnd()
        {
            base.OnLastFrameEnd();

            if (raisingBow)
            {
                raisingBow = false;
                ShootArrow();
            }
            else
            {
                Wait();

                ThisFighter.AnimationHandler.Play(this,
                    ThisFighter.MovementController.Animations.IdleAnimationClipHolder.AnimatorStateName);
            }
        }
    }
}