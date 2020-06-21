using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Framework.Audio
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private GameObject AudioPrefab;
        [SerializeField] private int MaxSimultaneousSounds;

        private void Start()
        {
            PoolManager.Instance.CreatePool(AudioPrefab, MaxSimultaneousSounds);
        }

        public void Play(SoundCue soundCue, Vector3 position, Quaternion rotation, float volume = 1.0f, bool randomPitch = true, float spatialBlend = 0.0f, float panStereo = 0.0f)
        {
            GameObject audioGameObject = PoolManager.Instance.ReuseObject(AudioPrefab, position, rotation).GameObject;
            var audioSource = audioGameObject.GetComponent<AudioSource>();

            if (audioSource == null || soundCue == null) 
                return;
            
            audioSource.clip = soundCue.GetClip();
            audioSource.volume = Math.Abs(volume - 1.0f) > 0.001f ? volume : soundCue.Volume;
            audioSource.pitch = randomPitch ? soundCue.GetRandomPitch() : 1f;
            audioSource.loop = soundCue.Loop;
            audioSource.spatialBlend = Math.Abs(spatialBlend) > 0.001f ? spatialBlend : soundCue.SpatialBlend;
            audioSource.panStereo = Math.Abs(panStereo) > 0.001f ? panStereo : soundCue.GetPanStereo(audioGameObject.transform);
            audioSource.outputAudioMixerGroup = soundCue.MixerGroup;
            audioSource.Play();
        }
    }
}
