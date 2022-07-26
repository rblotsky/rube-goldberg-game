using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RubeGoldbergGame
{
    public class LevelObjectiveComponent : MonoBehaviour
    {
        // DATA //
        // Objective Data
        public int objectiveIndex;

        // Cached Data
        protected LevelManager levelManager;


        // FUNCTIONS //
        // Unity Defaults
        protected virtual void Awake()
        {
            levelManager = FindObjectOfType<LevelManager>(true);
        }


        // Objective Completion Functions
        public void CompleteObjective()
        {
            levelManager.UpdateObjectiveCompletion(objectiveIndex, true);
        }
    }
}