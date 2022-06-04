using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RubeGoldbergGame
{ 
    public class UIConfirmationPanel : MonoBehaviour
    {
        // DATA //
        // Scene References
        public TextMeshProUGUI questionText;

        // Events
        public event BoolDelegate onConfirm; 


        // FUNCTIONS //
        // UI Events
        public void OnConfirmButtonClick(bool hasConfirmed)
        {
            // Runs the event
            if(onConfirm != null)
            {
                onConfirm(hasConfirmed);
            }

            // Closes the panel
            CloseConfirmationPanel();
        }


        // UI Management
        public void SetupConfirmationPanel(string questionString, BoolDelegate confirmationEvent)
        {
            // Updates text and events
            questionText.SetText(questionString);
            onConfirm += confirmationEvent;

            // Opens the UI
            gameObject.SetActive(true);
        }

        public void CloseConfirmationPanel()
        {
            onConfirm = null;
            questionText.SetText("N/A");
            gameObject.SetActive(false);
        }

    }
}