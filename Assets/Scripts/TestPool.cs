using Cinemachine;
using Framework;
using Framework.Audio;
using Framework.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestPool : MonoBehaviour
{
    public Object SceneToLoad;
    public SoundCue TestSoundCue;
    public CinemachineVirtualCamera NewCamera;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            //AudioManager.Instance.Play(TestSoundCue, transform.position, transform.rotation);
            LevelLoader.Instance.ReloadLevel();
            //NewCamera.gameObject.SetActive(!NewCamera.gameObject.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
