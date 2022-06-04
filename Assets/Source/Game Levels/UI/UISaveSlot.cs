using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

namespace RubeGoldbergGame
{
    public class UISaveSlot : MonoBehaviour
    {
        // DATA //
        // Scene References
        public TextMeshProUGUI saveNameText;

        // Cached Data
        private string saveName;
        private UILevelSavePanel savePanel;


        // FUNCTIONS //
        // External Management
        public void SetupSlot(UILevelSavePanel newSavePanel, string newSaveName)
        {
            savePanel = newSavePanel;
            saveName = newSaveName;
            saveNameText.SetText(newSaveName);
        }


        // UI Events
        public void OnSaveDelete()
        {
            //savePanel.DeleteSave();
        }

        public void OnSaveOpen()
        {
            savePanel.LoadSave(saveName);
        }
    }
}