using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace RubeGoldbergGame
{
    public class LevelManager : MonoBehaviour
    {
        // DATA //
        // Scene References
        public LevelUIManager interfaceManager;
        public MovableObject objectiveObject;
        public PlacingHologram placementHologram;
        public EditorBlockPlacingManager blockPlacingManager;

        // Simulation Management
        private static readonly int[] simSpeedPercentages = { 0, 25, 50, 100, 200, 300, 400 };

        // Level Management
        public int levelID;
        public BlockBase[] availableBlocks;
        public LevelData levelData;

        // Cached Data
        private int currentSimSpeedIndex = 3;
        public bool inSimulation;
        private Camera mainCam;
        private float simulationStartTime;


        // FUNCTIONS //
        // Unity Defaults
        private void Awake()
        {
            // Gets level references
            interfaceManager = FindObjectOfType<LevelUIManager>(true);
            mainCam = Camera.main;

            // Gets level data
            levelData = GlobalData.GetLevel(levelID);

            // Sets base UI according to level data
            interfaceManager.SetBasicInterface(levelData);
        }

        private void Start()
        {
            // Updates UI to have the required block slots
            interfaceManager.placeablesButtons.GenerateBlockSlots(availableBlocks);

            // Enters editor mode
            ToggleSimulationMode(false);
        }


        // Level Events
        public void CompleteLevel(Completion completionType)
        {
            interfaceManager.ToggleCompletionUI(true);
            interfaceManager.UpdateCompletionUIContent(completionType);

            // If the level was won, updates the level bests
            if(completionType == Completion.Passed)
            {
                levelData.UpdateLevelBests(Time.time-simulationStartTime, blockPlacingManager.BlocksUsed);
            }
        }
        
        public void RefreshTimescale()
        {
            // Updates timescale depending on whether the game is in simulation or editor mode
            if (inSimulation)
            {
                Time.timeScale = 1f * (simSpeedPercentages[currentSimSpeedIndex] / 100f);
            }

            else
            {
                Time.timeScale = 0;
            }
        }


        // UI Events
        public void ToggleSimulationMode(bool inSimMode)
        {
            // If in simulation mode, tracks the start time
            if(inSimMode)
            {
                simulationStartTime = Time.time;
            }

            // Resets all the placed objects to correct positions
            objectiveObject.ResetToInitialValues();

            // Toggles UI
            interfaceManager.ToggleSimulationUI(inSimMode);
            interfaceManager.UpdateSimSpeedText(simSpeedPercentages[currentSimSpeedIndex]);

            // Updates cached status
            inSimulation = inSimMode;

            // Refreshes timescale
            RefreshTimescale();
        }

        public void UpdateSimSpeedIndex(int changeAmount)
        {
            // Updates which simulation speed is being used
            currentSimSpeedIndex = Mathf.Clamp(currentSimSpeedIndex + changeAmount, 0, simSpeedPercentages.Length - 1);

            // Updates UI text
            interfaceManager.UpdateSimSpeedText(simSpeedPercentages[currentSimSpeedIndex]);

            // Refreshes timescale
            RefreshTimescale();
        }

        public void GoToNextLevel()
        {
            // Saves this level (by saving the entire game)
            GlobalData.SaveGameData();

            // Goes to next level if there is one
            if (levelData != null && levelData.nextLevel != null)
            {
                SceneManager.LoadScene(levelData.nextLevel.levelFileName, LoadSceneMode.Single);
            }
            else
            {
                Debug.Log("There is no next level!");
            }
        }

        public void ReturnToMenu()
        {
            // Saves this level (by saving the entire game), then goes to menu
            GlobalData.SaveGameData();
            SceneManager.LoadScene(MainMenuManager.MENU_SCENE_NAME, LoadSceneMode.Single);
        }

    }
}