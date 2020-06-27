using Enso.Characters.Player;
using Framework;
using UnityEngine;

namespace Enso.UI
{
    public class PlayerCanvas : Singleton<PlayerCanvas>
    {
        private Player player;

        [SerializeField] private GameObject DeathScreen;
        
        private void OnEnable()
        {
            player = FindObjectOfType<Player>();

            if (player == null)
                return;

            player.GetHealthSystem().Death += EnableDeathScreen;
        }

        private void OnDisable()
        {
            if (player == null)
                return;

            player.GetHealthSystem().Death -= EnableDeathScreen;
        }

        private void EnableDeathScreen()
        {
            DeathScreen.SetActive(true);
        }
    }
}
