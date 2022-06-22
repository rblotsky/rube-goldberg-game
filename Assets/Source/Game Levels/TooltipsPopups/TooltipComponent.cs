using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RubeGoldbergGame
{
    public class TooltipComponent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        // DATA //
        // Cached data
        internal bool isUserHovering
        {
            get;
            set;
        }
        
        private LevelUIManager interfaceManager;


        // FUNCTIONS //
        // Unity Defaults
        protected virtual void Awake()
        {
            interfaceManager = FindObjectOfType<LevelUIManager>(true);
            isUserHovering = false;
        }

        protected virtual void Update()
        {
            if(isUserHovering)
            {
                interfaceManager.OpenTooltipUI(GetTooltipText(), Input.mousePosition);
            }
        }

        protected virtual void OnDestroy()
        {
            if(isUserHovering)
            {
                interfaceManager.CloseTooltipUI();
            }
        }


        // Interface Functions
        public virtual void OnPointerEnter(PointerEventData pointerData)
        {
            isUserHovering = true;
        }

        public virtual void OnPointerExit(PointerEventData pointerData)
        {
            interfaceManager.CloseTooltipUI();
            isUserHovering = false;
        }


        // Virtual Functions
        public virtual string GetTooltipText()
        {
            return LanguageManager.TranslateFromEnglish("Default tooltip text");
        }
    }
}