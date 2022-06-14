using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

namespace RubeGoldbergGame
{
    public class UISettingsPanel : MonoBehaviour
    {
        // DATA //
        // Scene References
        public TMP_Dropdown resolutionsDropdown;
        public TMP_Dropdown graphicsQualityDropdown;
        public Toggle fullscreenToggle;

        // Cached data
        private Resolution[] screenResolutions;

        // Constants
        public static readonly string GRAPHICS_QUALITY_KEY = "GraphicsQuality";
        public static readonly string FULLSCREEN_KEY = "IsFullscreen";
        public static readonly string SCREEN_RES_KEY = "ScreenResolution";


        // FUNCTIONS //
        // Unity Defaults
        private void Start()
        {
            // Gets screen resolutions
            screenResolutions = Screen.resolutions;

            // Removes existing resolution options
            resolutionsDropdown.options.Clear();

            // Creates a list of new resolution options, caches the current used one, and adds it to the resolutions dropdown.
            int currentResIndex = -1;
            List<string> resolutionOptions = new List<string>();
            for (int i = 0; i < screenResolutions.Length; i++)
            {
                string optionText = screenResolutions[i].width + " x " + screenResolutions[i].height;
                resolutionOptions.Add(optionText);

                if (screenResolutions[i].width == Screen.currentResolution.width && screenResolutions[i].height == Screen.currentResolution.height)
                {
                    currentResIndex = i;
                }
            }

            resolutionsDropdown.AddOptions(resolutionOptions);
            resolutionsDropdown.value = currentResIndex;
        }


        // Setup
        public void LoadSettingsFromPlayerPrefs()
        {
            // Gets data from PlayerPrefs
            bool isFullscreen = PlayerPrefs.GetInt(FULLSCREEN_KEY, 0) == 1;
            int graphicsQualityIndex = PlayerPrefs.GetInt(GRAPHICS_QUALITY_KEY, 0);
            int resolutionsIndex = PlayerPrefs.GetInt(SCREEN_RES_KEY, 0);

            // Updates UI (and by consequence, events run, and the actual settings are modified)
            fullscreenToggle.isOn = isFullscreen;
            graphicsQualityDropdown.value = graphicsQualityIndex;
            resolutionsDropdown.value = resolutionsIndex;
        }


        // UI Events
        public void OnGraphicsQualityChange(int qualityIndex)
        {
            // Saves to PlayerPrefs
            PlayerPrefs.SetInt(GRAPHICS_QUALITY_KEY, qualityIndex);

            // Modifies data
            QualitySettings.SetQualityLevel(qualityIndex);
        }

        public void SetFullscreen(bool isFullscreen)
        {
            // Saves to PlayerPrefs
            if (isFullscreen)
            {
                PlayerPrefs.SetInt(FULLSCREEN_KEY, 1);
            }
            else
            {
                PlayerPrefs.SetInt(FULLSCREEN_KEY, 0);
            }

            // Modifies data
            Screen.fullScreen = isFullscreen;
        }

        public void SetResolution(int resolutionIndex)
        {
            // Saves to PlayerPrefs
            PlayerPrefs.SetInt(SCREEN_RES_KEY, resolutionIndex);

            // Sets the resolution to the one at the current selected index, fullscreen stays the same as it currently is
            Screen.SetResolution(screenResolutions[resolutionIndex].width, screenResolutions[resolutionIndex].height, Screen.fullScreen);
        }
    }
}