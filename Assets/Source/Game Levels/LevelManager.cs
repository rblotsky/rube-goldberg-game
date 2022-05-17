using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RubeGoldbergGame
{
    public class LevelManager : MonoBehaviour
    {
        // DATA //
        // Scene References
        public LevelUIManager interfaceManager;
        public MovableObject objectiveObject;
        public PlacingHologram placementHologram;

        // Simulation Management
        private static readonly int[] simSpeedPercentages = { 0, 25, 50, 100, 200, 300, 400 };

        // Level Management
        public int levelID;
        public int blockPlaceRotationAmount = 15;
        public BlockBase[] availableBlocks;
        public LevelData levelData;
        public List<BlockBase> placedBlocks = new List<BlockBase>();

        // Cached Data

        private int numBlocksUsed;
        private int currentSimSpeedIndex = 3;
        public bool inSimulation;
        private Camera mainCam;


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

            // Updates number of blocks used
            numBlocksUsed = 0;
        }

        private void Start()
        {
            // Updates UI to have the required block slots
            interfaceManager.placeablesButtons.GenerateBlockSlots(availableBlocks);

            // Enters editor mode
            ToggleSimulationMode(false);
        }


        // Level Events
        public void CompleteLevel()
        {
            interfaceManager.ToggleCompletionUI(true);
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
            // Resets all the placed objects to correct positions
            objectiveObject.ResetToInitialValues();
            TogglePlaceableObjects(inSimMode);

            // Toggles UI
            interfaceManager.ToggleSimulationUI(inSimMode);
            interfaceManager.UpdateSimSpeedText(simSpeedPercentages[currentSimSpeedIndex]);

            // Updates cached status
            inSimulation = inSimMode;

            // Refreshes timescale
            RefreshTimescale();
        }

        private void TogglePlaceableObjects(bool inSimMode)
        {
            foreach (BlockBase block in placedBlocks)
            {
                block.ToggleTriggerArea(inSimMode);
            }
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

    }
}