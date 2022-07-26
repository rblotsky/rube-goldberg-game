using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RubeGoldbergGame
{
    public class BlocksPlacedObjective : LevelObjectiveComponent
    {
        // DATA //
        // Objective Data
        public float maxBlocksUsed = 10;


        // FUNCTIONS //
        // Unity Defaults
        private void OnEnable()
        {
            levelManager.onLevelFinish += HandleLevelFinish;
        }

        private void OnDisable()
        {
            levelManager.onLevelFinish -= HandleLevelFinish;
        }


        // Event Handlers

        public void HandleLevelFinish(LevelManager manager)
        {
            // If less than the max amount of blocks were used, completes the objective
            if (levelManager.blockPlacingManager.BlocksUsed <= maxBlocksUsed)
            {
                CompleteObjective();
            }
        }
    }
}