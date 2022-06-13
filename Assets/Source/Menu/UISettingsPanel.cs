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

        // Cached data
        private Resolution[] screenResolutions;


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


        // UI Events
        public void OnGraphicsQualityChange(int qualityIndex)
        {
            QualitySettings.SetQualityLevel(qualityIndex);
        }

        public void SetFullscreen(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
        }

        public void SetResolution(int resolutionIndex)
        {
            // Sets the resolution to the one at the current selected index, fullscreen stays the same as it currently is
            Screen.SetResolution(screenResolutions[resolutionIndex].width, screenResolutions[resolutionIndex].height, Screen.fullScreen);
        }
    }
}