using Enso;
using Framework.Utils;
using UnityEngine;

namespace Framework.LevelDesignEvents
{
    public class ChangeLevelTrigger : LevelDesignEvent
    {
        [SerializeField] private Level NextLevel;
        [SerializeField] private StageArrivalLocation ArrivalLocation;

        public override void Execute()
        {
            base.Execute();

            LevelLoader.Instance.LoadLevel(NextLevel);
        }
    }

    public enum StageArrivalLocation
    {
        Entrance,
        Exit,
        Shrine
    }
}