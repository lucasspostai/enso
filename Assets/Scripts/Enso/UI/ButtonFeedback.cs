using Framework.Audio;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Enso.UI
{
    public class ButtonFeedback : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
    {
        [SerializeField] private SoundCue PointerEnterSoundCue;
        [SerializeField] private SoundCue PointerDownSoundCue;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if(PointerEnterSoundCue != null)
                AudioManager.Instance.Play(PointerEnterSoundCue, Vector3.zero, Quaternion.identity);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if(PointerDownSoundCue != null)
                AudioManager.Instance.Play(PointerDownSoundCue, Vector3.zero, Quaternion.identity);
        }
    }
}