using System;
using System.Collections;
using Framework;
using UnityEngine;

namespace Enso.CombatSystem
{
    [RequireComponent(typeof(CharacterAnimationHandler))]
    public class CharacterGhostEffectController : MonoBehaviour
    {
        private bool isActive;
        private Coroutine ghostEffectsCoroutine;
        private CharacterAnimationHandler characterAnimationHandler;
        private CharacterGhostEffect characterGhostEffect;
        private GameObject prefabClone;

        [SerializeField] private float GhostEffectInterval = 0.1f;
        [SerializeField] private float GhostEffectDuration = 0.5f;
        [SerializeField] private GameObject GhostEffectPrefab;
        [SerializeField] private int MaxSimultaneousGhosts;
        [SerializeField] private Color GhostInitialColor = Color.grey;
        [SerializeField] private TrailRenderer Trail;

        private void Start()
        {
            characterAnimationHandler = GetComponent<CharacterAnimationHandler>();

            prefabClone = Instantiate(GhostEffectPrefab);
            prefabClone.SetActive(false);
            
            PoolManager.Instance.CreatePool(prefabClone, MaxSimultaneousGhosts);

            for (int i = 0; i < MaxSimultaneousGhosts; i++)
            {
                characterGhostEffect = PoolManager.Instance
                    .ReuseObject(prefabClone, transform.position, transform.rotation).GameObject
                    .GetComponent<CharacterGhostEffect>();

                characterGhostEffect.SetAnimationHandlerAndColor(
                    characterAnimationHandler, GhostInitialColor, GhostEffectDuration
                );

                characterGhostEffect.Disable();
            }
        }

        private void InstantiateGhostEffect()
        {
            PoolManager.Instance.ReuseObject(prefabClone, transform.position, transform.rotation);
        }

        private IEnumerator InstantiateGhostEffects()
        {
            while (isActive)
            {
                InstantiateGhostEffect();

                yield return new WaitForSeconds(GhostEffectInterval);
            }
        }

        private void StopGhostEffectsCoroutine()
        {
            if (ghostEffectsCoroutine != null)
                StopCoroutine(ghostEffectsCoroutine);
        }

        public void ActivateGhostEffects()
        {
            isActive = true;
            
            Trail.gameObject.SetActive(true);

            StopGhostEffectsCoroutine();

            ghostEffectsCoroutine = StartCoroutine(InstantiateGhostEffects());
        }

        public void DisableGhostEffects()
        {
            isActive = false;
            
            Trail.gameObject.SetActive(false);

            StopGhostEffectsCoroutine();
        }
    }
}