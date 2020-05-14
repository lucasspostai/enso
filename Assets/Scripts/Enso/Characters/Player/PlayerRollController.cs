using Enso.CombatSystem;
using UnityEngine;

namespace Enso.Characters.Player
{
    public class PlayerRollController : RollController
    {
        #region Delegates

        private void OnEnable()
        {
            PlayerInput.RollInputDown += PlayRollAnimation;
        }

        private void OnDisable()
        {
            PlayerInput.RollInputDown -= PlayRollAnimation;
        }

        #endregion
    }
}
