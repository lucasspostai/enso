using Enso.Characters.Player;
using Framework.Utils;
using UnityEngine;

namespace Enso.UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private Element PausePanel;
        [SerializeField] private Element OptionsPanel;
        
        private void OnEnable()
        {
            PlayerInput.PauseInputDown += ChooseOption;
        }
        
        private void OnDisable()
        {
            PlayerInput.PauseInputDown -= ChooseOption;
        }

        private void ChooseOption()
        {
            if (GameManager.Instance.ShrineActive)
                return;
            
            if (GameManager.Instance.GamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        private void PauseGame()
        {
            PausePanel.gameObject.SetActive(true);
            PausePanel.Enable();
            
            GameManager.Instance.FreezeGame();
        }
        
        public void ResumeGame()
        {
            PausePanel.Disable();
            OptionsPanel.Disable();
            
            GameManager.Instance.NormalizeTime();
        }

        public void ReturnToMenu()
        {
            GameManager.Instance.NormalizeTime();
            
            LevelLoader.Instance.LoadMainMenu();
        }
    }
}
