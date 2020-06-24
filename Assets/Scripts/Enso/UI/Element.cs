using System;
using UnityEngine;

namespace Enso.UI
{
    [RequireComponent(typeof(Animator))]
    public class Element : MonoBehaviour
    {
        protected Animator ThisAnimator;
        protected bool IsEnabled;

        [SerializeField] private string DisableHash = "Disable";
        [SerializeField] private string EnableHash = "Enable";
        [SerializeField] private string UpdateHash = "Update";
        private bool isAnimatorNotNull;

        protected virtual void Start()
        {
            ThisAnimator = GetComponent<Animator>();
            
            isAnimatorNotNull = ThisAnimator != null;
        }

        public virtual void Disable()
        {
            SetTrigger(DisableHash);
            IsEnabled = false;
        }
        
        public virtual void Enable()
        {
            SetTrigger(EnableHash);
            IsEnabled = true;
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