using System.Collections.Generic;
using UnityEngine;

namespace RubeGoldbergGame
{
    public class PlaceablesUIManager : MonoBehaviour
    {
        private List<UIBlockSlot> placedBlockSlots = new List<UIBlockSlot>();
        public EditorPlaceablesManager interactionsManager;
        public UIBlockSlot originalBlockSlot;
        private UIBlockSlot selectedButton = null;
        
        //updates selected button if it is not selected
        //resets selected button if it has been selected already
        public bool setNewSelectedButton(UIBlockSlot block, BlockBase blkBase, Sprite img)
        {
            if (selectedButton != null)
            {
                selectedButton.spriteSelected.enabled = false;
            }
            
            if (selectedButton == block)
            {
                selectedButton = null;
                interactionsManager.editorPlacementState = PlacementTypes.None;
                return false;
            }

            //TODO:come up with a better way of updating hologram
            if (blkBase == null)
            {
                interactionsManager.SetHologramToDeletion(img);
            }
            else
            {
                interactionsManager.SetHologramToBlock(blkBase);
            }
            
            interactionsManager.editorPlacementState = block.buttonAction;
            selectedButton = block;
            return true;
        }

        public void GenerateBlockSlots(BlockBase[] blocks)
        {
            // First slot is the delete option, doesn't have a block assigned.
            originalBlockSlot.SetupAsDeletionSlot();

            // Creates a block slot for each block used
            for(int i = 0; i < blocks.Length; i++)
            {
                PlaceNewBlockSlot(blocks[i]);
            }
        }
        
        public void PlaceNewBlockSlot(BlockBase blockUsed)
        {
            // Gets the block slot to placed in reference to
            UIBlockSlot referenceSlot = null;
            if(placedBlockSlots.Count == 0)
            {
                referenceSlot = originalBlockSlot;
            }
            else
            {
                referenceSlot = placedBlockSlots[placedBlockSlots.Count - 1];
            }

            // Adds a new block slot to the right of the reference slot
            UIBlockSlot newSlot = Instantiate(originalBlockSlot, originalBlockSlot.transform.position, originalBlockSlot.transform.rotation, originalBlockSlot.transform.parent).GetComponent<UIBlockSlot>();
            newSlot.SetupSlot(blockUsed);
            placedBlockSlots.Add(newSlot);
        }
    }
}