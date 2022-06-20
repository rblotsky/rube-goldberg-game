using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RubeGoldbergGame
{
    public class UIDragBar : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        // DATA //
        // Scene References
        public RectTransform translatedObject;

        // Cached Data
        private Vector3 mouseOffset;
        private bool isMoving;
        private Canvas uiCanvas;


        // FUNCTIONS //
        // Unity Defaults
        private void OnEnable()
        {
            uiCanvas = GetComponentInParent<Canvas>();   
        }

        private void Update()
        {
            // If the object is being moved, moves it to a new position according to the mouse offset
            if(isMoving)
            {
                Debug.Log("Moving");
                translatedObject.transform.position = UtilityFuncs.ClampElementToCanvas(translatedObject, uiCanvas, Input.mousePosition + mouseOffset);
            }
        }


        // Interface Functions
        public void OnPointerDown(PointerEventData pointerData)
        {
            Debug.Log("Pressed Down");
            mouseOffset = translatedObject.position - Input.mousePosition;
            isMoving = true;
        }

        public void OnPointerUp(PointerEventData pointerData)
        {
            Debug.Log("Pressed Up");
            mouseOffset = Vector3.zero;
            isMoving = false;
        }
    }
}