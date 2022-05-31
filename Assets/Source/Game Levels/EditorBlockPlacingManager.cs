using System.Collections.Generic;
using UnityEngine;

namespace RubeGoldbergGame
{
    public class EditorBlockPlacingManager : MonoBehaviour
    {
        // DATA //
        // References
        private LevelManager levelManager;
        private PlacingHologram placementHologram;
        private UISelectionBox selectionPanel;
        private Camera mainCam;

        // Usage data
        private float _initialRotationDelay = 0.6f;
        public float rotationIncrementDelay = 0.2f;
        public float rotationIncrementAmount = -15f;
        private float rotationTime = 0f;

        // State data
        public PlacementType currentPlacementType = PlacementType.None;
        private BlockBase placementBlock;
        private List<BlockBase> placedBlocks = new List<BlockBase>();

        // Properties
        public int BlocksUsed { get { return placedBlocks.Count; } }


        // FUNCTIONS //
        // Unity Defaults
        private void Awake()
        {
            // Gets level references
            mainCam = Camera.main;
            levelManager = FindObjectOfType<LevelManager>(true);
            selectionPanel = FindObjectOfType<UISelectionBox>(true);
            placementHologram = FindObjectOfType<PlacingHologram>(true);
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
        }

        private void UpdateHologram(Vector3 placementPos)
        {
            // Toggles whether its active
            placementHologram.ToggleHologram((!levelManager.inSimulation && currentPlacementType != PlacementType.None));

            // Updates position, whether it can be placed, colour
            placementHologram.UpdatePosition(placementPos);
            placementHologram.UpdateCanPlace();
            placementHologram.UpdateColour();
            
            // If R is held down, rotates in increments over time
            if (Input.GetKey(KeyCode.R))
            {
                // First time R is pressed, has to wait an extra bit of time
                if (Input.GetKeyDown(KeyCode.R))
                {
                    placementHologram.RotateClockwise(rotationIncrementAmount);
                    rotationTime = -_initialRotationDelay; // Resets rotationTIme to the negative version of the initial delay so it goes the extra time first
                }
                rotationTime += Time.unscaledDeltaTime;

                // If the time is over the required increment delay, rotates and resets the counter to 0
                if(rotationTime > rotationIncrementDelay)
                {
                    placementHologram.RotateClockwise(rotationIncrementAmount);
                    rotationTime = 0;
                }
            }
        }


        // External Management
        public void UpdatePlacementStatus(PlacementType newPlacementType, BlockBase newPlacementBlock)
        {

        }

        public void SetHologramToBlock(BlockBase selectedBlock)
        {
            Debug.Log("AAAA");
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
        public void AttemptDeleteObject()
        {
            // Only does anything if current placement type is deletion
            if (currentPlacementType == PlacementType.Deletion)
            {
                // Runs a raycast
                Ray selectionRay = mainCam.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hitInfo = Physics2D.Raycast(selectionRay.origin, selectionRay.direction, 100, LayerMask.GetMask("Player Block"));

                // If it hit something, checks if it's a block and tries deleting it
                if (hitInfo.collider != null)
                {
                    // Gets the block
                    BlockBase hitBlock = hitInfo.collider.GetComponent<BlockBase>();

                    // If it is placed by the player, deletes it and removes it from placed blocks list
                    if (placedBlocks.Contains(hitBlock))
                    {
                        placedBlocks.Remove(hitBlock);
                        Destroy(hitBlock.gameObject);
                    }
                }
            }
        }

        public void AttemptSelectObject(BlockBase blockInfo, ISelectableObject selectableObject )
        {
            // If correct placement type, and it is placed by the player, selects the object
            if(currentPlacementType == PlacementType.None)
            {
                if (placedBlocks.Contains(blockInfo))
                { 
                    if(selectableObject != null)
                    {
                        selectionPanel.CloseSelectionBox();
                        selectableObject.ActivateSelectionPanel(selectionPanel);
                    }
                }
            }
        }

        public void AttemptPlaceBlock(Vector3 placementPos)
        {
            // Makes sure that the current type is correct
            if (currentPlacementType == PlacementType.PlaceHologram)
            {
                // If it's possible to place the block, instantiates it
                if (placementHologram.CanPlaceObject)
                {
                    BlockBase placedBlock = Instantiate(placementBlock, placementPos, placementHologram.transform.rotation).GetComponent<BlockBase>();
                    placedBlocks.Add(placedBlock);
                }
            }
        }

        
    }
}