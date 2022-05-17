using System;
using UnityEngine;

namespace RubeGoldbergGame
{
    public class EditorPlaceablesManager : MonoBehaviour
    {
        private BlockBase placementBlock;
        public PlacementTypes editorPlacementState = PlacementTypes.None;
        public PlacingHologram placementHologram;
        public LevelManager levelManager;
        private Camera mainCam;
        
        
        private void Awake()
        {
            // Gets level references
            mainCam = Camera.main;
        }

        private void Update()
        {
            EditorUpdate(levelManager.inSimulation);
        }

        void EditorUpdate(bool inSim)
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 placementPos = Vector3.Scale(mainCam.ScreenToWorldPoint(mousePos), (new Vector3(1, 1, 0)));
            if (editorPlacementState != PlacementTypes.None)
            {
                placementHologram.UpdateCanPlace();
                placementHologram.UpdateColour();
                UpdateHologram(placementPos);
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Ray selectionRay = mainCam.ScreenPointToRay(mousePos);
                RaycastHit2D hitInfo = Physics2D.Raycast(selectionRay.origin, selectionRay.direction);
                switch (editorPlacementState)
                {
                    case PlacementTypes.None:
                        //TODO: select block item
                        break;
                    case PlacementTypes.Deletion:
                        AttemptDeleteObject(hitInfo);
                        break;
                    case PlacementTypes.PlaceHologram:
                        PlaceHologram(placementPos);
                        break;

                }
            }
        }

        private void UpdateHologram(Vector3 placementPos)
        {
            placementHologram.ToggleHologram(!(levelManager.inSimulation));
            placementHologram.UpdatePosition(placementPos);
            
            if (Input.GetKeyDown(KeyCode.R))
            {
                placementHologram.RotateClockwise(levelManager.blockPlaceRotationAmount);
            }
        }
        
        //deletes a placeable if the mouse is over one
        private void AttemptDeleteObject(RaycastHit2D hitInfo)
        {
            if (hitInfo.collider != null)
            {
                
                
                // Gets the block
                BlockBase hitBlock = hitInfo.collider.GetComponent<BlockBase>();

                // If it is placed by the player, deletes it and removes it from placed blocks list
                if (levelManager.placedBlocks.Contains(hitBlock))
                {
                    //TODO: fix the wrong block being deleted (if you have 2 pushers the specific order of pushers will be deleted)
                    Destroy(hitBlock.gameObject);
                    levelManager.placedBlocks.Remove(hitBlock);
                    Debug.Log("Deleted a block!");
                }
            }
        }

        private void PlaceHologram(Vector3 placementPos)
        {


            // If player clicks, either places or deletes.
            if (placementHologram.CanPlaceObject)
            {
                BlockBase placedBlock = Instantiate(placementBlock, placementPos, placementHologram.transform.rotation).GetComponent<BlockBase>();
                levelManager.placedBlocks.Add(placedBlock);
            }
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

        
    }
}