using Enso.Characters.Player;
using UnityEngine;

namespace Framework.LevelDesignEvents
{
    public class ChangeOrthographicSize : LevelDesignEvent
    {
        [SerializeField] private float OrthographicSize;
        
        public override void Execute()
        {
            base.Execute();

            FindObjectOfType<PlayerCinemachineManager>().VirtualCamera.m_Lens.OrthographicSize = OrthographicSize;
        }
    }
}
