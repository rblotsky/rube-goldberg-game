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
            interfaceManager = FindObjectOfType<LevelUIManager>(true);
        }

        protected virtual void Update()
        {
            if (isUserHovering)
            {
                interfaceManager.OpenTooltipUI(displayName + "\n\n" + displayDescription, Input.mousePosition);
            }
        }

        protected virtual void OnDestroy()
        {
            if(isUserHovering)
            {
                interfaceManager.CloseTooltipUI();
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
