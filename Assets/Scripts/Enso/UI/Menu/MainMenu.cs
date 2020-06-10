using Framework.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Enso.UI.Menu
{
    public class MainMenu : MonoBehaviour
    {
        public string GameSceneName;

        public void StartGame()
        {
            LevelLoader.Instance.LoadLevel(GameSceneName);
        }
    }
}
