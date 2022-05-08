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
        private Vector3 initialPosition;
        private Rigidbody2D objRigidbody;


        // FUNCTIONS //
        // Unity defaults
        private void Awake()
        {
            // Finds some other objects/components
            interfaceManager = FindObjectOfType<LevelUIManager>();
            objRigidbody = GetComponent<Rigidbody2D>();

            // Sets initial position to the instantiation position
            initialPosition = transform.position;
        }

        private void Update()
        {
            if(isUserHovering)
            {
                interfaceManager.OpenTooltipUI(displayName + "\n\n" + displayDescription, Input.mousePosition);
            }
        }


        // Object Management
        public void ResetToInitialValues()
        {
            transform.position = initialPosition;
            objRigidbody.velocity = Vector3.zero;
            objRigidbody.angularVelocity = 0;
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