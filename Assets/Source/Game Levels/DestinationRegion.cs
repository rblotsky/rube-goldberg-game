using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RubeGoldbergGame
{
    public class DestinationRegion : BlockBase
    {
        // DATA //
        // Cached Data
        private LevelManager levelManager;


        // FUNCTIONS //
        // Unity Defaults
        protected override void Awake()
        {
            base.Awake();
            levelManager = FindObjectOfType<LevelManager>();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            TriggerBlockFunctionality();
        }


        // Override Functions
        public override void TriggerBlockFunctionality()
        {
            // Runs level completion
            levelManager.CompleteLevel();
        }
    }
}