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


        // Interface Functions
        public void OnPointerEnter(PointerEventData pointerData)
        {
            interfaceManager.OpenTooltipUI(displayName + "\n\n" + displayDescription, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            isUserHovering = true;
            Debug.Log("AHAHA");
        }

        public void OnPointerExit(PointerEventData pointerData)
        {
            interfaceManager.CloseTooltipUI();
            isUserHovering = false;
        }
    }
}