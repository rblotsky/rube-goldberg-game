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
        internal int PointerHoverCount
        {
            get;
            set;
        }
        
        internal bool IsUserHovering
        {
            get { return (PointerHoverCount != 0) ? true : false; }
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
                interfaceManager.CloseTooltipUI();
            }
        }


        // Interface Functions
        public virtual void OnPointerEnter(PointerEventData pointerData)
        {
            PointerHoverCount += 1;
        }

        public virtual void OnPointerExit(PointerEventData pointerData)
        {
            interfaceManager.CloseTooltipUI();
            PointerHoverCount -= 1;
        }


        // Virtual Functions
        public virtual string GetTooltipText()
        {
            return LanguageManager.TranslateFromEnglish("Default tooltip text");
        }
    }
}