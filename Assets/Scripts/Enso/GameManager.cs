using System.Collections;
using Framework;
using UnityEngine;

namespace Enso
{
    public class GameManager : Singleton<GameManager>
    {
        private Coroutine changeTimeScaleCoroutine;

        [HideInInspector] public bool GamePaused;
        [HideInInspector] public bool ShrineActive;
        [HideInInspector] public bool LeavingLocation;

        public void FreezeGame()
        {
            Time.timeScale = 0;
            GamePaused = true;
        }

        public void NormalizeTime()
        {
            Time.timeScale = 1;
            GamePaused = false;
        }

        public void ChangeTimeScale(float timeScale, float time)
        {
            Time.timeScale = timeScale;

            if (changeTimeScaleCoroutine != null)
                StopCoroutine(changeTimeScaleCoroutine);

            changeTimeScaleCoroutine = StartCoroutine(ChangeTimeScaleThenReturn(time));
        }
        
        private IEnumerator ChangeTimeScaleThenReturn(float time)
        {
            yield return new WaitForSeconds(time * Time.timeScale);

            Time.timeScale = 1f;
        }
    }
}
