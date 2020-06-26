using System.Collections;
using Enso;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Framework.Utils
{
    public class LevelLoader : Singleton<LevelLoader>
    {
        public const string MainMenuScene = "MainMenu";
        public const string CreditsScene = "Credits";
        
        public Animator TransitionAnimator;

        [SerializeField] private Level[] Levels;
        
        [HideInInspector] public int CurrentLevelIndex;

        public void LoadMainMenu()
        {
            LoadSavedLevel();
            StartCoroutine(LoadSceneAsynchronously(MainMenuScene, LoadSceneMode.Additive));
            GameManager.Instance.GamePaused = true;
        }
        
        public void LoadCredits()
        {
            StartCoroutine(LoadSceneAsynchronously(CreditsScene, LoadSceneMode.Single));
        }
        
        public void LoadLevel(Level level)
        {
            StartCoroutine(LoadSceneAsynchronously(level.EnvironmentSceneName, LoadSceneMode.Single));
            StartCoroutine(LoadSceneAsynchronously(level.GameplaySceneName, LoadSceneMode.Additive));
        }
        
        public void LoadSavedLevel()
        {
            var playerData = SaveSystem.Load();

            foreach (var level in Levels)
            {
                if (playerData != null && level.LevelIndex == playerData.LevelIndex)
                {
                    Instance.CurrentLevelIndex = playerData.LevelIndex;
                    
                    LoadLevel(level);

                    return;
                }
            }
            
            LoadLevel(Levels[0]);
        }

        private IEnumerator LoadSceneAsynchronously(string sceneName, LoadSceneMode mode)
        {
            print(TransitionAnimator.name);
            
            if(mode == LoadSceneMode.Single)
                TransitionAnimator.Play("LoadScreen_StartTransition");
            
            yield return new WaitForSeconds(1);

            var asyncOperation = SceneManager.LoadSceneAsync(sceneName, mode);

            while (!asyncOperation.isDone)
            {
                yield return null;
            }
            
            if(mode == LoadSceneMode.Single)
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
