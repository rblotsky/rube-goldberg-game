using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RubeGoldbergGame
{
    public class LevelManager : MonoBehaviour
    {
        // DATA //
        // UI Management
        public LevelUIManager interfaceManager;
        public MovableObject objectiveObject;
        public Transform initialObjectivePosition;

        // Simulation Management
        private static int[] simSpeedPercentages = { 0, 25, 50, 100, 200, 300, 400 };

        // Level Management
        public BlockBase[] availableBlocks;
        //public LevelID levelData;

        // Cached Data
        private int numBlocksUsed;
        private int currentSimSpeedIndex = 3;
        private bool inSimulation;


        // FUNCTIONS //
        // Unity Defaults
        private void Awake()
        {
            // Enters editor mode
            ToggleSimulationMode(false);

            // Updates initial position of objective
            objectiveObject.transform.position = initialObjectivePosition.position;

            // Updates number of blocks used
            numBlocksUsed = 0;
        }


        // Level Events
        public void CompleteLevel()
        {
            Debug.Log("You have won the level!");
        }


        // UI Events
        public void ToggleSimulationMode(bool inSimMode)
        {
            // Resets all the placed objects
            //TODO
            objectiveObject.transform.position = initialObjectivePosition.position;

            // Toggles UI
            interfaceManager.ToggleSimulationUI(inSimMode);

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


    }
}