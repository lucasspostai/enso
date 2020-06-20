using Cinemachine;
using Framework;
using UnityEngine;

namespace Enso.Characters.Player
{
    public class PlayerCinemachineManager : Singleton<PlayerCinemachineManager>
    {
        public void Setup(Player player)
        {
            VirtualCamera.Follow = player.transform;
        }
        
        public Camera MainCamera;
        public CinemachineVirtualCamera VirtualCamera;
        public CameraShakeController ShakeController;
    }
}
