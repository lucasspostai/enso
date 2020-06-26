using System.Collections;
using Framework.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Enso.UI.Menu
{
    public class MainMenu : Element
    {
        [SerializeField] private Canvas ThisCanvas;
        
        public void StartGame()
        {
            StartCoroutine(WaitAndUnloadScene());
        }

        private IEnumerator WaitAndUnloadScene()
        {
            Disable();
            
            yield return new WaitForSeconds(0.5f);
            
            GameManager.Instance.GamePaused = false;
            
            SceneManager.UnloadSceneAsync(LevelLoader.MainMenuScene);
        }
    }
}
