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
        public Image spriteSelected;
        public LevelManager levelManager;
        public PlaceablesUIManager myButtonManager;
        public PlacementTypes buttonAction;

            // Usage Data
        public BlockBase assignedBlock;

        // Cached data
        private bool isUserHovering = false;


        // FUNCTIONS //
        // Unity Defaults
        private void Awake()
        {
            levelManager = FindObjectOfType<LevelManager>();
            spriteSelected.enabled = false;
        }

        private void Update()
        {
            if (isUserHovering)
            {
                // Determines the tooltip text
                string tooltipText = "<b> CLICK TO SELECT </b>\n";
                switch (buttonAction)
                {
                    case PlacementTypes.Deletion:
                        tooltipText += "Delete blocks";
                        break;
                    case PlacementTypes.PlaceHologram:
                        tooltipText += assignedBlock.displayName;
                        tooltipText += "\n\n";
                        tooltipText += assignedBlock.displayDescription;
                        break;
                    default:
                        tooltipText += "There is no block assigned.";
                        break;
                }
                // Updates tooltip according to what's in this slot

                // Displays tooltip
                levelManager.interfaceManager.OpenTooltipUI(tooltipText, Input.mousePosition);
            }
        }


        // External Management Functions
        public void SetupSlot(BlockBase block)
        {
            buttonAction = PlacementTypes.PlaceHologram;
            assignedBlock = block;
            spriteDisplayer.sprite = assignedBlock.displaySprite;
        }

        public void SetupAsDeletionSlot()
        {
            buttonAction = PlacementTypes.Deletion;
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
            if(myButtonManager.setNewSelectedButton(this))
            {
                spriteSelected.enabled = true;
            }
            else
            {
                spriteSelected.enabled = false;
            }

            // When clicked, runs a function in interfaceManager to tell it to start placing this block or to start deletion
            // depending on whether there is an assigned block
            if(assignedBlock == null)
            {
                levelManager.SetHologramToDeletion(spriteDisplayer.sprite);
            }
            else
            {
                levelManager.SetHologramToBlock(assignedBlock);
            }
        }

    }
}