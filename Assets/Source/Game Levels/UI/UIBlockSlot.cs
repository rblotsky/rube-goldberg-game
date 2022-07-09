using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace RubeGoldbergGame
{
    public class UIBlockSlot : TooltipComponent, IPointerClickHandler
    {
        // DATA //
        // UI References
        public Image spriteDisplayer;
        public Image selectionOutline;
        private LevelManager levelManager;
        private EditorBlockPlacingManager editorBlockManager;

        // Usage Data
        public BlockBase assignedBlock;
        public string deletionText = "Delete blocks";


        // FUNCTIONS //
        // Unity Defaults
        protected override void Awake()
        {
            // Runs base awake for TooltipComponent
            base.Awake();

            // Gets level references
            levelManager = FindObjectOfType<LevelManager>(true);
            editorBlockManager = FindObjectOfType<EditorBlockPlacingManager>(true);

            // Assumes its not selected by default
            selectionOutline.enabled = false;

            // By default assumes its a deletion slot, if it isn't it will be updated externally later
            SetupSlot(null);
        }


        // External Management Functions
        public void SetupSlot(BlockBase block)
        {
            // If the block isn't null, updates the assigned block and sprite to that block
            if (block != null)
            {
                assignedBlock = block;
                spriteDisplayer.sprite = assignedBlock.displaySprite;
            }

            // Otherwise, assumes it's a deletion slot and sets the sprite to be the deletion sprite
            else
            {
                assignedBlock = null;
                spriteDisplayer.sprite = editorBlockManager.deletionSprite;
            }
        }

        public void UpdateSelectionOutline(bool enabled)
        {
            selectionOutline.enabled = enabled;
        }


        // Overrides
        public override string GetTooltipText()
        {
            // Determines tooltip text, depending on whether the assignedBlock is null
            string tooltipText = LanguageManager.TranslateFromEnglish("<b> CLICK TO SELECT </b>\n");
            switch (assignedBlock)
            {
                case null:
                    tooltipText += LanguageManager.TranslateFromEnglish(deletionText);
                    break;
                default:
                    tooltipText += LanguageManager.TranslateFromEnglish(assignedBlock.GetTooltipText());
                    break;
            }

            // Returns the determined text, translated.
            return tooltipText;
        }


        // Events
        public void SelectSlot()
        {
            // Notifies the editor block manager
            editorBlockManager.HandleBlockSlotSelection(this);
        }


        // Interface Functions
        public void OnPointerClick(PointerEventData pointerData)
        {
            // On click, selects the slot
            SelectSlot();
        }

    }
}