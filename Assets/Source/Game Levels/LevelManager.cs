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
        private LevelUIManager interfaceManager;
        private MovableObject objectiveObject;
        private PlacingHologram placementHologram;
        private EditorBlockPlacingManager blockPlacingManager;
        private LinearSlowTimeframe slowScript;
        private Camera mainCam;

        // Simulation Management
        private static readonly int[] simSpeedPercentages = { 0, 25, 50, 100, 200, 300, 400 };

        // Level Management
        public BlockBase[] availableBlocks;
        public LevelData levelData;

        // Cached Data
        private int currentSimSpeedIndex = 3;
        public bool inSimulation;
        private float simulationStartTime;


        // FUNCTIONS //
        // Unity Defaults
        private void Awake()
        {
            // Gets level references
            interfaceManager = FindObjectOfType<LevelUIManager>(true);
            blockPlacingManager = FindObjectOfType<EditorBlockPlacingManager>(true);
            slowScript = FindObjectOfType<LinearSlowTimeframe>(true);
            placementHologram = FindObjectOfType<PlacingHologram>(true);
            mainCam = Camera.main;
            foreach (MovableObject movableObject in FindObjectsOfType<MovableObject>(true))
            {
                if(movableObject.isObjectiveObject)
                {
                    objectiveObject = movableObject;
                    break;
                }
            }


            // Sets base UI according to level data
            interfaceManager.SetBasicInterface(levelData);
        }

        private void Start()
        {
            // Updates UI to have the required block slots
            interfaceManager.blockSlotManager.GenerateBlockSlots(availableBlocks);

            // Enters editor mode
            ToggleSimulationMode(false);
        }

        // Level Events
        public void CompleteLevel(Completion completionType)
        {
            // Gets blocks used and time taken
            float timeTaken = Time.time - simulationStartTime;
            int blocksUsed = blockPlacingManager.BlocksUsed;

            // If the completion UI is already open, does nothing
            if(interfaceManager.completionUI.isActiveAndEnabled)
            {
                return;
            }

            // If the level was won, updates the level bests
            if (completionType == Completion.Passed)
            {
                levelData.UpdateLevelBests(timeTaken, blocksUsed);
            }

            // Runs the time slow effect
            if (slowScript.enabled == false)
            {
                slowScript.enabled = true;
                slowScript.StartTimeSlow(simSpeedPercentages[currentSimSpeedIndex]);
            }
            
            // If the level has just now been completed, saves it as the first success
            if(levelData.completionStatus == Completion.NotPassed)
            {
                FindObjectOfType<UILevelSavePanel>(true).AttemptCreateNewSave("First Success");
            }

            // Toggles UI
            interfaceManager.ToggleCompletionUI(true);
            interfaceManager.completionUI.UpdateContent(completionType, timeTaken, blocksUsed, levelData);
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
                slowScript.enabled = false;
            }

            // Resets all the placed objects to correct positions
            objectiveObject.ResetToInitialValues();

            // Toggles UI
            interfaceManager.ToggleSimulationUI(inSimMode);
            interfaceManager.UpdateSimSpeedText(simSpeedPercentages[currentSimSpeedIndex]);
            interfaceManager.CloseTooltipUI();

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