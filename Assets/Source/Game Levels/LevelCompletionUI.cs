using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RubeGoldbergGame
{
    public class LevelCompletionUI : MonoBehaviour
    {
        // DATA //
        // References
        public Button nextLevelButton;
        public TextMeshProUGUI levelCompletionText;
        public TextMeshProUGUI[] objectiveTexts;
        public Image[] objectiveCompletionIcons;
        public TextMeshProUGUI bestObjectivesText;
        public GameObject newBestObjectivesNotification;

        // Text values
        public string completedLevelText = "Level Complete!";
        public string failedLevelText = "Level Failed!";


        // FUNCTIONS //
        // UI Management
        public void UpdateContent(Completion newCompletion, LevelData levelData, bool[] objectivesCompletedThisRun, bool newBest)
        {
            // Updates title and next level button
            if (newCompletion == Completion.Passed)
            {
                nextLevelButton.interactable = true;
                levelCompletionText.SetText(LanguageManager.TranslateFromEnglish(completedLevelText));
            }
            else
            {
                // Only disables next level if hasn't won yet
                if (levelData.completionStatus == Completion.NotPassed)
                { 
                    nextLevelButton.interactable = false; 
                }

                levelCompletionText.SetText(LanguageManager.TranslateFromEnglish(failedLevelText));
            }

            // Updates objectives completed this run
            for(int i = 0; i < levelData.objectiveDescriptions.Length; i++)
            {
                objectiveTexts[i].SetText(LanguageManager.TranslateFromEnglish(levelData.objectiveDescriptions[i])); 
            }

            // Updates best objectives completed
            bestObjectivesText.SetText(levelData.bestObjectivesCompleted.ToString());
            if(newBest)
            {
                newBestObjectivesNotification.SetActive(true);
            }
            else
            {
                newBestObjectivesNotification.SetActive(false);
            }


            // Updates objective completion display
            for (int i = 0; i < levelData.objectiveDescriptions.Length; i++)
            {
                if (objectivesCompletedThisRun[i] == true)
                {
                    objectiveCompletionIcons[i].color = Color.green;
                }
                else
                {
                    objectiveCompletionIcons[i].color = Color.red;
                }
            }

            // If there is no next level, doesn't display a next level button
            if(levelData.nextLevel == null)
            {
                nextLevelButton.gameObject.SetActive(false);
            }
        }
    }
}