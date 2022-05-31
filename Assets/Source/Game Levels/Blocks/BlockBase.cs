using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RubeGoldbergGame 
{
    [DisallowMultipleComponent]
    public class BlockBase : TooltipComponent, IPointerClickHandler
    {
        // DATA //
        // Description Data
        public string displayName;
        public string displayDescription;
        public Sprite displaySprite;
        public MonoBehaviour attachedProperties;



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
        
        public void OnPointerClick(PointerEventData pointerData)
        {
            // Updates the selected button
            Debug.Log("Gameobject has been clicked");
            var parentManager = FindObjectOfType<EditorBlockPlacingManager>();
            parentManager.DoPlacementAction();  
            
        }

    }
}
