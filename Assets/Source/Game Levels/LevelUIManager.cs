using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
            if (data != null)
            {
                levelTitleText.SetText(data.LevelName);
            }

            else
            {
                levelTitleText.SetText("LevelData not found for this level ID.");
            }    
        }

        public void ToggleCompletionUI(bool isOpen)
        {
            if(isOpen)
            {
                completionCanvas.gameObject.SetActive(true);
                simCanvas.gameObject.SetActive(false);
                editCanvas.gameObject.SetActive(false);
            }

            else
            {
                completionCanvas.gameObject.SetActive(false);
            }
        }

        public void ToggleTooltipUI(string toolTipText)
        {
            if (toolTipText.Length > 0)
            {
                tooltipObject.GetComponent<TextMeshProUGUI>().SetText(toolTipText);
                tooltipObject.SetActive(true);
                
            }
            else
            {
                tooltipObject.GetComponent<TextMeshProUGUI>().SetText("");
                tooltipObject.SetActive(false);
            }
            
        }
    }
}