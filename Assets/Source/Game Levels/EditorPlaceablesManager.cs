﻿using System;
using UnityEngine;

namespace RubeGoldbergGame
{
    public class EditorPlaceablesManager : MonoBehaviour
    {
        // DATA //
        // References
        public LevelManager levelManager;
        private Camera mainCam;
        public PlacingHologram placementHologram;

        // State data
        public PlacementType currentPlacementType = PlacementType.None;
        private BlockBase placementBlock;


        // FUNCTIONS //
        // Unity Defaults
        private void Awake()
        {
            // Gets level references
            mainCam = Camera.main;
            levelManager = FindObjectOfType<LevelManager>();
        }

        private void Update()
        {
            EditorUpdate(levelManager.inSimulation);
        }


        // Internal Management
        void EditorUpdate(bool inSim)
        {
            // If in simulation mode, does nothing
            if(inSim)
            {
                return;
            }

            // Gets mouse and placement positions
            Vector3 mousePos = Input.mousePosition;
            Vector3 placementPos = Vector3.Scale(mainCam.ScreenToWorldPoint(mousePos), (new Vector3(1, 1, 0)));

            // Update hologram state
            UpdateHologram(placementPos);
            
            // On a click, casts a ray down mouse pos and does a different action depending on placement type
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Ray selectionRay = mainCam.ScreenPointToRay(mousePos);
                RaycastHit2D hitInfo = Physics2D.Raycast(selectionRay.origin, selectionRay.direction);

                switch (currentPlacementType)
                {
                    case PlacementType.None:
                        //TODO: select block item
                        break;
                    case PlacementType.Deletion:
                        AttemptDeleteObject(hitInfo);
                        break;
                    case PlacementType.PlaceHologram:
                        PlaceHologram(placementPos);
                        break;
                }
            }
        }

        private void UpdateHologram(Vector3 placementPos)
        {
            // Toggles whether its active
            placementHologram.ToggleHologram((!levelManager.inSimulation && currentPlacementType != PlacementType.None));

            // Updates position, whether it can be placed, colour
            placementHologram.UpdatePosition(placementPos);
            placementHologram.UpdateCanPlace();
            placementHologram.UpdateColour();

            // Checks if needs to rotate it
            if (Input.GetKeyDown(KeyCode.R))
            {
                placementHologram.RotateClockwise(levelManager.blockPlaceRotationAmount);
            }
        }


        // External Management
        public void UpdatePlacementStatus(PlacementType newPlacementType, BlockBase newPlacementBlock)
        {

        }

        public void SetHologramToBlock(BlockBase selectedBlock)
        {
            // Gets the sprite on the selected block
            Sprite displaySprite = selectedBlock.GetComponent<SpriteRenderer>().sprite;

            // Updates display sprite
            BoxCollider2D blockCollider = selectedBlock.GetComponent<BoxCollider2D>();
            placementHologram.UpdateSprite(displaySprite, blockCollider);

            // Updates which block is used
            placementBlock = selectedBlock;
        }

        public void SetHologramToDeletion(Sprite displaySprite)
        {
            // Updates the current block to null and the hologram to the given sprite
            placementHologram.UpdateSprite(displaySprite, null);
            placementBlock = null;
        }


        // On Click Functions
        private void AttemptDeleteObject(RaycastHit2D hitInfo)
        {
            // If it hit something, checks if it's a block and tries deleting it
            if (hitInfo.collider != null)
            {
                // Gets the block
                BlockBase hitBlock = hitInfo.collider.GetComponent<BlockBase>();

                // If it is placed by the player, deletes it and removes it from placed blocks list
                if (levelManager.placedBlocks.Contains(hitBlock))
                {
                    //TODO: fix the wrong block being deleted (if you have 2 pushers the specific order of pushers will be deleted)
                    //      Note: This is caused by the trigger collider being attached directly to the pusher object. It can be fixed by making it a child object and moving it
                    //            to a different layer than the parent object, maybe IgnoreRaycast.
                    levelManager.placedBlocks.Remove(hitBlock);
                    Destroy(hitBlock.gameObject);
                }
            }
        }

        private void PlaceHologram(Vector3 placementPos)
        {
            // If it's possible to place the block, instantiates it
            if (placementHologram.CanPlaceObject)
            {
                BlockBase placedBlock = Instantiate(placementBlock, placementPos, placementHologram.transform.rotation).GetComponent<BlockBase>();
                levelManager.placedBlocks.Add(placedBlock);
            }
        }

        
    }
}