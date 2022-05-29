using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RubeGoldbergGame
{
    public class MainMenuManager : MonoBehaviour
    {
        // DATA //
        // References
        public GameObject mainMenuPanel;
        public GameObject levelSelectPanel;
        public UILevelSlot defaultLevelSlot;
        public LevelData firstLevel;
        
        // Constants
        public static readonly string MENU_SCENE_NAME = "MainMenuScene";


        // FUNCTIONS //
        // Unity Defaults
        private void Awake()
        {
            // Sets up the game immediately
            GlobalData.SetupGame();

            // Spawns all the level slots
            LevelData currentLevel = firstLevel;
            defaultLevelSlot.SetupSlot(currentLevel);
            currentLevel = currentLevel.nextLevel;
            while (currentLevel != null)
            {
                UILevelSlot newLevelSlot = Instantiate(defaultLevelSlot.gameObject, defaultLevelSlot.transform.parent).GetComponent<UILevelSlot>();
                newLevelSlot.SetupSlot(currentLevel);
                currentLevel = currentLevel.nextLevel;
            }

            // Closes level UI
            ToggleLevelUI(false);
        }


        // UI Management
        public void ToggleLevelUI(bool isOpen)
        {
            // Opens the panel
            levelSelectPanel.gameObject.SetActive(isOpen);
            mainMenuPanel.gameObject.SetActive(!isOpen);
        }


        // UI Events
        public void LoadLevel(string levelName)
        {
            SceneManager.LoadScene(levelName, LoadSceneMode.Single);
        }
    }
}