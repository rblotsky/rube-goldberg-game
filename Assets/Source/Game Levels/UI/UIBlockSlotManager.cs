using System;
using System.Collections.Generic;
using UnityEngine;

namespace RubeGoldbergGame
{
    public class UIBlockSlotManager : MonoBehaviour
    {
        // DATA //
        // Scene References
        private EditorBlockPlacingManager placementManager;
        public UIBlockSlot originalBlockSlot;
        private UIBlockSlot selectedSlot = null;
        private List<UIBlockSlot> placedBlockSlots = new List<UIBlockSlot>();


        // FUNCTIONS //
        // Unity defaults
        private void Awake()
        {
            // Gets level references
            placementManager = FindObjectOfType<EditorBlockPlacingManager>();
        }
        
        //selecting the blocks through hotkey
        private void Update()
        {
            //REDO THE SYSTEM FOR DETECTING KEY DOWN
            if (Input.GetKeyDown("1"))
            {
                setNewSelectedButtonThroughHotkey(1);
            }
            
            if (Input.GetKeyDown("2"))
            {
                setNewSelectedButtonThroughHotkey(2);
            }
            
            if (Input.GetKeyDown("3"))
            {
                setNewSelectedButtonThroughHotkey(3);
            }
            
            if (Input.GetKeyDown("4"))
            {
                setNewSelectedButtonThroughHotkey(4);
            }
        }

        // UI Events
        public bool SetNewSelectedButton(UIBlockSlot newSelectedSlot, BlockBase block, Sprite displaySprite)
        {
            // Clears the current selected slot's outline, will be reset after its changed
            if (selectedSlot != null)
            {
                selectedSlot.selectionOutline.enabled = false;
            }

            // If this slot is already selected, deselects it
            if (selectedSlot == newSelectedSlot)
            {
                selectedSlot.selectionOutline.enabled = false;
                selectedSlot = null;
                placementManager.currentPlacementType = PlacementType.None;

                // Returns false (none selected)
                return false;
            }

            // Updates selected block slot and its UI
            selectedSlot = newSelectedSlot;
            selectedSlot.selectionOutline.enabled = true;

            //TODO:come up with a better way of updating hologram
            if (block == null)
            {
                placementManager.SetHologramToDeletion(displaySprite);
            }
            else
            {
                placementManager.SetHologramToBlock(block);
            }
            
            // Updates the editor placement state
            placementManager.currentPlacementType = newSelectedSlot.placementTypeUsed;

            // Returns true (something selected)
            return true;
        }

        public void setNewSelectedButtonThroughHotkey(int buttonIndex)
        {
            if (buttonIndex <= placedBlockSlots.Count)
            {
                var selectedBlock = placedBlockSlots[buttonIndex - 1];
                SetNewSelectedButton(selectedBlock, selectedBlock.assignedBlock, selectedBlock.spriteDisplayer.sprite);
            }
            
            
        }
        
        
        // UI Generation
        public void GenerateBlockSlots(BlockBase[] blocks)
        {
            // First slot is the delete option, doesn't have a block assigned.
            originalBlockSlot.SetupAsDeletionSlot();
            placedBlockSlots.Add(originalBlockSlot);
            
            // Creates a block slot for each block used
            for(int i = 0; i < blocks.Length; i++)
            {
                PlaceNewBlockSlot(blocks[i]);
            }
        }
        
        public void PlaceNewBlockSlot(BlockBase blockUsed)
        {
            // Adds a new block slot to the right of the original slot (positioning is handled by the layout component)
            UIBlockSlot newSlot = Instantiate(originalBlockSlot, originalBlockSlot.transform.position, originalBlockSlot.transform.rotation, originalBlockSlot.transform.parent).GetComponent<UIBlockSlot>();
            newSlot.SetupSlot(blockUsed);
            placedBlockSlots.Add(newSlot);
        }
    }
}