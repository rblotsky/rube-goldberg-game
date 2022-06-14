using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RubeGoldbergGame
{ 
    public class LevelTimeObjective : MonoBehaviour
    {
        // DATA //
        // Objective Data
        public int objectiveIndex;
        public float timeLimit;

        // Cached data
        private LevelManager levelManager;
        private float simStartTime;


        // FUNCTIONS //
        // Unity Defaults
        private void Awake()
        {
            levelManager = FindObjectOfType<LevelManager>(true);
        }

        private void Update()
        {
            // If timescale is zero, resets simStartTime to current time.
            if(Time.timeScale == 0)
            {
                simStartTime = Time.time;
            }
        }

        private void OnEnable()
        {
            // Adds itself to the levelManager's level finish event
            levelManager.onLevelFinish += HandleLevelFinish;
        }

        private void OnDisable()
        {
            // Removes itself from the levelManager's level finish event
            levelManager.onLevelFinish -= HandleLevelFinish;
        }


        // Event Handlers
        public void HandleLevelFinish(LevelManager levelManager)
        {
            // Calculates the time taken to finish
            float timeTaken = Time.time - simStartTime;

            // If it's lower than the required time value, updates the objective
            if (timeTaken < timeLimit)
            {
                levelManager.UpdateObjectiveCompletion(objectiveIndex, true);
            }
        }

    }
}