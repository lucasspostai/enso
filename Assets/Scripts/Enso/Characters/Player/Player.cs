using System;
using System.Collections;
using System.Collections.Generic;
using Enso.Characters.Enemies;
using Enso.Characters.Enemies.Naosuke;
using Enso.Enums;
using Enso.UI;
using Framework;
using Framework.Audio;
using Framework.Utils;
using UnityEngine;

namespace Enso.Characters.Player
{
    public class Player : Fighter
    {
        private HealthBar playerHealthBar;
        
        [Header("Managers")] [SerializeField] private GameObject MainGameManager;
        [SerializeField] private GameObject MainAudioManager;
        [SerializeField] private GameObject MainPoolManager;
        [SerializeField] private GameObject MainInputManager;
        [SerializeField] private GameObject MainCanvas;
        [SerializeField] private GameObject PauseCanvas;
        [SerializeField] private GameObject MainCinemachineManager;
        [SerializeField] private GameObject MainLevelLoader;
        [SerializeField] private GameObject MainMusicManager;

        [Header("References")] public PlayerAttackController AttackController;
        public PlayerGuardController GuardController;
        public PlayerRollController RollController;
        public PlayerHealController HealController;
        public PlayerMeditationController MeditationController;

        [HideInInspector] public List<Enemy> CurrentEnemies = new List<Enemy>();

        private void OnDisable()
        {
            GetHealthSystem().Death -= OnDeath;
            GetHealthSystem().HealthValueChanged -= OnHealthValueChanged;
        }

        protected override void Awake()
        {
            base.Awake();

            InstantiateManagers();
        }

        protected override void Start()
        {
            base.Start();
            
            LoadGame();

            GetHealthSystem().HealthValueChanged += OnHealthValueChanged;

            if(GameManager.Instance.CurrentHealth == 0)
                OnHealthValueChanged();
        }

        public void SaveGame()
        {
            SaveSystem.Save(this);
        }

        public void LoadGame()
        {
            var playerData = SaveSystem.Load();

            if (playerData != null)
            {
                GetHealthSystem().SetMaxHealth(playerData.Health);

                if (GameManager.Instance.LeavingLocation)
                    GetHealthSystem().SetHealth(GameManager.Instance.CurrentHealth);
                else
                    GetHealthSystem().SetHealth(playerData.Health);

                HealController.SetMaxHealingCharges(playerData.HealingCharges);
                GetBalanceSystem().SetMaxBalance(playerData.Balance);
                AttackController.StrongAttackUnlocked = playerData.StrongAttackUnlocked;
                AttackController.SpecialAttackUnlocked = playerData.SpecialAttackUnlocked;
                ExperienceManager.Instance.XpAmount = playerData.XpAmount;
                ExperienceManager.Instance.PerksAvailable = playerData.Perks;
            }
            else
            {
                GetHealthSystem().SetMaxHealth(GetProperties().Health);

                if (GameManager.Instance.LeavingLocation)
                    GetHealthSystem().SetHealth(GameManager.Instance.CurrentHealth);
                else
                    GetHealthSystem().SetHealth(GetProperties().Health);

                HealController.SetMaxHealingCharges(GetProperties().HealingCharges);
                GetBalanceSystem().SetMaxBalance(GetProperties().BalanceAmount);
                AttackController.StrongAttackUnlocked = false;
                AttackController.SpecialAttackUnlocked = false;
                ExperienceManager.Instance.XpAmount = 0;
                ExperienceManager.Instance.PerksAvailable = 0;
            }

            var shrine = FindObjectOfType<Shrine>();

            if (shrine)
            {
                if (GameManager.Instance.LeavingLocation)
                {
                    transform.position = shrine.PlayerArrivalLocation.position;

                    GameManager.Instance.LeavingLocation = false;
                }
                else
                {
                    transform.position = shrine.SaveLocation.position;

                    MeditationController.StartMeditationLoop(shrine, true);

                    shrine.CameraManager.SetPriority();
                    shrine.VirtualCamera.Priority = 10;
                    shrine.Execute();
                    shrine.PlayerStartedHere = true;
                }
            }
            else
            {
                MeditationController.StartMeditationLoop(null, true);
            }
            
            playerHealthBar.SetupHealth();
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
                Instantiate(MainCinemachineManager).GetComponent<PlayerCinemachineManager>();

            //Canvas
            var playerCanvas = FindObjectOfType<PlayerCanvas>();

            if (!playerCanvas)
                playerCanvas = Instantiate(MainCanvas).GetComponent<PlayerCanvas>();

            playerHealthBar = playerCanvas.ThisHealthBar;

            //Pause Canvas
            var pauseCanvas = FindObjectOfType<PauseMenu>();

            if (!pauseCanvas)
                Instantiate(PauseCanvas);

            //Level Loader
            var levelLoader = FindObjectOfType<LevelLoader>();

            if (!levelLoader)
                Instantiate(MainLevelLoader);

            //Music Manager
            var musicManager = FindObjectOfType<MusicManager>();

            if (!musicManager)
            {
                Instantiate(MainMusicManager);
            }

            MusicManager.Instance.SetState(GameState.Adventure, 2f);

            GetHealthSystem().Death += OnDeath;

            GameManager.Instance.SceneHasStarted = true;
        }

