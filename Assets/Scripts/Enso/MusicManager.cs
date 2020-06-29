using System;
using System.Collections;
using Enso.Enums;
using Framework;
using UnityEngine;

namespace Enso
{
    public class MusicManager : Singleton<MusicManager>
    {
        private Coroutine adventureCoroutine;
        private Coroutine combatCoroutine;
        private Coroutine bossCoroutine;
        private float delayToStartMusic;

        [SerializeField] private AudioSource AdventureMusic;
        [SerializeField] private AudioSource CombatMusic;
        [SerializeField] private AudioSource BossMusic;

        [SerializeField] private float AdventureMusicFade = 2f;
        [SerializeField] private float CombatMusicFade = 2f;
        [SerializeField] private float BossMusicFade = 2f;

        [HideInInspector] public bool BossMusicIsPlaying;

        private void Start()
        {
            SetState(GameState.Adventure, 0);
        }

        public void SetState(GameState gameState, float delayToStart)
        {
            delayToStartMusic = delayToStart;
            
            switch (gameState)
            {
                case GameState.Adventure:
                    
                    if(!AdventureMusic.isPlaying)
                        AdventureMusic.Play();
                    
                    SetAdventureMusicVolume(1);
                    SetCombatMusicVolume(0);
                    SetBossMusicVolume(0);
                    break;
                
                case GameState.Combat:
                    
                    if(!CombatMusic.isPlaying)
                        CombatMusic.Play();
                    
                    SetAdventureMusicVolume(0);
                    SetCombatMusicVolume(1);
                    SetBossMusicVolume(0);
                    break;
                
                case GameState.Boss:
                    
                    if(!BossMusic.isPlaying)
                        BossMusic.Play();
                    
                    BossMusicIsPlaying = true;
                    
                    SetAdventureMusicVolume(0);
                    SetCombatMusicVolume(0);
                    SetBossMusicVolume(1);
                    break;
                
                default:
                    
                    SetAdventureMusicVolume(1);
                    SetCombatMusicVolume(0);
                    SetBossMusicVolume(0);
                    break;
            }
        }

        private void SetAdventureMusicVolume(float targetVolume)
        {
            if(adventureCoroutine != null)
                StopCoroutine(adventureCoroutine);

            adventureCoroutine = StartCoroutine(StartFade(AdventureMusic, AdventureMusicFade, targetVolume));
        }
        
        private void SetCombatMusicVolume(float targetVolume)
        {
            if(combatCoroutine != null)
                StopCoroutine(combatCoroutine);

            combatCoroutine = StartCoroutine(StartFade(CombatMusic, CombatMusicFade, targetVolume));
        }
        
        private void SetBossMusicVolume(float targetVolume)
        {
            if(bossCoroutine != null)
                StopCoroutine(bossCoroutine);

            bossCoroutine = StartCoroutine(StartFade(BossMusic, BossMusicFade, targetVolume));
        }
        
        public void StopAllMusics()
        {
            StartCoroutine(WaitThenStopAllMusics());
        }

        private IEnumerator WaitThenStopAllMusics()
        {
            SetAdventureMusicVolume(0);
            SetCombatMusicVolume(0);
            SetBossMusicVolume(0);
            
            yield return new WaitForSeconds(AdventureMusicFade);
            
            AdventureMusic.Stop();
            CombatMusic.Stop();
            BossMusic.Stop();
            
            BossMusicIsPlaying = false;
        }

        private IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
        {
            if (audioSource.volume > 0.9f)
                yield return null;
            else
                yield return new WaitForSeconds(delayToStartMusic);
            
            float currentTime = 0;
            float startVolume = audioSource.volume;

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / duration);
                yield return null;
            }
        }
    }
}
