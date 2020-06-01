using System;
using UnityEngine;

namespace Enso.UI
{
    [RequireComponent(typeof(Animator))]
    public class Element : MonoBehaviour
    {
        protected Animator ThisAnimator;

        [SerializeField] private string DisableHash = "Disable";
        [SerializeField] private string EnableHash = "Enable";
        [SerializeField] private string UpdateHash = "Update";
        private bool isAnimatorNotNull;

        private void Start()
        {
            ThisAnimator = GetComponent<Animator>();
            
            isAnimatorNotNull = ThisAnimator != null;
        }

        public void Disable()
        {
            SetTrigger(DisableHash);
        }
        
        public virtual void Enable()
        {
            SetTrigger(EnableHash);
        }

        public void UpdateInfo()
        {
            SetTrigger(UpdateHash);
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        protected void SetTrigger(string hash)
        {
            if (isAnimatorNotNull)
                ThisAnimator.SetTrigger(hash);
        }
    }
}