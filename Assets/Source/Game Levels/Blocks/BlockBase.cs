using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RubeGoldbergGame 
{
    [DisallowMultipleComponent]
    public class BlockBase : TooltipComponent
    {
        // DATA //
        // Description Data
        public string displayName;
        public string displayDescription;
        public Sprite displaySprite;


        // FUNCTIONS //
        // Virtual Functions
        public virtual void TriggerBlockFunctionality() 
        {
            Debug.Log("This block has no functionality.");
        }

        public virtual void ToggleTriggerArea(bool inSimMode)
        {
            //TODO
        }


        // Override Functions
        protected override string GetTooltipText()
        {
            return displayName + "\n" + displayDescription;
        }
    }
}
