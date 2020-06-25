﻿using Enso.Characters.Player;
using Enso.UI;
using Framework.LevelDesignEvents;
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
        }

        public override void Execute()
        {
            base.Execute();

            isActive = true;
            InteractionElement.Enable();
        }

        public override void Exit()
        {
            base.Exit();

            isActive = false;
            InteractionElement.Disable();
        }

        private void Interact()
        {
            if (!isActive || isInteracting)
                return;

            player.MeditationController.StartMeditation(this);

            isInteracting = true;
            
            player.SaveGame();
            
            ShopCanvasElement.gameObject.SetActive(true);
            ShopCanvasElement.Enable();
        }
        
        private void Return()
        {
            if (!isActive || !isInteracting)
                return;

            player.MeditationController.EndMeditation();

            isInteracting = false;
            
            player.SaveGame();
            
            ShopCanvasElement.Disable();
        }
    }
}
