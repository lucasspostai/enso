using Cinemachine;
using UnityEngine;

namespace LevelDesignEvents
{
    public class EnableCamera : LevelDesignEvent
    {
        [SerializeField] private CinemachineVirtualCamera NewCinemachineCamera;

        public override void Execute()
        {
            if (NewCinemachineCamera != null)
                NewCinemachineCamera.gameObject.SetActive(true);
        }
    }
}