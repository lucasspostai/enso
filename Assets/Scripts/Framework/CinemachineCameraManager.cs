using System;
using Cinemachine;
using UnityEngine;

namespace Framework
{
    public class CinemachineCameraManager : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera[] VirtualCameras;

        public void SetPriority()
        {
            foreach (var virtualCamera in VirtualCameras)
            {
                virtualCamera.Priority = 1;
            }
        }
    }
}
