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

        [SerializeField] private AudioSource AdventureMusic;
        [SerializeField] private AudioSource CombatMusic;
        [SerializeField] private AudioSource BossMusic;

        [SerializeField] private float AdventureMusicFade = 2f;
        [SerializeField] private float CombatMusicFade = 2f;
        [SerializeField] private float BossMusicFade = 2f;
        [SerializeField] private float StopMusicFade = 2f;

        [HideInInspector] public bool BossMusicIsPlaying;

        private void Start()
        {
            SetState(GameState.Adventure, 0);
        }

        public void SetState(GameState gameState, float delayToStart, bool startFromBeginning = false)
        {
            switch (gameState)
            {
                case GameState.Adventure:

                    if (!AdventureMusic.isPlaying)
                        AdventureMusic.Play();

                    if (startFromBeginning)
                        AdventureMusic.time = 0;

                    SetAdventureMusicVolume(1, delayToStart, false);
                    SetCombatMusicVolume(0, 0, false);
                    SetBossMusicVolume(0, 0, false);
                    break;

                case GameState.Combat:

                    if (!CombatMusic.isPlaying)
                        CombatMusic.Play();

                    if (startFromBeginning)
                        CombatMusic.time = 0;

                    SetAdventureMusicVolume(0, 0, false);
                    SetCombatMusicVolume(1, delayToStart, false);
                    SetBossMusicVolume(0, 0, false);
                    break;

                case GameState.Boss:

                    if (!BossMusic.isPlaying)
                        BossMusic.Play();

                    if (startFromBeginning)
                        BossMusic.time = 0;

                    BossMusicIsPlaying = true;

                    SetAdventureMusicVolume(0, 0, false);
                    SetCombatMusicVolume(0, 0, false);
                    SetBossMusicVolume(1, delayToStart, false);
                    break;

                default:

                    SetAdventureMusicVolume(1, delayToStart, false);
                    SetCombatMusicVolume(0, 0, false);
                    SetBossMusicVolume(0, 0, false);
                    break;
            }
        }

        private void SetAdventureMusicVolume(float targetVolume, float delayToStart, bool stopAfter)
        {
            if (adventureCoroutine != null)
                StopCoroutine(adventureCoroutine);

            adventureCoroutine = StartCoroutine(StartFade(AdventureMusic,
                stopAfter ? StopMusicFade : AdventureMusicFade, targetVolume, delayToStart));
        }

        private void SetCombatMusicVolume(float targetVolume, float delayToStart, bool stopAfter)
        {
            if (combatCoroutine != null)
                StopCoroutine(combatCoroutine);

            combatCoroutine =
                StartCoroutine(StartFade(CombatMusic, stopAfter ? StopMusicFade : CombatMusicFade, targetVolume,
                    delayToStart));
        }

        private void SetBossMusicVolume(float targetVolume, float delayToStart, bool stopAfter)
        {
            if (bossCoroutine != null)
                StopCoroutine(bossCoroutine);

            bossCoroutine =
                StartCoroutine(StartFade(BossMusic, stopAfter ? StopMusicFade : BossMusicFade, targetVolume,
                    delayToStart));
        }

        public void StopAllMusics()
        {
            StartCoroutine(WaitThenStopAllMusics());
        }

        private IEnumerator WaitThenStopAllMusics()
        {
            SetAdventureMusicVolume(0, 0,true);
            SetCombatMusicVolume(0, 0,true);
            SetBossMusicVolume(0, 0,true);

            yield return new WaitForSeconds(AdventureMusicFade);

            AdventureMusic.Stop();
            CombatMusic.Stop();
            BossMusic.Stop();

            BossMusicIsPlaying = false;
        }

        private IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume, float delayToStart)
        {
            yield return new WaitForSeconds(delayToStart);

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