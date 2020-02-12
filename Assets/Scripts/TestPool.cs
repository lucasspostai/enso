using Framework;
using Framework.Audio;
using UnityEngine;

public class TestPool : MonoBehaviour
{
    public SoundCue TestSoundCue;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AudioManager.Instance.Play(TestSoundCue, transform.position, transform.rotation);
        }
    }
}
