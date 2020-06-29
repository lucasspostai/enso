using System.Collections.Generic;
using Enso.Characters.Enemies;
using Enso.UI;
using Framework;
using Framework.Audio;
using Framework.Utils;
using UnityEngine;

namespace Enso.Characters.Player
{
    public class Player : Fighter
    {
        [Header("Managers")] [SerializeField] private GameObject MainGameManager;
        [SerializeField] private GameObject MainAudioManager;
        [SerializeField] private GameObject MainPoolManager;
        [SerializeField] private GameObject MainInputManager;
        [SerializeField] private GameObject MainCanvas;
        [SerializeField] private GameObject MainCinemachineManager;

        [Header("References")] public PlayerAttackController AttackController;
        public PlayerGuardController GuardController;
        public PlayerRollController RollController;
        public PlayerHealController HealController;
        public PlayerMeditationController MeditationController;
        
        [HideInInspector] public List<Enemy> CurrentEnemies = new List<Enemy>();

        protected override void Awake()
        {
            base.Awake();

            InstantiateManagers();
        }

        protected override void Start()
        {
            base.Start();
            
            LoadGame();
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
                GetHealthSystem().SetHealth(playerData.Health);
                HealController.SetMaxHealingCharges(playerData.HealingCharges);
                GetBalanceSystem().SetMaxBalance(playerData.Balance);
                AttackController.StrongAttackUnlocked = playerData.StrongAttackUnlocked;
                AttackController.SpecialAttackUnlocked = playerData.SpecialAttackUnlocked;
                ExperienceManager.Instance.XpAmount = playerData.XpAmount;
                ExperienceManager.Instance.PerksAvailable = playerData.Perks;
            }
            
            var shrine = FindObjectOfType<Shrine>();

            if (shrine)
            {
                transform.position = shrine.SaveLocation.position;
                
                MeditationController.StartMeditation(shrine, true);
            }
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
            }

            //Canvas
            var playerCanvas = FindObjectOfType<PlayerCanvas>();

            if (!playerCanvas)
                Instantiate(MainCanvas);
        }

        public override void EnterCombatWith(Fighter fighter)
        {
            var enemy = fighter as Enemy;
            
            CurrentEnemies.Add(enemy);
        }

        public void RemoveEnemyFromList(Enemy enemy)
        {
            if (CurrentEnemies.Contains(enemy))
                CurrentEnemies.Remove(enemy);
        }

        public Vector2 GetDirectionToClosestEnemy()
        {
            Enemy closestEnemy = null;
            var closestDistance = float.MaxValue;

            foreach (var enemy in CurrentEnemies)
            {
                var distanceBetweenFighters = Vector2.Distance(enemy.transform.position, transform.position);
                
                if (distanceBetweenFighters < closestDistance)
                {
                    closestDistance = distanceBetweenFighters;
                    closestEnemy = enemy;
                }
            }

            return closestEnemy != null
                ? (Vector2) (closestEnemy.transform.position - transform.position).normalized
                : Vector2.zero;
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