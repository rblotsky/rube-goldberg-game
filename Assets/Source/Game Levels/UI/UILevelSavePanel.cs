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
        public TMP_InputField nameInputField;

        // Cached Data
        private List<UISaveSlot> saveSlots = new List<UISaveSlot>();
        private LevelData thisLevelData;
        private LevelUIManager interfaceManager;
        private EditorBlockPlacingManager blockManager;


        // FUNCTIONS //
        // Unity Defaults
        private void OnEnable()
        {
            UpdateUI();
        }

        private void OnDisable()
        {
            DeleteUI();
        }


        // External Management
        public void SetupSavePanel()
        {
            blockManager = FindObjectOfType<EditorBlockPlacingManager>(true);
            thisLevelData = FindObjectOfType<LevelManager>(true).levelData;
            interfaceManager = FindObjectOfType<LevelUIManager>(true);
        }

        public void PromptDeleteSave(string saveName)
        {
            // Prompts the user to delete the save
            interfaceManager.OpenConfirmationPanel(string.Format("Are you sure you want to delete save \"{0}\"?", saveName), saveName, ConfirmedSaveDeletion);
        }

        public void ConfirmedSaveDeletion(object saveNameObject, bool isConfirmed)
        {
            // Does nothing if not confirmed
            if(!isConfirmed)
            {
                return;
            }

            // Assumes the saveToDelete is a string, then tries deleting that save.
            GlobalData.DeleteLevelSave(thisLevelData.name, (string)saveNameObject);

            // Updates UI
            UpdateUI();
        }

        public void ConfirmedCreateNewSave(object saveNameObject, bool isConfirmed)
        {
            // If not confirmed, does nothing
            if(!isConfirmed)
            {
                return;
            }

            // saveName should be a string, so converts it
            string saveName = (string)saveNameObject;

            // Creates a new level save
            blockManager.SaveAllBlocks(saveName);

            // Updates UI to include the new save
            UpdateUI();
        }

        public void LoadSave(string saveName)
        {
            // Tells the block placing manager to load all the blocks individually
            blockManager.LoadAllBlocks(saveName);

            // Updates UI
            UpdateUI();
        }

        public void AttemptCreateNewSave(string saveName)
        {
            // If one with this name already exists, prompts user to delete it
            if (GetSaveNames().Contains(saveName))
            {
                interfaceManager.OpenConfirmationPanel(string.Format("A save named \"{0}\" already exists. Overwrite it?", saveName), saveName, ConfirmedCreateNewSave);
            }

            else
            {
                // Otherwise, jumps straight to creating a new save
                ConfirmedCreateNewSave(saveName, true);
            }
        }


        // UI Events
        public void InputFieldValueChange()
        {
            // Sets it to "New Save" if it's empty
            string inputFieldValue = nameInputField.text;
            if(inputFieldValue.Trim().Length < 1)
            {
                nameInputField.text = "New Save";
            }
        }

        public void CreateNewSaveFromUserInput()
        {
            // Gets the name to use
            string saveName = nameInputField.text;

            // Creates the save
            AttemptCreateNewSave(saveName);
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

        private string[] GetSaveNames()
        {
            // Gets all save paths
            string[] savePaths = GetSavePaths();

            // Culls everything except the names
            for(int i = 0; i < savePaths.Length; i++)
            {
                savePaths[i] = Path.GetFileNameWithoutExtension(savePaths[i]);
            }

            // Returns the paths array, now containing only names
            return savePaths;
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
                string saveName = Path.GetFileNameWithoutExtension(path);

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