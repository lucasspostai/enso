using System;
using Cinemachine;
using UnityEngine;

namespace Enso.Characters.Player
{
    public class PlayerCamera : MonoBehaviour
    {
        private CinemachineFramingTransposer cinemachineFramingTransposer;
        private Player player;
        private float defaultPlayerMoveSpeed;
        private float desiredLookAheadTime;

        [SerializeField] private CinemachineVirtualCamera VirtualCamera;

        private void Awake()
        {
            if (!VirtualCamera)
                return;

            cinemachineFramingTransposer = VirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

            if (cinemachineFramingTransposer)
                desiredLookAheadTime = cinemachineFramingTransposer.m_LookaheadTime;

            player = VirtualCamera.Follow.GetComponent<Player>();

            if (player)
                defaultPlayerMoveSpeed = player.GetProperties().MoveSpeed;
        }

        private void Update()
        {
            if (VirtualCamera && cinemachineFramingTransposer && player)
            {
                if (player.Movement.Velocity.magnitude > 0.1f)
                {
                    cinemachineFramingTransposer.m_LookaheadTime =
                        Math.Abs(player.Movement.Velocity.magnitude - defaultPlayerMoveSpeed) < 0.1f
                            ? desiredLookAheadTime
                            : defaultPlayerMoveSpeed / desiredLookAheadTime * player.Movement.Velocity.magnitude;
                }
                else
                { 
                   // VirtualCamera.
                }
                    
            }
        }
    }
}