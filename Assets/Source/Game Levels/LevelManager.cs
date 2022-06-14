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

        // Events
        public event LevelManagerDelegate onLevelFinish;

        // Level Management
        public BlockBase[] availableBlocks;
        public LevelData levelData;

        // Cached Data
        private int currentSimSpeedIndex = 3;
        public bool inSimulation;
        private float simulationStartTime;
        private bool[] levelObjectivesComplete = { false, false, false };


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
        public void FinishLevel(Completion completionType)
        {
            // If the completion UI is already open (already completed), does nothing
            if(interfaceManager.completionUI.isActiveAndEnabled)
            {
                return;
            }

            // Runs the onFinish event
            if(onLevelFinish != null)
            {
                onLevelFinish(this);
            }

            // If this is the first successful completion, saves it as the first success
            if (completionType == Completion.Passed && levelData.completionStatus == Completion.NotPassed)
            {
                FindObjectOfType<UILevelSavePanel>(true).AttemptCreateNewSave("First Success");
            }

            // If the level was won, updates the level bests
            if (completionType == Completion.Passed)
            {
                levelData.UpdateLevelBests(completionType, levelObjectivesComplete);
            }

            // Runs the time slow effect
            if (slowScript.enabled == false)
            {
                slowScript.StartTimeSlow(simSpeedPercentages[currentSimSpeedIndex]);
            }

            // Toggles UI
            interfaceManager.ToggleCompletionUI(true);
            interfaceManager.completionUI.UpdateContent(completionType, levelData);
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

        public void UpdateObjectiveCompletion(int objectiveIndex, bool objectiveStatus)
        {
            levelObjectivesComplete[objectiveIndex] = objectiveStatus;
        }

        public void ResetObjectiveCompletions()
        {
            for(int i = 0; i < levelObjectivesComplete.Length; i++)
            {
                UpdateObjectiveCompletion(i, false);
            }
        }


        // UI Events
        public void ToggleSimulationMode(bool inSimMode)
        {
            // Resets the cached objective completion
            ResetObjectiveCompletions();
            
            blockPlacingManager.resetPositionOfBlocks();

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