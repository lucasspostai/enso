﻿using UnityEngine;
using UnityEngine.Audio;

namespace Framework.Audio
{
    [CreateAssetMenu(fileName = "NewSoundCue", menuName = "Enso/Audio/Sound Cue")]
    public class SoundCue : ScriptableObject
    {
        private UniqueRandom uniqueRandom;

        public AudioClip[] Clips;
        public float Volume = 1f;
        public float MinPitch = 1f;
        public float MaxPitch = 1f;
        public bool Loop;
        public float SpatialBlend;

        public AudioMixerGroup MixerGroup;

        private void OnEnable()
        {
            if (Clips != null)
                uniqueRandom = new UniqueRandom(0, Clips.Length);
        }

        public AudioClip GetClip()
        {
            return Clips[uniqueRandom.GetRandomInt()];
        }

        public float GetRandomPitch()
        {
            return Random.Range(MinPitch, MaxPitch);
        }

        public float GetPanStereo(Transform gameObjectTransform)
        {
            float xPosition = Camera.main.WorldToScreenPoint(gameObjectTransform.position).x;
            float panStereo = Mathf.Lerp(-1f, 1f, Mathf.InverseLerp(0, Screen.width, xPosition));
            
            return panStereo;
        }
    }
}