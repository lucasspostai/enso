using UnityEngine;

namespace Enso.Characters.Enemies
{
    public class Naosuke : Enemy
    {
        public NaosukeProperties GetProperties()
        {
            return BaseProperties as NaosukeProperties;
        }
    }
}
