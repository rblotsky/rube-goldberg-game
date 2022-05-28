using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace RubeGoldbergGame
{
    public class UILevelSlot : MonoBehaviour
    {
        // DATA //
        // References
        public TextMeshProUGUI levelNameText;
        public Image levelStatusHighlight;
        public MainMenuManager menuManager;
        public LevelData storedLevelData;


        // FUNCTIONS //
        // Unity Defaults
        private void Awake()
        {
            // Gets menu manager
            menuManager = FindObjectOfType<MainMenuManager>(true);
        }


        // Setup
        public void SetupSlot(LevelData newData)
        {
            // Updates stored data
            storedLevelData = newData;

            // Updates UI
            levelNameText.SetText(storedLevelData.displayName);
            if(storedLevelData.completionStatus == Completion.NotPassed)
            {
                levelStatusHighlight.color = Color.red; // TODO: Change this to use a standardized colour palette instead! That way we can do colourblind and dark modes too
            }
            else
            {
                levelStatusHighlight.color = Color.green;
            }
        }


        // UI Events
        public void OpenLevel()
        {
            menuManager.LoadLevel(storedLevelData.levelFileName);
        }
    }
}