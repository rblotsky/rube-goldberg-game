using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RubeGoldbergGame
{
    public class MinimalCollisionsObjective : LevelObjectiveComponent
    {
        // DATA //
        public float maxObjectiveCollisions;

        // Cached Data
        private float numObjectiveCollisions = 0;


        // FUNCTIONS //
        private void OnEnable()
        {
            levelManager.onSimulationStart += HandleSimulationStart;
            levelManager.onLevelFinish += HandleLevelFinish; 
        }

        private void OnDisable()
        {
            levelManager.onSimulationStart -= HandleSimulationStart;
            levelManager.onLevelFinish -= HandleLevelFinish;
        }


        // Event Handling
        public void HandleObjectiveCollision()
        {
            numObjectiveCollisions++;
        }

        public void HandleSimulationStart(LevelManager managerUsed)
        {
            // Resets num collisions tracker
            numObjectiveCollisions = 0;
        }

        public void HandleLevelFinish(LevelManager managerUsed)
        {
            // Completes the objective if the number of collisions was less than max
            if (numObjectiveCollisions < maxObjectiveCollisions)
            {
                CompleteObjective();
            }
        }        
    }
}