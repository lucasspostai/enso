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
            if (GameManager.Instance.ShrineActive || GameManager.Instance.MenuIsOpen)
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
            OptionsPanel.gameObject.SetActive(false);
            PausePanel.gameObject.SetActive(true);
            
            if(!PausePanel.IsEnabled)
                PausePanel.Enable();

            GameManager.Instance.FreezeGame();
        }
        
        public void ResumeGame()
        {
            if (OptionsPanel.IsEnabled)
            {
                OptionsPanel.Disable();
                PauseGame();
                return;
            }

            if (PausePanel.IsEnabled)
            {
                PausePanel.Disable();
                OptionsPanel.gameObject.SetActive(false);
            }

            GameManager.Instance.NormalizeTime();
        }

        public void ReturnToMenu()
        {
            GameManager.Instance.NormalizeTime();
            
            LevelLoader.Instance.LoadMainMenu();
        }
    }
}
