using UnityEngine;

namespace Enso.Characters
{
    public class XpReceiver : MonoBehaviour
    {
        public void GainXp(int xpAmount)
        {
            print(xpAmount);
            
            ExperienceManager.Instance.GainXp(xpAmount);
        }
    }
}
