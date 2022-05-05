using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RubeGoldbergGame
{
    public class DestinationRegion : BlockBase
    {
        // DATA //
        private void OnTriggerEnter2D(Collider2D col)
        {
            TriggerBlockFunctionality();
        }

        //overridden function
        public void TriggerBlockFunctionality()
        {
            Debug.Log("This should be seen once the objective is in my area!");
        }
    }
}