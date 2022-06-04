using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;
using System.Linq;

namespace RubeGoldbergGame
{
    public class UILevelSavePanel : MonoBehaviour
    {
        // DATA //
        // Scene References
        public GameObject newSaveButton;
        public GameObject saveSlotPrefab;
        public GameObject scrollContent;

        // Cached Data
        private List<UISaveSlot> saveSlots = new List<UISaveSlot>();
        private LevelData thisLevelData;
        private LevelUIManager interfaceManager;


        // FUNCTIONS //
        // Unity Defaults
        private void Awake()
        {
            thisLevelData = FindObjectOfType<LevelManager>(true).levelData;
            interfaceManager = FindObjectOfType<LevelUIManager>(true);
        }

        private void OnEnable()
        {
            UpdateUI();
        }

        private void OnDisable()
        {
            DeleteUI();
        }


        // External Management
        public void PromptDeleteSave(string saveName)
        {
            // Prompts the user to delete the save
            interfaceManager.OpenConfirmationPanel(string.Format("Are you sure you want to delete \"{0}\"?", saveName), saveName, ConfirmedSaveDeletion);
        }

        public void ConfirmedSaveDeletion(object saveToDelete, bool isConfirmed)
        {
            // Assumes the saveToDelete is a string, then tries deleting that save.
            GlobalData.DeleteLevelSave(thisLevelData.name, (string)saveToDelete);

            // Updates UI
            UpdateUI();
        }

        public void CreateNewSave(string saveName)
        {
            // Creates a new level save
            GlobalData.CreateNewLevelSave(thisLevelData.name, saveName);

            // Updates UI to include the new save
            UpdateUI();
        }

        public void LoadSave(string saveName)
        {
            Debug.Log(string.Format("Loading save \"{0}\" (this doesn't actually work yet)", saveName));
        }


        // Internal Functions
        private string[] GetSavePaths()
        {
            // Gets directory path
            string saveFolderPath = GlobalData.GetLevelFolderPath(thisLevelData.name);

            // Looks for a folder under the right name and retrieves all the available saves
            if (Directory.Exists(saveFolderPath))
            {
                return Directory.GetFiles(saveFolderPath);
            }

            else
            {
                return new string[0];
            }
        }


        // UI Management
        private void UpdateUI()
        {
            // Deletes exising UI
            DeleteUI();

            // Opens new UI
            // Gets save names
            string[] savePaths = GetSavePaths();

            // Creates a save button for each one
            foreach (string path in savePaths)
            {
                // Gets save name
                string saveName = Path.GetFileName(path);

                // Does nothing if there already exists a button for this save
                if (saveSlots.Find(x => x.name.Equals(saveName)) != null)
                {
                    continue; 
                }

                // Otherwise, creates the button
                UISaveSlot newSlot = Instantiate(saveSlotPrefab, scrollContent.transform).GetComponent<UISaveSlot>();
                newSlot.name = saveName;
                newSlot.SetupSlot(this, saveName);
                saveSlots.Add(newSlot);
            }
        }

        private void DeleteUI()
        {
            // Destroys gameobjects for all the buttons
            foreach(UISaveSlot slot in saveSlots)
            {
                Destroy(slot.gameObject);
            }

            // Clears the buttons list
            saveSlots.Clear();
        }
    }

}