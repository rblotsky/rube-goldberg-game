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
        public GameObject[] uiPanels;
        public GameObject mainUIPanel;
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

            // Sets up settings panel - this also updates the game settings, eg. fullscreen, quality, language, etc.
            FindObjectOfType<UISettingsPanel>(true).SetupSettingsPanel();

            // Translates static UI
            FindObjectOfType<StaticUITranslator>(true).TranslateUI();

            // Focuses the main panel
            FocusUIPanel(mainUIPanel);        
        }

        private void Start()
        {
            // Spawns all the level slots in the level select
            LevelData currentLevel = firstLevel;
            defaultLevelSlot.SetupSlot(currentLevel);
            currentLevel = currentLevel.nextLevel;
            while (currentLevel != null)
            {
                UILevelSlot newLevelSlot = Instantiate(defaultLevelSlot.gameObject, defaultLevelSlot.transform.parent).GetComponent<UILevelSlot>();
                newLevelSlot.SetupSlot(currentLevel);
                currentLevel = currentLevel.nextLevel;
            }
        }


        // UI Management
        public void FocusUIPanel(GameObject panelToFocus)
        {
            // Disables all UI panels
            foreach(GameObject panel in uiPanels)
            {
                panel.SetActive(false);
            }

            // Enables the focused panel
            panelToFocus.SetActive(true);
        }


        // UI Events
        public void LoadLevel(string levelName)
        {
            SceneManager.LoadScene(levelName);
        }

        public void ExitGame()
        {
            // Saves the game
            GlobalData.SaveGameData();

            // Quits the game
            Application.Quit();
        }
    }
}