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

        // Text values
        public string completedLevelText = "Level Complete!";
        public string failedLevelText = "Level Failed!";


        // FUNCTIONS //
        // UI Management
        public void UpdateContent(Completion newCompletion, LevelData levelData)
        {
            // Updates title and next level button
            if (newCompletion == Completion.Passed)
            {
                nextLevelButton.interactable = true;
                levelCompletionText.SetText(completedLevelText);
            }
            else
            {
                // Only disables next level if hasn't won yet
                if (levelData.completionStatus == Completion.NotPassed)
                { 
                    nextLevelButton.interactable = false; 
                }

                levelCompletionText.SetText(failedLevelText);
            }

            // Updates objective text
            for(int i = 0; i < levelData.objectiveDescriptions.Length; i++)
            {
                objectiveTexts[i].SetText(levelData.objectiveDescriptions[i]); 
            }

            // Updates objective completion display
            for (int i = 0; i < levelData.objectivesCompleted.Length; i++)
            {
                if (levelData.objectivesCompleted[i])
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