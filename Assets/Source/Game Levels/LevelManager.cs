using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RuthGoldbergGame
{
    public class LevelManager : MonoBehaviour
    {
        // DATA //
        // UI Management
        public LevelUIManager interfaceManager;


        // Simulation Management


        // Level Management
        public BlockBase[] availableBlocks;
        //public LevelID levelData;


        // Cached Data
        private int numBlocksUsed;


        // FUNCTIONS //
        // Unity Defaults
        private void Awake()
        {
            //TODO: Enter into editor mode, ensure everything is set up properly.
        }


        // Level Events
        public void CompleteLevel()
        {
            Debug.Log("You have won the level!");
        }


        // UI Events
        public void ToggleSimulationMode(bool inSimMode)
        {

        }

        public void UpdateSimSpeed()
        {

        }


    }
}