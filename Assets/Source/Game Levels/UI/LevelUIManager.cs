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
        public UIConfirmationPanel confirmationOptionPanel;
        public LevelCompletionUI completionUI;
        public UIBlockSlotManager blockSlotManager;
        public TextMeshProUGUI simSpeedText;
        public TextMeshProUGUI levelTitleText;
        public UITooltip tooltipObject;
        public RectTransform blockPlacementBounds;
        public GameObject levelSettingsPanel;
        public GameObject levelDescriptionPanel;
        public TextMeshProUGUI levelDescriptionText;


        // FUNCTIONS //
        // UI Management
        public void OpenConfirmationPanel(string questionString, object parameterToConfirm, ConfirmationDelegate confirmationEvent)
        {
            // Opens the panel
            confirmationOptionPanel.SetupConfirmationPanel(questionString, parameterToConfirm, confirmationEvent);
        }

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

        public void ToggleLevelSettingPanel()
        {
            levelSettingsPanel.SetActive(!levelSettingsPanel.activeSelf);
        }

        public void ToggleLevelDescriptionPanel()
        {
            levelDescriptionPanel.SetActive(!levelDescriptionPanel.activeSelf);
        }

        public void ToggleCompletionUI(bool isOpen)
        {

            if (isOpen)
            {
                completionUI.gameObject.SetActive(true);
                simCanvas.gameObject.SetActive(false);
                editCanvas.gameObject.SetActive(false);
            }

            // Closes otherwise
            else
            {
                completionUI.gameObject.SetActive(false);
            }
        }

        public void OpenTooltipUI(string text, Vector3 rootPosition)
        {
            // Doesn't open tooltip if in sim mode
            if (editCanvas.enabled)
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
    }
}