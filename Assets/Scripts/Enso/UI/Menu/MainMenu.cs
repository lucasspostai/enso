﻿using System.Collections;
using Framework.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Enso.UI.Menu
{
    public class MainMenu : Element
    {
        private PlayerCanvas playerCanvas;
        
        [SerializeField] private Canvas ThisCanvas;

        protected override void Start()
        {
            base.Start();

            playerCanvas = FindObjectOfType<PlayerCanvas>();
            
            if(playerCanvas != null)
                playerCanvas.gameObject.SetActive(false);
        }

        public void StartGame()
        {
            StartCoroutine(WaitAndUnloadScene());
        }

        private IEnumerator WaitAndUnloadScene()
        {
            Disable();
            
            yield return new WaitForSeconds(0.5f);
            
            GameManager.Instance.GamePaused = false;
            
            SceneManager.UnloadSceneAsync(LevelLoader.MainMenuScene);
            
            if(playerCanvas != null)
                playerCanvas.gameObject.SetActive(true);
        }
    }
}
