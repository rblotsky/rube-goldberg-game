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
        public Image selectionOutline;
        public LevelManager levelManager;
        public PlaceablesUIManager slotSelectionManager;
        public PlacementType placementTypeUsed;

        // Usage Data
        public BlockBase assignedBlock;

        // Cached data
        private bool isUserHovering = false;


        // FUNCTIONS //
        // Unity Defaults
        private void Awake()
        {
            // Gets level references
            levelManager = FindObjectOfType<LevelManager>();
            slotSelectionManager = FindObjectOfType<PlaceablesUIManager>();

            // Assumes its not selected by default
            selectionOutline.enabled = false;

            // By default assumes its a deletion slot, if it isn't it will be updated externally later
            SetupAsDeletionSlot();
        }

        private void Update()
        {
            if (isUserHovering)
            {
                // Determines the tooltip text
                string tooltipText = "<b> CLICK TO SELECT </b>\n";
                switch (placementTypeUsed)
                {
                    case PlacementType.Deletion:
                        tooltipText += "Delete blocks";
                        break;
                    case PlacementType.PlaceHologram:
                        tooltipText += assignedBlock.displayName;
                        tooltipText += "\n\n";
                        tooltipText += assignedBlock.displayDescription;
                        break;
                    default:
                        tooltipText += "There is no block assigned.";
                        break;
                }

                // Displays tooltip
                levelManager.interfaceManager.OpenTooltipUI(tooltipText, Input.mousePosition);
            }
        }


        // External Management Functions
        public void SetupSlot(BlockBase block)
        {
            placementTypeUsed = PlacementType.PlaceHologram;
            assignedBlock = block;
            spriteDisplayer.sprite = assignedBlock.displaySprite;
        }

        public void SetupAsDeletionSlot()
        {
            placementTypeUsed = PlacementType.Deletion;
            assignedBlock = null;
        }


        // Interface Functions
        public void OnPointerEnter(PointerEventData pointerData)
        {
            isUserHovering = true;
        }

        public void OnPointerExit(PointerEventData pointerData)
        {
            levelManager.interfaceManager.CloseTooltipUI();
            isUserHovering = false;
        }

        public void OnPointerClick(PointerEventData pointerData)
        {
            // Updates the selected button
            slotSelectionManager.SetNewSelectedButton(this, assignedBlock, spriteDisplayer.sprite);
        }

    }
}