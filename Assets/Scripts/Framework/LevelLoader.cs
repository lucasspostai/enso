using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Framework
{
    public class LevelLoader : Singleton<LevelLoader>
    {
        public Animator TransitionAnimator;
        
        public void LoadLevel(Object scene)
        {
            StartCoroutine(LoadSceneAsynchronously(scene.name));
        }

        private IEnumerator LoadSceneAsynchronously(string sceneName)
        {
            TransitionAnimator.Play("LoadScreen_StartTransition");
            
            yield return new WaitForSeconds(1);

            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

            while (!asyncOperation.isDone)
            {
                yield return null;
            }
            
            TransitionAnimator.Play("LoadScreen_EndTransition");
        }
    }
}
