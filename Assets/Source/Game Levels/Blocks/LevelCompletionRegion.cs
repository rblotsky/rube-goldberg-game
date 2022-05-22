using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RubeGoldbergGame
{
    public class LevelCompletionRegion : MonoBehaviour
    {
        // DATA //
        // References
        public MovableObject requiredMovableObject;
        public TextMeshProUGUI numberRequirementText;

        // Usage Data
        public Completion completionUsed;

        // Cached Data
        private LevelManager levelManager;


        // FUNCTIONS //
        // Unity Defaults
        private void Awake()
        {
            // Finds references
            levelManager = FindObjectOfType<LevelManager>();
            numberRequirementText = GetComponentInChildren<TextMeshProUGUI>();

            // If there is a required object and number text, sets it to show the name of the object that has to go there
            if(requiredMovableObject != null && numberRequirementText != null)
            {
                numberRequirementText.SetText(requiredMovableObject.name);
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            // If the collided object is the required one, completes the level
            MovableObject collidedObject = col.GetComponent<MovableObject>();
            if(collidedObject != null)
            {
                // If it's the required one, or there is no required one, runs completelevel with the completion given
                if (collidedObject == requiredMovableObject || requiredMovableObject == null)
                {
                    levelManager.CompleteLevel(completionUsed);
                }
            }
        }
    }
}