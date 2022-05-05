using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RubeGoldbergGame
{
    public class DestinationRegion : BlockBase
    {
        // DATA //
        
        //overridden function
        public void TriggerBlockFunctionality()
        {
            Debug.Log("This should be seen once the objective is in my area!");
        }
    }
}