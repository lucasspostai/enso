using System.Collections;
using Framework;
using UnityEngine;

namespace Enso
{
    public class GameManager : Singleton<GameManager>
    {
        private Coroutine changeTimeScaleCoroutine;
        
        public void FreezeGame()
        {
            Time.timeScale = 0;
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
