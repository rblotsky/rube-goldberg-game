using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

namespace RubeGoldbergGame
{
    public class LanguageManager
    {
        // DATA //
        // Language Database
        public static string[] availableLanguages;
        public static Dictionary<string, string[]> wordTranslations;

        // Current Language
        public static LanguageOptions currentLanguage = LanguageOptions.English;

        // Cached Data
        private static List<string> unrecordedStrings = new List<string>();

        // File IO
        public static readonly string LANGUAGE_FILE_PATH = "Languages\\TranslationsFile";

        // Events
        public static event EmptyDelegate onLanguageChange;


        // FUNCTIONS //
        // Translating Functions
        public static string TranslateFromEnglish(string englishText)
        {
            return TranslateFromEnglish(englishText, currentLanguage);
        }

        public static string TranslateFromEnglish(string englishText, LanguageOptions language)
        {
            // Caches what text to return (english by default if it fails to translate)
            string returnText = englishText;

            // Updates the english text to fit with csv file guidelines (no newlines, no commas)
            string updatedText = englishText.Replace(",", "`");
            updatedText = updatedText.Replace("\n", "\\n");

            // Gets the index of the language used
            int languageIndex = -1;
            for(int i = 0; i < availableLanguages.Length; i++)
            {
                if(availableLanguages[i].Equals(language.ToString()))
                {
                    languageIndex = i;
                    break;
                }
            }

            // Tries getting that language's translation for the text
            if(wordTranslations.TryGetValue(updatedText, out string[] translations))
            {
                if(translations.Length >= languageIndex && languageIndex != -1)
                {
                    returnText = translations[languageIndex];

                    // Adds back newlines and commas
                    returnText = returnText.Replace("\\n", "\n");
                    returnText = returnText.Replace("`", ",");
                }
            }

            else
            {
                // If we're in the unity editor, enables a debug line that will save the returnText and log it if it couldn't be translated. Also adds the text to unrecordedStrings to print to a file when the game is closed.
#if UNITY_EDITOR
                // If the text is not in unrecorded strings, logs a warning and adds it to unrecorded strings
                if (!unrecordedStrings.Contains(updatedText))
                {
                    Debug.LogWarning(string.Format("Could not translate text: \"{0}\", check if it exists in the translation file!", updatedText));
                    unrecordedStrings.Add(updatedText);
                }
#endif
            }


            // Returns whatever text it retrieved OR the english text if it couldn't retrieve anything
            return returnText;
        }


        // Data Saving
        public static void SaveUnrecordedStrings()
        {
            // Opens translation file for editing
            StreamWriter writer = File.AppendText("Assets\\Resources\\" + LANGUAGE_FILE_PATH + ".csv");

            // Writes the unrecorded strings to it
            foreach(string text in unrecordedStrings)
            {
                writer.WriteLine(text + ",");
            }

            // Closes streamwriter and clears the unrecorded strings list
            writer.Close();
            unrecordedStrings.Clear();
            Debug.Log("Wrote all unrecorded strings to translation file for translation. Also regenerated translation table.");

            // Regenerates translation table
            RegenerateTranslationTable();
        }


        // Data Loading
        public static void RegenerateTranslationTable()
        {
            // If word translation and available languages aren't null, clears them
            if(availableLanguages != null)
            {
                availableLanguages = null;
            }

            if(wordTranslations != null)
            {
                wordTranslations.Clear();
            }


            // Opens the translation file
            TextAsset translationFile = Resources.Load<TextAsset>(LANGUAGE_FILE_PATH);

            // Reads it as a csv file
            string[] lines = translationFile.text.Trim().Split("\n");

            // If there are no lines, stops
            if(lines.Length == 0)
            {
                return;
            }

            // Otherwise, gets the available languages from the first row
            availableLanguages = lines[0].Split(",");

            // Generates the table of word translations (key is the english phrase, result is a list of strings of length equalling the number of languages, in the order that they appear.)
            wordTranslations = new Dictionary<string, string[]>();

            // Each row afterwards contains an english phrase on the first column and its translations in later columns.
            for(int i = 1; i < lines.Length; i++)
            {
                // Extracts translations and english text
                string[] translations = lines[i].Split(",");

                // Gets the english text key
                string englishKey = translations[0];

                // Adds to the dictionary
                try
                {
                    wordTranslations.Add(englishKey, translations);
                }
                catch (ArgumentException e)
                {
                    Debug.LogError(string.Format("LANGUAGE FILE: There already exists a row for \"{0}\" in the language file! Error Message: {1}", englishKey, e.Message));
                }
            }
        }
    }
}