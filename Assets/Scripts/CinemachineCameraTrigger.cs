using Cinemachine;
using Enso.Characters.Player;
using Framework.LevelDesignEvents;
using UnityEngine;

public class CinemachineCameraTrigger : LevelDesignEvent
{
    [SerializeField] private CinemachineVirtualCamera VirtualCamera;

    public override void Execute()
    {
        base.Execute();

        VirtualCamera.m_Lens.OrthographicSize = 4;
        VirtualCamera.Follow = FindObjectOfType<Player>().transform;
        VirtualCamera.gameObject.SetActive(true);
    }

    public void Activate(Transform target)
    {
        
    }

    public void Deactivate()
    {
        VirtualCamera.gameObject.SetActive(false);
    }
}