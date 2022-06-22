using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RubeGoldbergGame
{
    public class StaticUITranslator : MonoBehaviour
    {
        // DATA //
        // Cached data
        private List<Text> translatedTextBoxes = new List<Text>();
        private List<TextMeshProUGUI> translatedTMPro = new List<TextMeshProUGUI>();


        // FUNCTIONS //
        // Unity Defaults
        private void Start()
        {
            TranslateUIAndCacheTranslated();
        }


        // Events
        public void TranslateUI()
        {
            // Gets all Text and TextMeshProUGUI elements
            Text[] allTextElements = FindObjectsOfType<Text>(true);
            TextMeshProUGUI[] allTMProElements = FindObjectsOfType<TextMeshProUGUI>(true);

            // Translates all of their text
            foreach (Text element in allTextElements)
            {
                element.text = LanguageManager.TranslateFromEnglish(element.text, LanguageManager.currentLanguage);
            }

            foreach (TextMeshProUGUI element in allTMProElements)
            {
                element.SetText(LanguageManager.TranslateFromEnglish(element.text, LanguageManager.currentLanguage));
            }
        }

        public void TranslateUIAndCacheTranslated()
        {
            // Translates UI elements, caching which text elements got translated properly
            //TODO
        }

    }
}