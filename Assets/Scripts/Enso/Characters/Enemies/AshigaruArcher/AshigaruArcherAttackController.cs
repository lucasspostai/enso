using System.Collections;
using Enso.CombatSystem;
using Framework;
using UnityEngine;

namespace Enso.Characters.Enemies.AshigaruArcher
{
    public class AshigaruArcherAttackController : EnemyAttackController
    {
        private bool hitTarget;
        private bool raisingBow;
        private bool mustRotate;
        private Coroutine disableLineCoroutine;
        private Vector2 shootDirection;

        private const float ArrowMissDistance = 50f;
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
            {
                RotateTowardsTarget();
                SetShootDirection();
            }
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

        private void SetShootDirection()
        {
            if (ThisFighter.Target)
                shootDirection = ThisFighter.Target.position - transform.position;
        }

        private void GetCollisions()
        {
            // ReSharper disable once Unity.PreferNonAllocApi
            var raycastHits = Physics2D.RaycastAll(transform.position, shootDirection,
                ArrowMissDistance, HitLayerMask);

            ArrowLineRenderer.SetPosition(0, ArrowSpawn.position);

            hitTarget = false;

            foreach (var hit in raycastHits)
            {
                var hurtbox = hit.transform.GetComponent<Hurtbox>();

                if (hurtbox && hurtbox.ThisFighter.FighterTeam != ThisFighter.FighterTeam &&
                    !hurtbox.ThisFighter.GetHealthSystem().IsInvincible)
                {
                    hurtbox.TakeDamage(ShootArrowAnimation.Damage, ArrowSpawn.right);
                    ArrowLineRenderer.SetPosition(1, hit.point);

                    hitTarget = true;

                    break;
                }
            }

            if (!hitTarget)
            {
                hitTarget = false;
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