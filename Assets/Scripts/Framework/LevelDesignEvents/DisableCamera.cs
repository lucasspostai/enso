using Cinemachine;
using UnityEngine;

namespace Framework.LevelDesignEvents
{
    public class DisableCamera : LevelDesignEvent
    {
        [SerializeField] private CinemachineVirtualCamera ActualCinemachineCamera;

        public override void Execute()
        {
            if (ActualCinemachineCamera != null)
                ActualCinemachineCamera.gameObject.SetActive(false);
        }
    }
}