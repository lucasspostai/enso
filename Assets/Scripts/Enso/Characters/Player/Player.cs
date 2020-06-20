﻿using Enso.UI;
using Framework;
using Framework.Audio;
using UnityEngine;

namespace Enso.Characters.Player
{
    public class Player : Fighter
    {
        [Header("Managers")] 
        [SerializeField] private GameObject MainGameManager;
        [SerializeField] private GameObject MainAudioManager;
        [SerializeField] private GameObject MainPoolManager;
        [SerializeField] private GameObject MainInputManager;
        [SerializeField] private GameObject MainCanvas;
        [SerializeField] private GameObject MainCinemachineManager;

        [Header("References")] 
        public PlayerAttackController AttackController;
        public PlayerGuardController GuardController;
        public PlayerRollController RollController;
        public PlayerHealController HealController;

        protected override void Awake()
        {
            base.Awake();

            InstantiateManagers();
        }

        private void InstantiateManagers()
        {
            //Game Manager
            var gameManager = FindObjectOfType<GameManager>();

            if (!gameManager)
                Instantiate(MainGameManager);
            
            //Audio Manager
            var audioManager = FindObjectOfType<AudioManager>();

            if (!audioManager)
                Instantiate(MainAudioManager);
            
            //Pool Manager
            var poolManager = FindObjectOfType<PoolManager>();

            if (!poolManager)
                Instantiate(MainPoolManager);
            
            //Input Manager
            var inputManager = FindObjectOfType<PlayerInput>();

            if (!inputManager)
                Instantiate(MainInputManager);
            
            //Cinemachine Manager
            var cinemachineManager = FindObjectOfType<PlayerCinemachineManager>();

            if (!cinemachineManager)
            {
                cinemachineManager = Instantiate(MainCinemachineManager).GetComponent<PlayerCinemachineManager>();
                cinemachineManager.Setup(this);
            }
            
            //Canvas
            var playerCanvas = FindObjectOfType<PlayerCanvas>();

            if (!playerCanvas)
                Instantiate(MainCanvas);
        }

        public PlayerProperties GetProperties()
        {
            return BaseProperties as PlayerProperties;
        }
    }
}
