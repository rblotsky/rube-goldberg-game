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
        public LevelManager levelManager;
        public UIBlockSlotManager slotSelectionManager;
        public PlacementType placementTypeUsed;

        // Usage Data
        public BlockBase assignedBlock;


        // FUNCTIONS //
        // Unity Defaults
        protected override void Awake()
        {
            // Runs base awake for TooltipComponent
            base.Awake();

            // Gets level references
            levelManager = FindObjectOfType<LevelManager>();
            slotSelectionManager = FindObjectOfType<UIBlockSlotManager>(true);

            // Assumes its not selected by default
            selectionOutline.enabled = false;

            // By default assumes its a deletion slot, if it isn't it will be updated externally later
            SetupAsDeletionSlot();
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


        // Overrides
        public override string GetTooltipText()
        {
            // Determines tooltip text
            string tooltipText = LanguageManager.TranslateFromEnglish("<b> CLICK TO SELECT </b>\n");
            switch (placementTypeUsed)
            {
                case PlacementType.Deletion:
                    tooltipText += LanguageManager.TranslateFromEnglish("Delete blocks");
                    break;
                case PlacementType.PlaceHologram:
                    tooltipText += LanguageManager.TranslateFromEnglish(assignedBlock.GetTooltipText());
                    break;
                default:
                    tooltipText += LanguageManager.TranslateFromEnglish("There is no block assigned.");
                    break;
            }

            // Returns the determined text, translated.
            return tooltipText;
        }


        // Interface Functions
        public void OnPointerClick(PointerEventData pointerData)
        {
            // Updates the selected button
            slotSelectionManager.SetNewSelectedButton(this, assignedBlock, spriteDisplayer.sprite);
        }

    }
}