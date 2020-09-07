using Enso.Characters.Player;
using Framework.Utils;
using UnityEngine;

namespace Enso.UI.Menu
{
    public class ExtrasMenu : Element
    {
        [SerializeField] private GameObject ReturnObjectToActivate;
        
        private void OnEnable()
        {
            PlayerInput.CancelInputDown += Return;
        }

        private void OnDisable()
        {
            PlayerInput.CancelInputDown -= Return;
        }
        
        public void LoadCredits()
        {
            MusicManager.Instance.StopAllMusics();
            LevelLoader.Instance.LoadCredits();
        }

        public void DeleteSave()
        {
            SaveSystem.DeleteSave();
            LevelLoader.Instance.LoadMainMenu();
        }
        
        private void Return()
        {
            if (!IsEnabled)
                return;

            if(ReturnObjectToActivate)
                ReturnObjectToActivate.SetActive(true);

            Disable();
        }
    }
}
