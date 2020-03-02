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

        public void Play(SoundCue soundCue, Vector3 position, Quaternion rotation, float volume = 1.0f, bool randomPitch = false, float spatialBlend = 0.0f, float panStereo = 0.0f)
        {
            GameObject audioGameObject = PoolManager.Instance.ReuseObject(AudioPrefab, position, rotation).GameObject;
            var audioSource = audioGameObject.GetComponent<AudioSource>();

            if (audioSource == null) 
                return;
            
            audioSource.clip = soundCue.GetClip();
            audioSource.volume = volume != 1.0f ? volume : soundCue.Volume;
            audioSource.pitch = randomPitch ? soundCue.GetRandomPitch() : 1f;
            audioSource.loop = soundCue.Loop;
            audioSource.spatialBlend = spatialBlend != 0.0f ? spatialBlend : soundCue.SpatialBlend;
            audioSource.panStereo = panStereo != 0.0f ? panStereo : soundCue.GetPanStereo(audioGameObject.transform);
            audioSource.outputAudioMixerGroup = soundCue.MixerGroup;
            audioSource.Play();
        }
    }
}
