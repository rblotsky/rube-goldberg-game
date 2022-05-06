using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RubeGoldbergGame
{
    public class ObjectPusher : BlockBase, IPointerEnterHandler, IPointerExitHandler
    {
        //this is a test object
        public BoxCollider2D triggerArea;
        private LevelUIManager interfaceManager;
        private bool isUserHovering = false;
        
        private void Awake()
        {
            interfaceManager = FindObjectOfType<LevelUIManager>();
        }
        private void OnTriggerStay2D(Collider2D col)
        {
            TriggerBlockFunctionality(col);
        }

        public override void ToggleTriggerArea(bool inSimMode)
        {
            triggerArea.enabled = inSimMode;
        }
        public new void TriggerBlockFunctionality()
        {
            Debug.Log("This should do something to anything that goes in my trigger zone");
        }

        public void TriggerBlockFunctionality(Collider2D col)
        {
            col.attachedRigidbody.velocity += Vector2.right * gameObject.transform.right;
        }
        
        private void LateUpdate()
        {
            if(isUserHovering)
            {
                interfaceManager.OpenTooltipUI(displayName + "\n\n" + displayDescription, Input.mousePosition);
            }
        }
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