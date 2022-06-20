using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RubeGoldbergGame
{
    public class StaticUITranslator : MonoBehaviour
    {
        //TODO: This will translate all TextMeshProUGUI elements or Text elements in the scene on start

        // FUNCTIONS //
        // Unity Defaults
        private void Start()
        {
            // Gets all Text and TextMeshProUGUI elements
            Text[] allTextElements = FindObjectsOfType<Text>(true);
            TextMeshProUGUI[] allTMProElements = FindObjectsOfType<TextMeshProUGUI>(true);

            // Translates all of their text
            foreach (Text element in allTextElements)
            {
                element.text = LanguageManager.TranslateFromEnglish(element.text, LanguageManager.currentLanguage);
            }

            foreach(TextMeshProUGUI element in allTMProElements)
            {
                element.SetText(LanguageManager.TranslateFromEnglish(element.text, LanguageManager.currentLanguage));
            }
        }

    }
}