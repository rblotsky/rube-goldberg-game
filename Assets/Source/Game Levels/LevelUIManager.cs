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
        public TextMeshProUGUI simSpeedText;


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
    }
}