using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RubeGoldbergGame
{
    public class LevelCompletionRegion : MonoBehaviour
    {
        // DATA //
        // Usage Data
        public Completion completionUsed;

        // Cached Data
        private LevelManager levelManager;


        // FUNCTIONS //
        // Unity Defaults
        private void Awake()
        {
            levelManager = FindObjectOfType<LevelManager>();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            levelManager.CompleteLevel(completionUsed);
        }
    }
}