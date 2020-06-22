using UnityEngine;

namespace Enso.Characters.Enemies
{
    public abstract class EnemyProperties : FighterProperties
    {
        [Header("Experience")] public int XpAmount;
    }
}
