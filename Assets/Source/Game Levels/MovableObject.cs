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

        // FUNCTIONS //
        // Unity defaults
        private void Awake()
        {
            interfaceManager = FindObjectOfType<LevelUIManager>();
        }

        private void OnMouseEnter()
        {
            //Debug.Log("I am hovered over!");
            FindObjectOfType<LevelUIManager>().ToggleTooltipUI(displayName +": "+ displayDescription); //TODO replace this with a better method
        }

        private void OnMouseExit()
        {
            //Debug.Log("I am not hovered over anymore");
            FindObjectOfType<LevelUIManager>().ToggleTooltipUI("");
        }


        // Interface Functions
        public void OnPointerEnter(PointerEventData pointerData)
        {
            //TODO
        }

        public void OnPointerExit(PointerEventData pointerData)
        {
            //TODO
        }
    }
}