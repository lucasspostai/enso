using System.Collections.Generic;
using Enso.Characters.Player;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Enso.UI.Menu
{
    public class OptionsMenu : MonoBehaviour
    {
        private Resolution[] resolutions;

        [SerializeField] private AudioMixer MasterAudioMixer;
        [SerializeField] private TextMeshProUGUI MasterVolumeText;
        [SerializeField] private TextMeshProUGUI MusicVolumeText;
        [SerializeField] private TextMeshProUGUI AmbienceVolumeText;
        [SerializeField] private TextMeshProUGUI SfxVolumeText;
        [SerializeField] private TMP_Dropdown ResolutionsDropdown;
        [SerializeField] private Button[] HeaderButtons;
        [SerializeField] private GameObject AudioOptions;
        [SerializeField] private GameObject GraphicsOptions;
        [SerializeField] private GameObject ControlsOptions;
        [SerializeField] private GameObject ReturnObjectToActivate;

        private void OnEnable()
        {
            PlayerInput.PageLeftInputDown += GoToLeftOption;
            PlayerInput.PageRightInputDown += GoToRightOption;
            PlayerInput.CancelInputDown += Return;
        }

        private void OnDisable()
        {
            PlayerInput.PageLeftInputDown -= GoToLeftOption;
            PlayerInput.PageRightInputDown -= GoToRightOption;
            PlayerInput.CancelInputDown -= Return;
        }

        private void Start()
        {
            SetResolutionProperties();
        }

        private void Return()
        {
            var element = GetComponent<Element>();

            if (!element) 
                return;
            
            if (!element.IsEnabled)
                return;
            
            element.Disable();
                
            if(ReturnObjectToActivate)
                ReturnObjectToActivate.SetActive(true);
        }

        #region Audio Settings

        public void SetMasterVolume(float volume)
        {
            SetVolume("MasterVolume", volume, MasterVolumeText);
        }

        public void SetMusicVolume(float volume)
        {
            SetVolume("MusicVolume", volume, MusicVolumeText);
        }

        public void SetAmbienceVolume(float volume)
        {
            SetVolume("AmbienceVolume", volume, AmbienceVolumeText);
        }

        public void SetSfxVolume(float volume)
        {
            SetVolume("SFXVolume", volume, SfxVolumeText);
        }

        private void SetVolume(string exposedParameterName, float value, TextMeshProUGUI textMeshProUgui)
        {
            MasterAudioMixer.SetFloat(exposedParameterName, Mathf.Log10(value) * 20);
            textMeshProUgui.text = $"{value * 100:0}" + "%";
        }

        #endregion

        #region Graphics Settings

        public void SetResolution(int resolutionIndex)
        {
            var resolution = resolutions[resolutionIndex];

            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }

        public void SetFullscreen(bool fullscreen)
        {
            Screen.fullScreen = fullscreen;
        }

        private void SetResolutionProperties()
        {
            resolutions = Screen.resolutions;

            ResolutionsDropdown.ClearOptions();

            List<string> resolutionOptions = new List<string>();

            int currentResolutionIndex = 0;

            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + " x " + resolutions[i].height;

                resolutionOptions.Add(option);

                if (resolutions[i].width == Screen.resolutions[i].width &&
                    resolutions[i].height == Screen.resolutions[i].height)
                {
                    currentResolutionIndex = i;
                }
            }

            ResolutionsDropdown.AddOptions(resolutionOptions);
            ResolutionsDropdown.value = currentResolutionIndex;
            ResolutionsDropdown.RefreshShownValue();
        }

        #endregion

        private void GoToLeftOption()
        {
            if (AudioOptions.activeSelf)
                EnableControls();
            else if (GraphicsOptions.activeSelf)
                EnableAudio();
            else if (ControlsOptions.activeSelf)
                EnableGraphics();
        }

        private void GoToRightOption()
        {
            if (AudioOptions.activeSelf)
                EnableGraphics();
            else if (GraphicsOptions.activeSelf)
                EnableControls();
            else if (ControlsOptions.activeSelf)
                EnableAudio();
        }

        private void EnableOption(bool audioOption, bool graphicsOption, bool controlsOption)
        {
            AudioOptions.SetActive(audioOption);
            GraphicsOptions.SetActive(graphicsOption);
            ControlsOptions.SetActive(controlsOption);
        }

        public void EnableAudio()
        {
            EnableOption(true, false, false);
            HeaderButtons[0].Select();
        }

        public void EnableGraphics()
        {
            EnableOption(false, true, false);
            HeaderButtons[1].Select();
        }

        public void EnableControls()
        {
            EnableOption(false, false, true);
            HeaderButtons[2].Select();
        }
    }
}