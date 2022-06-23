using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RubeGoldbergGame
{
    public class StaticUITranslator : MonoBehaviour
    {
        // FUNCTIONS //
        // Management
        public void TranslateUI()
        {
            // Gets all TMPro and regular text boxes, checks if their text is in the language manager, and if so, translates them as required.
            Text[] allTexts = FindObjectsOfType<Text>(true);
            TextMeshProUGUI[] allTMPros = FindObjectsOfType<TextMeshProUGUI>(true);

            // Loops through them all, updates their text
            foreach(TextMeshProUGUI textTMPro in allTMPros)
            {
                if(LanguageManager.EnglishKeyExists(textTMPro.text))
                {
                    textTMPro.SetText(LanguageManager.TranslateFromEnglish(textTMPro.text));
                }
            }

            foreach (Text text in allTexts)
            {
                if (LanguageManager.EnglishKeyExists(text.text))
                {
                    text.text = (LanguageManager.TranslateFromEnglish(text.text));
                }
            }
        }
    }
}