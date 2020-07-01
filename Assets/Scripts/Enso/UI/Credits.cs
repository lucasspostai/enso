using Enso.Enums;
using Framework.Utils;
using UnityEngine;

namespace Enso.UI
{
    public class Credits : MonoBehaviour
    {
        public void SetSceneHasStarted()
        {
            GameManager.Instance.SceneHasStarted = true;
        }
        
        public void StopAllMusics()
        {
            MusicManager.Instance.StopAllMusics();
        }
        
        public void PlayMusic()
        {
            MusicManager.Instance.SetState(GameState.Adventure, 0, true);
        }

        public void LoadMenu()
        {
            LevelLoader.Instance.LoadMainMenu();
        }
    }
}
