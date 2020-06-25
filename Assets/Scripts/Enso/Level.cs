using UnityEngine;

namespace Enso
{
    [CreateAssetMenu(fileName = "Level", menuName = "Enso/Level")]
    public class Level : ScriptableObject
    {
        public int LevelIndex;
        public string EnvironmentSceneName = "Area_Location_Environment";
        public string GameplaySceneName = "Area_Location_Gameplay";
    }
}
