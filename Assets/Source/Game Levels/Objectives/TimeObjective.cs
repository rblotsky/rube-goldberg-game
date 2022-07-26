using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RubeGoldbergGame
{
    public class TimeObjective : LevelObjectiveComponent
    {
        // DATA //
        // Objective Data
        public float maxTimeTakenSeconds = 10;

        // Cached Data
        private float simStartTime = 0;


        // FUNCTIONS //
        // Unity Defaults
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


        // Event Handlers
        public void HandleSimulationStart(LevelManager manager)
        {
            simStartTime = 0;
        }

        public void HandleLevelFinish(LevelManager manager)
        {
            // If it took less than the max time, completes the objective
            if(Time.time - simStartTime < maxTimeTakenSeconds)
            {
                CompleteObjective();
            }            
        }

    }
}