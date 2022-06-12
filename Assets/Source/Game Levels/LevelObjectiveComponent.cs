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
        private LevelManager levelManager;


        // FUNCTIONS //
        // Unity Defaults
        

        // Objective Completion Functions
        public void CompleteObjective()
        {
            //TODO: Figure out how to have this track different potential objectives.
            levelManager.UpdateObjectiveCompletion(objectiveIndex, true);
        }
    }
}