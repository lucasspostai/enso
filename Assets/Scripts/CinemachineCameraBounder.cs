using System;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CinemachineCameraBounder : MonoBehaviour
{
    private Bounds targetBounds;
    private CinemachineVirtualCamera cinemachineVirtualCamera;

    private void Awake()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        
        var followCollider = cinemachineVirtualCamera.Follow.GetComponent<Collider2D>();

        targetBounds = followCollider.bounds;
        
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = targetBounds.size.x / targetBounds.size.y;
 
        if (screenRatio >= targetRatio)
        {
            cinemachineVirtualCamera.m_Lens.OrthographicSize = targetBounds.size.y / 2;
        }
        else
        {
            float differenceInSize = targetRatio / screenRatio;
            cinemachineVirtualCamera.m_Lens.OrthographicSize = targetBounds.size.y / 2 * differenceInSize;
        }
    }
}
