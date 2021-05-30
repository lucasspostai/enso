using System;
using Enso.Characters.Player;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Enso.UI.Menu
{
    public class Option : MonoBehaviour
    {
        private bool hasFocus;
        private TMP_Dropdown thisDropdown;
        private Toggle thisToggle;
        
        [SerializeField] private Option OptionUp;
        [SerializeField] private Option OptionDown;
        [SerializeField] private GameObject ObjectToFocus;
        [SerializeField] private Animator ThisAnimator;
        
        [SerializeField] private string Normal = "Normal";
        [SerializeField] private string Highlight = "Highlight";

        public void Focus()
        {
            hasFocus = true;
            
            ThisAnimator.SetTrigger(Highlight);

            if (ObjectToFocus)
            {
                thisDropdown = ObjectToFocus.GetComponent<TMP_Dropdown>();
                thisToggle = ObjectToFocus.GetComponent<Toggle>();

                if (thisDropdown)
                {
                    PlayerInput.CancelInputDown += CloseDropdown;
                    PlayerInput.InteractionInputDown += OpenDropdown;
                }

                if (thisToggle)
                {
                    PlayerInput.InteractionInputDown += SwitchToggle;
                }
                
                EventSystem.current.SetSelectedGameObject(ObjectToFocus);
            }

            PlayerInput.UiUpInputDown += FocusOptionUp;
            PlayerInput.UiDownInputDown += FocusOptionDown;
        }
        
        public void FocusOptionUp()
        {
            if (thisDropdown)
            {
                if (thisDropdown.IsExpanded)
                    return;
            }
            
            if (!OptionUp || !hasFocus) 
                return;

            RemoveFocus();
            
            OptionUp.Focus();
        }

        public void FocusOptionDown()
        {
            if (thisDropdown)
            {
                if (thisDropdown.IsExpanded)
                    return;
            }
            
            if (!OptionDown || !hasFocus)
                return;
            
            RemoveFocus();
            
            OptionDown.Focus();
        }

        private void RemoveFocus()
        {
            if(EventSystem.current.isFocused)
                ThisAnimator.SetTrigger(Normal);
            
            hasFocus = false;
            
            if (thisDropdown)
            {
                PlayerInput.CancelInputDown -= CloseDropdown;
                PlayerInput.InteractionInputDown -= OpenDropdown;
            }
            
            if (thisToggle)
            {
                PlayerInput.InteractionInputDown -= SwitchToggle;
            }
            
            PlayerInput.UiUpInputDown -= FocusOptionUp;
            PlayerInput.UiDownInputDown -= FocusOptionDown;
        }

        private void CloseDropdown()
        {
            if (thisDropdown)
            {
                if (thisDropdown.IsExpanded)
                    thisDropdown.Hide();
            }
        }

        private void OpenDropdown()
        {
            if (thisDropdown)
            {
                if (!thisDropdown.IsExpanded)
                    thisDropdown.Show();
            }
        }

        private void SwitchToggle()
        {
            if (thisToggle)
            {
                thisToggle.isOn = !thisToggle.isOn;
            }
        }
    }
}
