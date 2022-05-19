using System;
using UnityEngine;

namespace RubeGoldbergGame
{
    public class EditorBlockPlacingManager : MonoBehaviour
    {
        // DATA //
        // References
        public LevelManager levelManager;
        private Camera mainCam;
        public PlacingHologram placementHologram;
        
        public double timeRotate = 0f;
        public float pressTimeForRotation = 0.5f;

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
            if (Input.GetKeyUp(KeyCode.R))
            {
                timeRotate = 0f;
            }
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
                // Casts a ray that ONLY colliders with colliders on the "Player Block" layer (blocks that the player placed)
                Ray selectionRay = mainCam.ScreenPointToRay(mousePos);
                RaycastHit2D hitInfo = Physics2D.Raycast(selectionRay.origin, selectionRay.direction, 100, LayerMask.GetMask("Player Block"));

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
            
            //detecting long key press
            if (Input.GetKey(KeyCode.R))
            {
                Debug.Log("R is held down");
                
                timeRotate += Time.deltaTime;
                if (timeRotate > pressTimeForRotation)
                {
                    placementHologram.RotateClockwise(levelManager.blockPlaceRotationAmount * 0.1f);
                }
            }
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
            Debug.Log(hitInfo.collider);
            if (hitInfo.collider != null)
            {
                Debug.Log("HIT: " + hitInfo.collider.name);
                // Gets the block
                BlockBase hitBlock = hitInfo.collider.GetComponent<BlockBase>();

                // If it is placed by the player, deletes it and removes it from placed blocks list
                if (levelManager.placedBlocks.Contains(hitBlock))
                {
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