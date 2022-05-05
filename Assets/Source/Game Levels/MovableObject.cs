using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RubeGoldbergGame
{
    public class MovableObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        // DATA //
        // Toggles
        public bool isObjectiveObject = false;
        public string displayName;
        public string displayDescription;
        public Sprite displaySprite; // Not necessary: This is used in BlockBase so we can display it in the editor, but I don't think players will be placing MoveableObjects from the editor.

        // Cached data
        private LevelUIManager interfaceManager;
        private bool isUserHovering = false;


        // FUNCTIONS //
        // Unity defaults
        private void Awake()
        {
            interfaceManager = FindObjectOfType<LevelUIManager>();
        }

        private void LateUpdate()
        {
            if(isUserHovering)
            {
                interfaceManager.OpenTooltipUI(displayName + "\n\n" + displayDescription, Input.mousePosition);
            }
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