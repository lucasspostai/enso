using System;
using System.Collections;
using Framework;
using UnityEngine;

namespace Enso.CombatSystem
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class CharacterGhostEffect : PoolObject
    {
        private Color ghostColor = Color.grey;
        private Coroutine alphaCoroutine;
        private CharacterAnimationHandler characterAnimationHandler;
        private float ghostDuration = 0.2f;
        private float timeRemaining;
        private SpriteRenderer spriteRenderer;
        
        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetAnimationHandlerAndColor(CharacterAnimationHandler animationHandler, Color color, float duration)
        {
            characterAnimationHandler = animationHandler;
            ghostColor = color;
            ghostDuration = duration;

            SetProperties();
        }

        public override void OnObjectReuse()
        {
            base.OnObjectReuse();

            SetProperties();
        }

        private void SetProperties()
        {
            if (characterAnimationHandler == null)
                return;

            transform.position = characterAnimationHandler.transform.position;
            transform.localScale = characterAnimationHandler.CharacterAnimator.transform.localScale;

            spriteRenderer.sprite = characterAnimationHandler.CharacterSpriteRenderer.sprite;
            spriteRenderer.color = ghostColor;

            timeRemaining = ghostDuration;

            if(alphaCoroutine != null)
                StopCoroutine(alphaCoroutine);
            
            alphaCoroutine = StartCoroutine(FadeAlpha(0, ghostDuration));
        }

        private void Update()
        {
            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 0)
                Disable();
        }

        private IEnumerator FadeAlpha(float finalValue, float duration)
        {
            var alpha = spriteRenderer.material.color.a;
            var color = spriteRenderer.color;

            for (float time = 0.0f; time < 1.0f; time += Time.deltaTime / duration)
            {
                var newColor = new Color(color.r, color.g, color.b, Mathf.Lerp(alpha, finalValue, time));
                spriteRenderer.color = newColor;
                yield return null;
            }
        }
    }
}