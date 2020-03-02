using Framework;
using Framework.Audio;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestPool : MonoBehaviour
{
    public Object SceneToLoad;
    public SoundCue TestSoundCue;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AudioManager.Instance.Play(TestSoundCue, transform.position, transform.rotation);
            LevelLoader.Instance.LoadLevel(SceneToLoad);
        }
    }
}
