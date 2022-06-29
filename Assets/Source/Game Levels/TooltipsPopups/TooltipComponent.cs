using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//TODO: redo tooltip component functionality to function differently
//currently opens and closes on basis of pointer exiting and entering events
//which sometimes fails
//when refactoring this code:
//also need to change UIBlockSlotManager

namespace RubeGoldbergGame
{
    public class TooltipComponent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        // DATA //
        // Cached data
        private int _pointerHoverCount;
        internal int PointerHoverCount
        {
            get { return _pointerHoverCount;}
            set { _pointerHoverCount = Math.Clamp(value, 0,100);}
        }
        
        internal bool IsUserHovering
        {
            get { return (_pointerHoverCount != 0) ? true : false; }
        }
        private LevelUIManager interfaceManager;


        // FUNCTIONS //
        // Unity Defaults
        protected virtual void Awake()
        {
            interfaceManager = FindObjectOfType<LevelUIManager>(true);
            PointerHoverCount = 0;
        }

        protected virtual void Update()
        {
            if(IsUserHovering)
            {
                interfaceManager.OpenTooltipUI(GetTooltipText(), Input.mousePosition);
            }
        }

        protected virtual void OnDestroy()
        {
            if(IsUserHovering)
            {
                Debug.Log("destroyed");
                interfaceManager.CloseTooltipUI();
            }
        }


        // Interface Functions
        public virtual void OnPointerEnter(PointerEventData pointerData)
        {
            
            PointerHoverCount += 1;
            Debug.Log("Mouse enter on object " + this.name + " Count is now " + PointerHoverCount);
        }

        public virtual void OnPointerExit(PointerEventData pointerData)
        {
            
            PointerHoverCount -= 1;
            if (!IsUserHovering)
            {
                interfaceManager.CloseTooltipUI();
            }
            Debug.Log("Mouse exit on object " + this.name + " Count is now " + PointerHoverCount);
        }


        // Virtual Functions
        public virtual string GetTooltipText()
        {
            return LanguageManager.TranslateFromEnglish("Default tooltip text");
        }
    }
}