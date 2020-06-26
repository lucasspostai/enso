using Enso;
using Framework.Utils;
using UnityEngine;

namespace Framework.LevelDesignEvents
{
    public class ChangeLevelTrigger : LevelDesignEvent
    {
        [SerializeField] private Level NextLevel;
        
        public override void Execute()
        {
            base.Execute();
            
            LevelLoader.Instance.LoadLevel(NextLevel);
        }
    }
}
