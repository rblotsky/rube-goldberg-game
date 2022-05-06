using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RubeGoldbergGame 
{
    public class BlockBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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


        // Interface Functions
        public void OnPointerEnter(PointerEventData pointerData)
        {
            //TODO
        }
        
        public virtual void ToggleTriggerArea(bool inSimMode)
        {
        }

        public void OnPointerExit(PointerEventData pointerData)
        {
            //TODO
        }
    }
}
