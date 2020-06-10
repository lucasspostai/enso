using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Framework.Utils
{
    public class LevelLoader : Singleton<LevelLoader>
    {
        public Animator TransitionAnimator;
        
        public void LoadLevel(string sceneName)
        {
            StartCoroutine(LoadSceneAsynchronously(sceneName));
        }
        
        public void ReloadLevel()
        {
            StartCoroutine(LoadSceneAsynchronously(SceneManager.GetActiveScene().name));
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

        public void QuitGame()
        {
            StartCoroutine(CloseGame());
        }

        private IEnumerator CloseGame()
        {
            TransitionAnimator.Play("LoadScreen_StartTransition");
            
            yield return new WaitForSeconds(1);
            
            Application.Quit();
        }
    }
}
