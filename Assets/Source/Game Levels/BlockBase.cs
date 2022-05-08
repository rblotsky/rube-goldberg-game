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

        // Cached Data (Protected so it's accessible in subclasses)
        protected ActiveObjective objectiveItem;
        protected LevelUIManager interfaceManager;
        protected bool isUserHovering;


        // FUNCTIONS //
        // Unity Defaults
        protected virtual void Awake()
        {
            // NOTE: In inherited classes, PLEASE use base.Awake() to run this code before whatever else is added.
            interfaceManager = FindObjectOfType<LevelUIManager>(true);
        }

        protected virtual void Update()
        {
            if (isUserHovering)
            {
                if(interfaceManager == null)
                {
                    Debug.Log("INTERFACE IS NULL");
                }    
                interfaceManager.OpenTooltipUI(displayName + "\n\n" + displayDescription, Input.mousePosition);
            }
        }


        // Virtual Functions
        public virtual void TriggerBlockFunctionality() 
        {
            Debug.Log("This block has no functionality.");
        }

        public virtual void ToggleTriggerArea(bool inSimMode)
        {
            //TODO
        }


        // Interface Functions
        public void OnPointerEnter(PointerEventData pointerData)
        {
            isUserHovering = true;
        }

        public void OnPointerExit(PointerEventData pointerData)
        {
            interfaceManager.CloseTooltipUI();
            isUserHovering = false;
        }
    }
}
