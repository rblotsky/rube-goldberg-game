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

        // Cached Data
        private List<Button> saveButtons = new List<Button>();
        private LevelData thisLevelData;


        // FUNCTIONS //
        // Unity Defaults
        private void Awake()
        {
            thisLevelData = FindObjectOfType<LevelManager>().levelData;
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
        public void CreateNewSave(string saveName)
        {
            // Gets save folder, creates if nonexistent
            string saveFolderpath = GetSaveFolderPath();

            if(!Directory.Exists(saveFolderpath))
            {
                Directory.CreateDirectory(saveFolderpath);
            }

            // Creates a save file in the directory
            File.Create(Path.Combine(GetSaveFolderPath(), Mathf.RoundToInt(Time.realtimeSinceStartup) + ".txt"));

            // Updates UI to include the new save
            UpdateUI();
        }

        public void LoadSave(string saveName)
        {
            Debug.Log(string.Format("Loading save \"{0}\" (this doesn't actually work yet)", saveName));
        }


        // Internal Functions
        private string GetSaveFolderPath()
        {
            string saveFolderPath = Application.persistentDataPath + "\\" + thisLevelData.name + "\\";
            return saveFolderPath;
        }

        private string[] GetSavePaths()
        {
            // Gets directory path
            string saveFolderPath = GetSaveFolderPath();

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

        private void UpdateUI()
        {
            // Gets save names
            string[] savePaths = GetSavePaths();

            // Creates a save button for each one
            foreach (string path in savePaths)
            {
                // Gets save name
                string saveName = Path.GetFileName(path);

                // Does nothing if there already exists a button for this save
                if (saveButtons.Find(x => x.name.Equals(saveName)) != null)
                {
                    continue; 
                }

                // Otherwise, creates the button
                Button newButton = Instantiate(newSaveButton, newSaveButton.transform.parent).GetComponent<Button>();
                newButton.onClick.RemoveAllListeners();
                newButton.GetComponentInChildren<TextMeshProUGUI>().SetText(saveName);
                newButton.name = saveName;
                newButton.onClick.AddListener(() => { LoadSave(path); });
                saveButtons.Add(newButton);
            }
        }

        private void DeleteUI()
        {
            // Destroys gameobjects for all the buttons
            foreach(Button button in saveButtons)
            {
                Destroy(button);
            }

            // Clears the buttons list
            saveButtons.Clear();
        }
    }

}