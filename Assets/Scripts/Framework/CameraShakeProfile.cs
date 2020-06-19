using UnityEngine;

namespace Framework
{
    [CreateAssetMenu(fileName = "CameraShakeProfile", menuName = "Enso/Camera Shake Profile")]
    public class CameraShakeProfile : ScriptableObject
    {
        public float ShakeDuration = 0.3f;
        public float ShakeAmplitude = 1.2f;
        public float ShakeFrequency = 2.0f;
    }
}
