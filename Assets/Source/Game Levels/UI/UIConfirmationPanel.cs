using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

namespace RubeGoldbergGame
{ 
    public class UIConfirmationPanel : MonoBehaviour
    {
        // DATA //
        // Scene References
        public TextMeshProUGUI questionText;

        // Events
        public event ConfirmationDelegate onConfirm;

        // Cached Data
        private object confirmationParameter = null;


        // FUNCTIONS //
        // UI Events
        public void OnConfirmButtonClick(bool hasConfirmed)
        {
            // Runs the event
            if(onConfirm != null)
            {
                onConfirm(confirmationParameter, hasConfirmed);
            }

            // Closes the panel
            CloseConfirmationPanel();
        }


        // UI Management
        public void SetupConfirmationPanel(string questionString, object parameterToConfirm, ConfirmationDelegate confirmationEvent)
        {
            // Updates text and events
            questionText.SetText(questionString);
            onConfirm += confirmationEvent;
            confirmationParameter = parameterToConfirm;

            // Opens the UI
            gameObject.SetActive(true);
        }

        public void CloseConfirmationPanel()
        {
            onConfirm = null;
            confirmationParameter = null;
            questionText.SetText("N/A");
            gameObject.SetActive(false);
        }

    }
}