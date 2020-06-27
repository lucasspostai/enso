using Cinemachine;
using Framework;
using UnityEngine;

namespace Enso.Characters.Player
{
    public class PlayerCinemachineManager : Singleton<PlayerCinemachineManager>
    {
        public Camera MainCamera;
        public CameraShakeController ShakeController;
    }
}
