using Enso.Characters.Player;
using Framework;
using UnityEngine;

namespace Enso.UI
{
    public class PlayerCanvas : MonoBehaviour
    {
        private Player player;

        [SerializeField] private Animator HudAnimator;
        [SerializeField] private GameObject DeathScreen;
        
        private static readonly int DisableHash = Animator.StringToHash("Disable");
        private static readonly int EnableHash = Animator.StringToHash("Enable");

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
        
        public void Hide()
        {
            HudAnimator.SetTrigger(DisableHash);
        }
        
        public void Show()
        {
            HudAnimator.SetTrigger(EnableHash);
        }

        private void EnableDeathScreen()
        {
            DeathScreen.SetActive(true);
        }
    }
}
