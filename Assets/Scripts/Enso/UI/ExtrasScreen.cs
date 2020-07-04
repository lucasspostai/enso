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

        public void DeleteSave()
        {
            SaveSystem.DeleteSave();
            LevelLoader.Instance.LoadMainMenu();
        }
    }
}
