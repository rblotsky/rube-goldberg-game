using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace RubeGoldbergGame
{
    public class UIBlockSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        // DATA //
        // UI References
        public Image spriteDisplayer;
        public LevelUIManager interfaceManager;
        
        // Usage Data
        public BlockBase assignedBlock;

        // Cached data
        private bool isUserHovering = false;
        private bool isDeletion = false;


        // FUNCTIONS //
        // Unity Defaults
        private void Awake()
        {
            interfaceManager = FindObjectOfType<LevelUIManager>();
        }

        private void Update()
        {
            if (isUserHovering)
            {
                // Determines the tooltip text
                string tooltipText = "<b> CLICK TO SELECT </b>\n";

                // Updates tooltip according to what's in this slot
                if(isDeletion)
                {
                    tooltipText += "Delete blocks";
                }
                else if(assignedBlock == null)
                {
                    tooltipText += "There is no block assigned.";
                }
                else
                {
                    tooltipText += assignedBlock.displayName;
                    tooltipText += "\n\n";
                    tooltipText += assignedBlock.displayDescription;
                }

                // Displays tooltip
                interfaceManager.OpenTooltipUI(tooltipText, Input.mousePosition);
            }
        }


        // External Management Functions
        public void SetupSlot(BlockBase block)
        {
            isDeletion = false;
            assignedBlock = block;
        }

        public void SetupAsDeletionSlot()
        {
            isDeletion = true;
            assignedBlock = null;
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

        public void OnPointerClick(PointerEventData pointerData)
        {
            // When clicked, runs a function in interfaceManager to tell it to start placing this block
            interfaceManager.SetCurrentPlacementBlock(assignedBlock);
        }

    }
}