using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

namespace RubeGoldbergGame
{
    public class LevelUIManager : MonoBehaviour
    {
        // DATA //
        // UI References
        public Canvas simCanvas;
        public Canvas editCanvas;
        public Canvas completionCanvas;
        public TextMeshProUGUI simSpeedText;
        public TextMeshProUGUI levelTitleText;
        public UITooltip tooltipObject;
        public RectTransform blockPlacementBounds;
        public UIBlockSlotManager placeablesButtons;
        public GameObject levelInfoPanel;
        public TextMeshProUGUI levelDescriptionText;
        public TextMeshProUGUI levelCompletionText;
        public Button nextLevelButton;

        // Text values
        public string completedLevelText = "Level Complete!";
        public string failedLevelText = "Level Failed!";



        // FUNCTIONS //
        // UI Management
        public void ToggleSimulationUI(bool inSimMode)
        {
            simCanvas.gameObject.SetActive(inSimMode);
            editCanvas.gameObject.SetActive(!inSimMode);
        }

        public void UpdateSimSpeedText(int simSpeed)
        {
            simSpeedText.SetText(simSpeed + " %");
        }

        public void SetBasicInterface(LevelData data)
        {
            // If there is level data, updates level description in UI
            if (data != null)
            {
                levelTitleText.SetText(data.displayName);
                levelDescriptionText.SetText(data.displayDescription);
            }

            // Otherwise, sets it to error message
            else
            {
                levelTitleText.SetText("LevelData not found for this level ID.");
                levelDescriptionText.SetText("LevelData not found for this level ID.");
            }    
        }

        public void ToggleLevelInfoPanel()
        {
            levelInfoPanel.SetActive(!levelInfoPanel.activeSelf);
        }

        public void ToggleCompletionUI(bool isOpen)
        {
            // Opens if it should be open
            if(isOpen)
            {
                completionCanvas.gameObject.SetActive(true);
                simCanvas.gameObject.SetActive(false);
                editCanvas.gameObject.SetActive(false);
            }

            // Closes otherwise
            else
            {
                completionCanvas.gameObject.SetActive(false);
            }
        }

        public void UpdateCompletionUIContent(Completion completionType)
        {
            if(completionType == Completion.Passed)
            {
                nextLevelButton.interactable = true;
                levelCompletionText.SetText(completedLevelText);
            }
            else
            {
                nextLevelButton.interactable = false;
                levelCompletionText.SetText(failedLevelText);
            }
        }

        public void OpenTooltipUI(string text, Vector3 rootPosition)
        {
            // If there is text, opens the tooltip and updates its position
            if (text.Length > 0)
            {
                tooltipObject.gameObject.SetActive(true);
                tooltipObject.UpdateText(text);
                tooltipObject.UpdatePosition(rootPosition);
            }

            // Otherwise, closes it
            else
            {
                CloseTooltipUI();
            }
        }

        public void CloseTooltipUI()
        {
            // Deactivates tooltip object
            tooltipObject.gameObject.SetActive(false);
        }

        public bool WithinPlacementBounds(Vector3 position)
        {
            return blockPlacementBounds.rect.Contains(position);
        }

        public void LoadLevel(string levelName)
        {
            SceneManager.LoadScene(levelName, LoadSceneMode.Single);
        }

        
    }
}