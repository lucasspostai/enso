using System;
using Enso.Characters.Player;
using Framework.Utils;
using UnityEngine;

namespace Enso.UI.Menu
{
    public class PreMenu : MonoBehaviour
    {
        private void OnEnable()
        {
            PlayerInput.AnyInputDown += LoadMainMenu;
        }

        private void OnDisable()
        {
            PlayerInput.AnyInputDown -= LoadMainMenu;
        }

        private void LoadMainMenu()
        {
            LevelLoader.Instance.LoadMainMenu();
        }
    }
}
