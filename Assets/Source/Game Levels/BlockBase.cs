using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RubeGoldbergGame 
{
    public class BlockBase : MonoBehaviour
    {
        // DATA //
        // Description Data
        public string displayName;
        public string displayDescription;
        public Sprite displaySprite;

        // Cached Data
        private ActiveObjective objectiveItem;


        // FUNCTIONS //
        // Virtual Functions
        public virtual void TriggerBlockFunctionality()
        {
            Debug.Log("This block has no functionality.");
        }
    }
}
