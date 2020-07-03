using Framework.Utils;

namespace Enso.UI
{
    public class ExtrasScreen : Element
    {
        public void LoadCredits()
        {
            MusicManager.Instance.StopAllMusics();
            LevelLoader.Instance.LoadCredits();
        }
    }
}
