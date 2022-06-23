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
        public TMP_Dropdown languageDropdown;
        public Toggle fullscreenToggle;

        // Cached data
        private Resolution[] screenResolutions;

        // Constants
        public static readonly string GRAPHICS_QUALITY_KEY = "GraphicsQuality";
        public static readonly string FULLSCREEN_KEY = "IsFullscreen";
        public static readonly string SCREEN_RES_KEY = "ScreenResolution";
        public static readonly string LANGUAGE_SEL_KEY = "LanguageSelected";


        // FUNCTIONS //
        // Setup
        public void SetupSettingsPanel()
        {
            // Briefly enables itself
            gameObject.SetActive(true);

            // Sets up UI
            SetupResolutionsDropdown();
            SetupLanguagesDropdown();

            // Loads setting sfrom PlayerPrefs
            LoadSettingsFromPlayerPrefs();

            // Disables itself
            gameObject.SetActive(false);
        }

        public void SetupResolutionsDropdown()
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
        }

        public void SetupLanguagesDropdown()
        {
            // Gets language options
            string[] languageOptions = LanguageManager.availableLanguages;

            // Clears current options
            languageDropdown.options.Clear();
            languageDropdown.AddOptions(new List<string>(languageOptions));
        }

        public void LoadSettingsFromPlayerPrefs()
        {
            // Gets data from PlayerPrefs
            bool isFullscreen = PlayerPrefs.GetInt(FULLSCREEN_KEY, 0) == 1;
            int graphicsQualityIndex = PlayerPrefs.GetInt(GRAPHICS_QUALITY_KEY, 0);
            int resolutionIndex = PlayerPrefs.GetInt(SCREEN_RES_KEY, 0);
            int languageIndex = PlayerPrefs.GetInt(LANGUAGE_SEL_KEY, 0);


            // Updates UI (and by consequence, events run, and the actual settings are modified)
            fullscreenToggle.isOn = isFullscreen;
            graphicsQualityDropdown.value = graphicsQualityIndex;
            resolutionsDropdown.value = resolutionIndex;
            languageDropdown.value = languageIndex;
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

        public void OnLanguagesChange(int languageIndex)
        {
            // Saves to PlayerPrefs
            PlayerPrefs.SetInt(LANGUAGE_SEL_KEY, languageIndex);

            // Modifies current selected language
            LanguageManager.currentLanguage = (LanguageOptions)languageIndex;

            // Tells the static UI manager to retranslate UI
            FindObjectOfType<StaticUITranslator>().TranslateUI();
        }
    }
}