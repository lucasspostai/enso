using Enso.Characters.Player;
using Framework.Utils;
using UnityEngine;

namespace Enso.UI
{
    public class DeathScreen : MonoBehaviour
    {
        public void LoadSavedGame()
        {
            LevelLoader.Instance.LoadSavedLevel();
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}
