using System;
using System.Collections.Generic;
using UnityEngine;

namespace RubeGoldbergGame
{
    public class UIBlockSlotManager : MonoBehaviour
    {
        // DATA //
        // Scene References
        public UIBlockSlot originalBlockSlot;

        // Cached Data
        private EditorBlockPlacingManager placementManager;
        private UIBlockSlot selectedSlot = null;
        private List<UIBlockSlot> placedBlockSlots = new List<UIBlockSlot>();


        // FUNCTIONS //
        // Unity defaults
        private void Awake()
        {
            // Gets level references
            placementManager = FindObjectOfType<EditorBlockPlacingManager>();
        }
        

        // UI Events
        public void UpdateSelectedSlotUI(UIBlockSlot newSelectedSlot)
        {
            // Clears the current selected slot's outline, will be reset after its changed
            if (selectedSlot != null)
            {
                selectedSlot.selectionOutline.enabled = false;
                //selectedSlot.PointerHoverCount = 0; NOTE: This has been removed for now since I don't really know what it does but it seems unnecessary
            }

            // If this slot is already selected, deselects it
            if (selectedSlot == newSelectedSlot)
            {
                selectedSlot.selectionOutline.enabled = false;
                selectedSlot = null;
            }

            // Updates selected block slot and its UI
            selectedSlot = newSelectedSlot;
            //selectedSlot.PointerHoverCount = 1; NOTE: This has been commented for now since I don't really know what it does but it seems unnecessary
            selectedSlot.selectionOutline.enabled = true;
        }

        public UIBlockSlot GetSlotAtIndex(int slotIndex)
        {
            // Checks if there's a slot with that hotkey index and returns it (NOTE: What if we have more than 10 slots?)
            if(placedBlockSlots.Count > slotIndex && slotIndex >= 0)
            {
                return placedBlockSlots[slotIndex];
            }

            // Returns null by default
            return null;
        }
        
        
        // UI Generation
        public void GenerateBlockSlots(BlockBase[] blocks)
        {
            // First slot is the delete option, doesn't have a block assigned.
            originalBlockSlot.SetupSlot(null);
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