        private void OnDeath()
        {
            MusicManager.Instance.StopAllMusics();
        }

        private void OnHealthValueChanged()
        {
            GameManager.Instance.CurrentHealth = GetHealthSystem().GetHealth();
        }

        public override void EnterCombatWith(Fighter fighter)
        {
            var enemy = fighter as Enemy;

            if (enemy as Naosuke != null)
            {
                MusicManager.Instance.SetState(GameState.Boss, 0);
            }
            else if (CurrentEnemies.Count == 0)
                MusicManager.Instance.SetState(GameState.Combat, 0f);

            CurrentEnemies.Add(enemy);
        }

        public void RemoveEnemyFromList(Enemy enemy)
        {
            if (CurrentEnemies.Contains(enemy))
                CurrentEnemies.Remove(enemy);

            if (enemy as Naosuke != null)
                StartCoroutine(WaitThenLoadCredits());
            else if (CurrentEnemies.Count == 0)
                MusicManager.Instance.SetState(GameState.Adventure, 20f);
        }

        private IEnumerator WaitThenLoadCredits()
        {
            GameManager.Instance.GamePaused = true;

            var playerCanvas = FindObjectOfType<PlayerCanvas>();

            if (playerCanvas)
                playerCanvas.Hide();

            yield return new WaitForSeconds(6f);

            LevelLoader.Instance.LoadCredits();
        }

        public Vector2 GetDirectionToClosestEnemy()
        {
            Enemy closestEnemy = null;
            var closestDistance = float.MaxValue;

            foreach (var enemy in CurrentEnemies)
            {
                var distanceBetweenFighters = Vector2.Distance(enemy.transform.position, transform.position);

                if (distanceBetweenFighters < closestDistance && !enemy.GetHealthSystem().IsDead)
                {
                    closestDistance = distanceBetweenFighters;
                    closestEnemy = enemy;
                }
            }

            return closestEnemy != null
                ? (Vector2) (closestEnemy.transform.position - transform.position).normalized
                : PlayerInput.Movement;
        }

        public void SetActionDirection()
        {
            if (PlayerInput.Movement != Vector2.zero &&
                !AnimationHandler.IsAnyGuardAnimationPlaying() && CurrentEnemies.Count == 0)
            {
                AnimationHandler.SetFacingDirection(PlayerInput.Movement);
            }
            else if (CurrentEnemies.Count > 0)
            {
                AnimationHandler.SetFacingDirection(
                    GetDirectionToClosestEnemy());
            }
        }

        public PlayerProperties GetProperties()
        {
            return BaseProperties as PlayerProperties;
        }
    }
}