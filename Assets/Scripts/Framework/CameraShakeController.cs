﻿using System.Collections;
using Cinemachine;
using UnityEngine;

namespace Framework
{
    public class CameraShakeController : MonoBehaviour
    {
        private CinemachineBasicMultiChannelPerlin virtualCameraNoise;
        private Coroutine shakeCoroutine;
        private float shakeDuration;
        private float shakeAmplitude;
        private float shakeFrequency;

        public void SetNoise(CinemachineVirtualCamera virtualCamera)
        {
            virtualCameraNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        public void Shake(CameraShakeProfile cameraShakeProfile)
        {
            shakeDuration = cameraShakeProfile.ShakeDuration;
            shakeAmplitude = cameraShakeProfile.ShakeAmplitude;
            shakeFrequency = cameraShakeProfile.ShakeFrequency;
            
            if(shakeCoroutine != null)
                StopCoroutine(shakeCoroutine);
            
            shakeCoroutine = StartCoroutine(ShakeThenStop());
        }

        public void StopShake()
        {
            virtualCameraNoise.m_AmplitudeGain = 0f;
        }

        private IEnumerator ShakeThenStop()
        {
            if (virtualCameraNoise)
            {
                virtualCameraNoise.m_AmplitudeGain = shakeAmplitude;
                virtualCameraNoise.m_FrequencyGain = shakeFrequency;
            
                yield return new WaitForSeconds(shakeDuration);

                StopShake();
            }
            else
            {
                yield return null;
            }
        }
    }
}
