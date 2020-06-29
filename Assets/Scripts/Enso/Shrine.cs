using System.Collections;
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
        private bool isInteracting;
        private Player player;

        [SerializeField] private Element InteractionElement;
        [SerializeField] private Element ShopCanvasElement;
        [SerializeField] private GameObject SaveGameCanvas;
        [SerializeField] private GameObject SaveParticle;
        [SerializeField] private SoundCue SaveSoundCue;
        [SerializeField] private Level ThisLevel;
        
        public Transform SaveLocation;

        private void OnEnable()
        {
            PlayerInput.InteractionInputDown += Interact;
            PlayerInput.ReturnInputDown += Return;
        }
        
        private void OnDisable()
        {
            PlayerInput.InteractionInputDown -= Interact;
            PlayerInput.ReturnInputDown -= Return;
        }

        private void Start()
        {
            player = FindObjectOfType<Player>();
            
            InteractionElement.Disable();
            
            PoolManager.Instance.CreatePool(SaveParticle, 1);
        }

        public override void Execute()
        {
            base.Execute();

            isActive = true;
            InteractionElement.Enable();

            GameManager.Instance.ShrineActive = true;
        }

        public override void Exit()
        {
            base.Exit();

            isActive = false;
            InteractionElement.Disable();
            
            GameManager.Instance.ShrineActive = false;
        }

        private void Interact()
        {
            if (!isActive || isInteracting)
                return;

            player.MeditationController.StartMeditation(this, false);

            isInteracting = true;
            
            player.SaveGame();

            ShopCanvasElement.gameObject.SetActive(true);
            ShopCanvasElement.Enable();
            
            InteractionElement.Disable();
        }
        
        private void Return()
        {
            if (!isActive || !isInteracting)
                return;

            player.MeditationController.EndMeditation();

            isInteracting = false;

            LevelLoader.Instance.CurrentLevelIndex = ThisLevel.LevelIndex;
            player.SaveGame();

            PlaySaveFeedback();
            
            ShopCanvasElement.Disable();
            
            InteractionElement.Enable();
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
    }
}
