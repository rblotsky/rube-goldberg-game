using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RubeGoldbergGame
{
    public class EditorBlockPlacingManager : MonoBehaviour
    {
        // DATA //
        // References
        private LevelManager levelManager;
        private PlacingHologram placementHologram;
        public UISelectionBox selectionPanel;
        private Camera mainCam;
        private LevelData levelData;
        private Grid placementGrid;

        // Usage data
        [Header("Required Data")]
        public float rotationIncrementDelay = 0.2f;
        public float rotationIncrementAmount = -15f;
        public Sprite deletionSprite;
        public Material defaultSpriteMaterial;
        public Material outlineSelectionMaterial;
        
        // Selection Scripts
        public ManagerSelectingBase selectingSriptManager;


        // Required Scene References
        [Header("Scene References")]
        public RectTransform selectionBox;

        // Cached data
        private PlacementType currentPlacementType = PlacementType.None;
        private BlockBase placementBlock;
        private List<BlockBase> placedBlocks = new List<BlockBase>();
        private UIBlockSlotManager uiSlotManager;
        private Vector2 selectionStartPoint;
        private Canvas selectionBoxCanvas;
        private List<BlockBase> selectedBlocks = new List<BlockBase>();
        private float _initialRotationDelay = 0.6f;
        private float rotationTime = 0f;

        // Properties
        public int BlocksUsed { get { return placedBlocks.Count; } } 
        private BlockBase selectionMoveBlock;


        // FUNCTIONS //
        // Unity Defaults
        private void Awake()
        {
            // Gets level references
            mainCam = Camera.main;
            levelManager = FindObjectOfType<LevelManager>(true);
            selectionPanel = FindObjectOfType<UISelectionBox>(true);
            placementHologram = FindObjectOfType<PlacingHologram>(true);
            levelData = levelManager.levelData;
            placementGrid = FindObjectOfType<Grid>(true);
            uiSlotManager = FindObjectOfType<UIBlockSlotManager>(true);
            selectionBoxCanvas = selectionBox.GetComponentInParent<Canvas>();
        }

        private void Update()
        {
            EditorUpdate(levelManager.inSimulation);
        }


        // Internal Management
        private void EditorUpdate(bool inSim)
        {
            // If in simulation mode, disables hologram and stops placing anything
            if (inSim)
            {
                currentPlacementType = PlacementType.None;
                placementBlock = null;
                placementHologram.ToggleHologram(false);
                return;
            }
            
            // Enables the hologram if the placement type is deleting or placing, disables if modifying selection or none
            placementHologram.ToggleHologram(currentPlacementType != PlacementType.None && currentPlacementType != PlacementType.ModifyingSelection);
            
            // Selection move script
            if (currentPlacementType == PlacementType.None)
            {
                if (!selectingSriptManager.enabled)
                {
                    selectingSriptManager.enabled = true;
                }
            }
            else
            {
                selectingSriptManager.enabled = false;
            }

            // Gets screen mouse and world mouse positions
            Vector3 screenMousePos = Input.mousePosition; 
            Vector3 worldMousePos = Vector3.Scale(mainCam.ScreenToWorldPoint(screenMousePos), (new Vector3(1, 1, 0)));
            Vector3 snappedMousePos = UtilityFuncs.SnapToGrid(worldMousePos, placementGrid);

            // If the hologram is active, updates its position and colour
            if (placementHologram.gameObject.activeInHierarchy)
            {
                placementHologram.UpdatePosition(snappedMousePos);
                placementHologram.UpdateColour(placementHologram.GetCanPlace());
            }

            // Decides what to do depending on the placement type
            if(currentPlacementType == PlacementType.Deletion)
            {
                // Resets the hologram to default rotation
                placementHologram.ResetRotation();
            }
            
            else if(currentPlacementType == PlacementType.PlaceHologram)
            {
                // Does nothing if this block has a custom placement function
                if (!placementBlock.hasCustomPlacement)
                {
                    // Rotates the hologram if the according to player input
                    RotateHologramFromUserInput();
                }
            }

            else if(currentPlacementType == PlacementType.ModifyingSelection)
            {
                // If the player presses backspace, deletes every selected block and cancels modifying
                if(Input.GetKeyDown(KeyCode.Backspace))
                {
                    // Caches the blocks to delete list, since if we use the selected blocks list it will be modified during the loop and will crash the program.
                    List<BlockBase> blocksToDelete = new List<BlockBase>(selectedBlocks);
                    foreach(BlockBase block in blocksToDelete)
                    {
                        DeleteBlockAction(block);
                    }

                    // Cancels the placement type
                    CancelCurrentPlacementType();
                }
            }

            // If the player right clicks, cancels their current placement type in favour of none
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                CancelCurrentPlacementType();
            }
        }
        

        public void RotateSelectionFromInput()
        {
            // Gets the center of the selection
            /*
            
            ??? ????? ?? ???? ?????? ????
            ????? ?????? ???????????
              ?????????? ???????????? ????????????
              ??????
              ?????????????? ????
                  ??????????? ???????? ????????
                  ??????????????? ????
                  ????????? ?????? ??????????? ????????????
                  ????????? ????????????
                  ?????????????
                  ??
                  ?????? ????????? ??????
              ??
              ??????? ?????? ???
              ????
              ???????? ??
              ??? ??????????
            ??????????
            ???????

            */

            // If the player presses R, rotates the selection the rotation increment amount
            if(Input.GetKeyDown(KeyCode.R))
            {
                // Rotates all the selected blocks around the center of the selection
            }
        }


        // External Management
        public void CancelCurrentPlacementType()
        {
            // If the player is currently placing a block, and it has a custom placement function, stops it
            if(currentPlacementType == PlacementType.PlaceHologram && placementBlock.hasCustomPlacement)
            {
                //todo
            }

            // Cancels whatever the player is currently doing in placement
            currentPlacementType = PlacementType.None;
            placementBlock = null;
            

            // Updates UI
            uiSlotManager.UpdateSelectedSlotUI(null);
        }

        public void ResetPositionOfBlocks()
        {
            foreach (BlockBase block in placedBlocks)
            {
                block.SimulationResetPos();
            }
        }


        // Hologram Management
        private void RotateHologramFromUserInput()
        {
            //TODO: Use input manager or Q and E to rotate better.
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
                if (rotationTime > rotationIncrementDelay)
                {
                    placementHologram.RotateClockwise(rotationIncrementAmount);
                    rotationTime = 0;
                }
            }
        }

        private void UpdateHologramSpriteAndCollider()
        {
            // If current block is null, uses the deletion sprite and a null block collider
            if (placementBlock == null)
            {
                placementHologram.UpdateSprite(deletionSprite, null);
            }

            // Otherwise, uses the current block's sprite and collider
            else
            {
                // Gets the sprite on the selected block
                Sprite displaySprite = placementBlock.GetComponent<SpriteRenderer>().sprite;

                // Updates display sprite
                BoxCollider2D blockCollider = placementBlock.GetComponent<BoxCollider2D>();
                placementHologram.UpdateSprite(displaySprite, blockCollider);
            }
        }
        


        // Event Handling
        public void HandleBlockSlotSelection(UIBlockSlot selectedBlockSlot)
        {
            // Gets the selected block
            BlockBase selectedBlock = selectedBlockSlot.assignedBlock;

            // If the block slot is null, update to deletion
            if (selectedBlock == null)
            {
                currentPlacementType = PlacementType.Deletion;
                placementBlock = null;
                uiSlotManager.UpdateSelectedSlotUI(selectedBlockSlot);
            }

            // Otherwise if it's the same as the current one, cancels the current placement
            else if (selectedBlock == placementBlock)
            {
                CancelCurrentPlacementType();
            }

            // If it's a new block, updates the selected block
            else
            {
                currentPlacementType = PlacementType.PlaceHologram;
                placementBlock = selectedBlock;
                uiSlotManager.UpdateSelectedSlotUI(selectedBlockSlot);
            }
            
            // Updates hologram
            UpdateHologramSpriteAndCollider();
        }

        public void HandleHologramClick(Vector3 hologramPos)
        {
            // Decides whether to place or delete blocks and does that if it's possible.
            if(currentPlacementType == PlacementType.PlaceHologram)
            {
                // If it's possible to place the block, places it.
                if(placementHologram.GetCanPlace())
                {
                    PlaceBlockAction(hologramPos, placementHologram.transform.rotation, placementBlock);
                }
            }

            else if(currentPlacementType == PlacementType.Deletion)
            {
                // Deletes the block at the mouse position if there is one
                DeleteBlockAtScreenPos(Input.mousePosition);
            }
        }


        // Block Actions
        public void DeleteBlockAtScreenPos(Vector3 screenPos)
        {
            // Only does anything if current placement type is deletion
            if (currentPlacementType == PlacementType.Deletion)
            {
                // Runs a raycast
                Ray selectionRay = mainCam.ScreenPointToRay(screenPos);
                RaycastHit2D hitInfo = Physics2D.Raycast(selectionRay.origin, selectionRay.direction, 100, LayerMask.GetMask("Player Block"));

                // If it hit something, checks if it's a block and tries deleting it
                if (hitInfo.collider != null)
                {
                    // Gets the block
                    BlockBase hitBlock = hitInfo.collider.GetComponent<BlockBase>();

                    // Runs the deletion action
                    DeleteBlockAction(hitBlock);
                }
            }
        }

        public void PlaceBlockAction(Vector3 position, Quaternion rotation, BlockBase block)
        {
            BlockBase placedBlock = Instantiate(placementBlock, position, rotation).GetComponent<BlockBase>();
            placedBlocks.Add(placedBlock);
        }

        public void DeleteBlockAction(BlockBase block)
        {
            if (placedBlocks.Contains(block))
            {
                if(selectedBlocks.Contains(block))
                {
                    selectedBlocks.Remove(block);
                }

                placedBlocks.Remove(block);
                Destroy(block.gameObject);
            }
        }


        // Data Management
        public void DeleteAllBlocks()
        {
            foreach(BlockBase block in placedBlocks)
            {
                Destroy(block.gameObject);
            }

            placedBlocks.Clear();
        }


        public void LoadAllBlocks(string saveName)
        {
            // Deletes all existing blocks
            DeleteAllBlocks();

            // Gets all data
            string stringData = GlobalData.LoadLevel(levelData.name, saveName);
            string[] lines = stringData.Split('\n');

            // Goes through all the lines, loads the appropriate block
            foreach(string line in lines)
            {
                string[] dataItems = line.Trim().Split(',');

                // If dataItems has less than 1 item, does nothing
                if(dataItems.Length < 1)
                {
                    continue;
                }
                else
                {
                    // Finds a block with the appropriate name
                    BlockBase block = levelManager.availableBlocks.FirstOrDefault(x => x.displayName.Equals(dataItems[0]));

                    // If no block found, does nothing
                    if(block == null)
                    {
                        continue;
                    }

                    // Otherwise, instantiates a copy of it
                    BlockBase newBlock = Instantiate(block.gameObject).GetComponent<BlockBase>();
                    placedBlocks.Add(newBlock);
                    newBlock.LoadBlockData(dataItems);
                }
            }
        }

        public void SaveAllBlocks(string saveName)
        {
            // Gets a list of block data strings and saves it to the file
            List<string> savedBlocks = new List<string>();

            foreach(BlockBase block in placedBlocks)
            {
                savedBlocks.Add(block.SaveBlockData());
            }

            // Saves using GlobalData
            GlobalData.CreateNewLevelSave(levelData.name, saveName, savedBlocks.ToArray());
        }
        
    }
}