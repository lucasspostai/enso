using Framework.Audio;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Enso.UI
{
    public class ButtonFeedback : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler
    {
        public Animator FeedbackAnimator;
        public GameObject ObjectToFocus;
        
        [SerializeField] private SoundCue PointerEnterSoundCue;
        [SerializeField] private SoundCue PointerDownSoundCue;

        private static readonly int Highlight = Animator.StringToHash("Highlight");
        private static readonly int Normal = Animator.StringToHash("Normal");

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(PointerEnterSoundCue != null)
                AudioManager.Instance.Play(PointerEnterSoundCue, Vector3.zero, Quaternion.identity);
            
            if(FeedbackAnimator)
                FeedbackAnimator.SetTrigger(Highlight);

            if (ObjectToFocus)
            {
                var eventSystem = FindObjectOfType<EventSystem>();
                
                if(eventSystem)
                    eventSystem.SetSelectedGameObject(ObjectToFocus);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if(PointerDownSoundCue != null)
                AudioManager.Instance.Play(PointerDownSoundCue, Vector3.zero, Quaternion.identity);
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            if(FeedbackAnimator)
                FeedbackAnimator.SetTrigger(Normal);
        }
    }
}