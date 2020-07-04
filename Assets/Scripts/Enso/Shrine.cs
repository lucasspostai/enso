using System.Collections;
using Cinemachine;
using Enso.Characters.Player;
using Enso.UI;
using Framework;
using Framework.Audio;
using Framework.LevelDesignEvents;
using Framework.Utils;
using UnityEngine;

namespace Enso
{
    public class Shrine : LevelDesignEvent
    {
        private bool isActive;
        private bool interactionAvailable = true;
        private Player player;
        private PlayerCanvas playerCanvas;

        [SerializeField] private Element InteractionElement;
        [SerializeField] private Element ShopCanvasElement;
        [SerializeField] private GameObject SaveGameCanvas;
        [SerializeField] private GameObject SaveParticle;
        [SerializeField] private SoundCue SaveSoundCue;
        [SerializeField] private Level ThisLevel;

        public CinemachineCameraManager CameraManager;
        public CinemachineVirtualCamera VirtualCamera;
        public Transform SaveLocation;
        public Transform PlayerArrivalLocation;
        
        [HideInInspector] public bool IsInteracting;
        [HideInInspector] public bool PlayerStartedHere;

        private void OnEnable()
        {
            PlayerInput.InteractionInputDown += Interact;
            PlayerInput.CancelInputDown += Return;
        }
        
        private void OnDisable()
        {
            PlayerInput.InteractionInputDown -= Interact;
            PlayerInput.CancelInputDown -= Return;
        }

        private void Start()
        {
            player = FindObjectOfType<Player>();
            playerCanvas = FindObjectOfType<PlayerCanvas>();
            
            PoolManager.Instance.CreatePool(SaveParticle, 1);
        }

        public override void Execute()
        {
            base.Execute();

            isActive = true;
            InteractionElement.Enable();
            
            interactionAvailable = true;
        }

        public override void Exit()
        {
            base.Exit();

            isActive = false;
            InteractionElement.Disable();
            
            interactionAvailable = false;
        }

        private void Interact()
        {
            if (!isActive || IsInteracting || !interactionAvailable)
                return;

            IsInteracting = true;

            if(!player.MeditationController.IsMeditating)
                player.MeditationController.StartMeditation(this);

            player.SaveGame();

            ShopCanvasElement.gameObject.SetActive(true);
            ShopCanvasElement.Enable();
            
            InteractionElement.Disable();
            SetCanvasActive(false);
            
            GameManager.Instance.ShrineActive = true;
        }
        
        public void Return()
        {
            if (!isActive || !IsInteracting)
                return;

            IsInteracting = false;

            player.MeditationController.EndMeditation();
            
            player.GetHealthSystem().Heal(player.GetHealthSystem().GetMaxHealth());
            player.HealController.RechargeHealing();

            LevelLoader.Instance.CurrentLevelIndex = ThisLevel.LevelIndex;
            player.SaveGame();

            PlaySaveFeedback();
            
            ShopCanvasElement.Disable();
            
            InteractionElement.Enable();
            SetCanvasActive(true);
            
            GameManager.Instance.ShrineActive = false;

            StartCoroutine(WaitThenEnableInteraction());
        }

        private IEnumerator WaitThenEnableInteraction()
        {
            interactionAvailable = false;
            
            yield return new WaitForSeconds(1.5f);
            
            interactionAvailable = true;
        }

        private void PlaySaveFeedback()
        {
            if(SaveParticle)
                PoolManager.Instance.ReuseObject(SaveParticle, player.transform.position, SaveParticle.transform.rotation);
            
            if(SaveSoundCue)
                AudioManager.Instance.Play(SaveSoundCue, player.transform.position, Quaternion.identity);

            StartCoroutine(WaitAndDisableSaveText());
        }

        private IEnumerator WaitAndDisableSaveText()
        {
            SaveGameCanvas.SetActive(true);
            
            yield return new WaitForSeconds(3f);
            
            SaveGameCanvas.SetActive(false);
        }

        private void SetCanvasActive(bool active)
        {
            if (playerCanvas == null)
                return;
            
            if(active)
                playerCanvas.Show();
            else
                playerCanvas.Hide();
            
        }
    }
}
