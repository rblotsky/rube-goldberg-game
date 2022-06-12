using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RubeGoldbergGame
{
    public class LevelFailureRegion : MonoBehaviour
    {
        // DATA //
        // Cached Data
        private LevelManager levelManager;


        // FUNCTIONS //
        // Unity Defaults
        private void Awake()
        {
            // Finds references
            levelManager = FindObjectOfType<LevelManager>(true);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            // If the collided object is an objective object, fails the level
            MovableObject collidedObject = col.GetComponent<MovableObject>();
            if (collidedObject != null && collidedObject.isObjectiveObject)
            {
                levelManager.FinishLevel(Completion.NotPassed);
            }
        }
    }
}