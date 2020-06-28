using Cinemachine;
using Enso.Characters.Player;
using UnityEngine;

namespace Framework.LevelDesignEvents
{
    public class CinemachineCameraTrigger : LevelDesignEvent
    {
        private Player player;
        
        [SerializeField] private bool TargetPlayer = true;
        [SerializeField] private bool IsFirstCamera;
        [SerializeField] private CinemachineCameraManager CameraManager;
        [SerializeField] private CinemachineVirtualCamera VirtualCamera;

        private void Start()
        {
            player = FindObjectOfType<Player>();
            
            if(IsFirstCamera)
                Execute();
        }

        public override void Execute()
        {
            base.Execute();

            if(TargetPlayer && player)
                VirtualCamera.Follow = player.transform;
            
            CameraManager.SetPriority();
            VirtualCamera.Priority = 10;
            
            PlayerCinemachineManager.Instance.ShakeController.SetNoise(VirtualCamera);
            PlayerCinemachineManager.Instance.ShakeController.StopShake();
        }
    }
}