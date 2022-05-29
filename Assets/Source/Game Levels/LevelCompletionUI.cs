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
        public TextMeshProUGUI bestTimeText;
        public TextMeshProUGUI bestBlocksText;

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
                nextLevelButton.interactable = false;
                levelCompletionText.SetText(failedLevelText);
            }

            // Updates the level bests
            if(levelData.bestTime >= 0)
            {
                bestTimeText.SetText(levelData.bestTime.ToString());
            }
            else
            {
                bestTimeText.SetText("None");
            }

            if(levelData.bestBlocksUsed >= 0)
            {
                bestBlocksText.SetText(levelData.bestBlocksUsed.ToString());
            }
            else
            {
                bestBlocksText.SetText("None");
            }

            // If there is no next level, doesn't display a next level button
            if(levelData.nextLevel == null)
            {
                nextLevelButton.gameObject.SetActive(false);
            }
        }
    }
}