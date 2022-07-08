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
        private UISelectionBox selectionPanel;
        private Camera mainCam;
        private LevelData levelData;
        private Grid placementGrid;

        // Usage data
        [Header("Required Data")]
        public Sprite deletionSprite;
        public Material defaultSpriteMaterial;
        public Material outlineSelectionMaterial;

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

            else
            {
                // Enables the hologram if the placement type is deleting or placing, disables if modifying selection or none
                placementHologram.ToggleHologram(currentPlacementType != PlacementType.None && currentPlacementType != PlacementType.ModifyingSelection);
            }

            // Gets screen mouse and world mouse positions
            Vector3 screenMousePos = Input.mousePosition; 
            Vector3 worldMousePos = Vector3.Scale(mainCam.ScreenToWorldPoint(screenMousePos), (new Vector3(1, 1, 0)));
            Vector3 snappedMousePos = placementGrid.CellToWorld(placementGrid.WorldToCell(worldMousePos));

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
                    placementHologram.RotateHologramFromUserInput();
                }
            }

            else if(currentPlacementType == PlacementType.ModifyingSelection)
            {
                //TODO: Figure out how to modify selections properly (has to move and rotate a large selection of blocks at once, or just one block, or view the selection panel for that one block somehow?)
            }

            else
            {
                // If the player isn't doing anything, runs selection region
                UpdateSelectionBox();
                
            }

            // If the player right clicks, cancels their current placement type in favour of none
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                CancelCurrentPlacementType();
            }
        }


        // Selection Management
        public void CancelSelectionBox()
        {
            selectionStartPoint = Vector2.zero;
            selectionBox.gameObject.SetActive(false);
        }    

        public void UpdateSelectionBox()
        {
            // If the player clicks, creates a selection region sprite (Doesn't start the selection if hovering over an EventSystem object)
            if (Input.GetKeyDown(KeyCode.Mouse0) && !UtilityFuncs.IsScreenPosOverUIObject(Input.mousePosition))
            {
                selectionBox.gameObject.SetActive(true);
                selectionStartPoint = Input.mousePosition;
            }

            // If the player lets go of their click, clears the selection region and saves selected blocks
            else if (Input.GetKeyUp(KeyCode.Mouse0) && selectionStartPoint != Vector2.zero)
            {
                ClearSelectedBlocks();
                selectedBlocks.AddRange(GetBlocksInSelection());
                CancelSelectionBox();
            }

            // If the player is in the process of clicking and the selection region is active, updates its position
            else if (Input.GetKey(KeyCode.Mouse0) && selectionStartPoint != Vector2.zero)
            {
                // Updates size and position of the selection regionu
                Vector2 worldMousePos = Input.mousePosition;
                float width = (worldMousePos.x - selectionStartPoint.x)/ selectionBoxCanvas.scaleFactor;
                float height = (worldMousePos.y - selectionStartPoint.y) / selectionBoxCanvas.scaleFactor;

                selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
                selectionBox.position = selectionStartPoint + new Vector2(width/2, height/2)*selectionBoxCanvas.scaleFactor;
            }
        }

        public BlockBase[] GetBlocksInSelection()
        {
            // Creates a Box Collider in the area of the selection box
            Collider2D[] collidersInSelection = Physics2D.OverlapAreaAll(mainCam.ScreenToWorldPoint(selectionStartPoint), mainCam.ScreenToWorldPoint(Input.mousePosition));

            // Finds all BlockBase that were placed by the player and selects them
            List<BlockBase> blocksInSelection = new List<BlockBase>();

            foreach(Collider2D collider in collidersInSelection)
            {
                // Makes sure its a player-placed block before highlighting and selecting it
                BlockBase colliderBlock = collider.GetComponent<BlockBase>();

                if(IsPlayerBlock(colliderBlock))
                {
                    colliderBlock.currentMaterial = outlineSelectionMaterial;
                    blocksInSelection.Add(colliderBlock);
                }
            }

            // Returns the found blocks
            return blocksInSelection.ToArray();
        }

        public void ClearSelectedBlocks()
        {
            // Resets the highlight on selected blocks and clears the array
            foreach(BlockBase block in selectedBlocks)
            {
                block.currentMaterial = defaultSpriteMaterial;
            }

            selectedBlocks.Clear();
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

            // Deselects all blocks
            ClearSelectedBlocks();

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

        public bool IsPlayerBlock(BlockBase block)
        {
            return placedBlocks.Contains(block);
        }    


        // Hologram Management
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


        // On Click Functions
        public void AttemptSelectObject(BlockBase blockInfo, IPropertiesComponent selectableObject )
        {
            // If correct placement type, and it is placed by the player, selects the object
            if(currentPlacementType == PlacementType.None)
            {
                if (placedBlocks.Contains(blockInfo))
                { 
                    if(selectableObject != null)
                    {
                        selectionPanel.CloseSelectionBox();
                        blockInfo.ClickedOn();
                    }
                }
            } 
            
            else if (currentPlacementType == PlacementType.ModifyingSelection)
            {
                placementHologram.placementArea.layer = 2; //set placement area layer to no raycast
                if (selectionMoveBlock == blockInfo)
                {
                    currentPlacementType = PlacementType.None;
                }
                else
                {
                    currentPlacementType = PlacementType.None;
                    blockInfo.ClickedOn();
                }
            }
        }
        
        public void SelectionOpenMenu(BlockBase blockInfo, IPropertiesComponent selectableObject)
        {
            selectableObject.ActivateSelectionPanel(selectionPanel);
        }

        public void SelectionDragObject(BlockBase blockInfo, IPropertiesComponent selectableObject)
        {
            currentPlacementType = PlacementType.ModifyingSelection;
            selectionMoveBlock = blockInfo;
            placementHologram.placementArea.layer = 0;
            Debug.Log("drag object");
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

        public void MoveBlockAction(BlockBase block, Vector3 newPosition)
        {
            //TODO
        }

        public void RotateBlockAction(BlockBase block, Quaternion newRotation)
        {
            //TODO (NOTE: Consider that this might be done to blocks in a large selection in which case the player might want to rotate it around the center of the selected blocks)
        }

        public void AttemptMoveBlock(Transform objectTransform, BoxCollider2D objCollider, Vector3 placementPos)
        {
            // Gets nearby colliders
            Vector3 oldCoords = objectTransform.position;
            objectTransform.position = placementPos;
            Collider2D[] nearbyColliders = Physics2D.OverlapBoxAll(objectTransform.position, Vector2.Scale(objectTransform.lossyScale, objCollider.size), objectTransform.rotation.eulerAngles.z);
            
            // Stores checks for different conditions
            bool inPlacingArea = false;
            bool hasFoundOtherCollider = false;
            foreach (Collider2D collider in nearbyColliders)
            {
                // Checks if colliding w/ placing area
                if (collider.gameObject.CompareTag("PlacingArea"))
                {
                    inPlacingArea = true;
                }

                // Otherwise, ensures it's not colliding with other objects than itself
                else if (collider != objCollider)
                {
                    // Ensures the collider isn't a trigger
                    if (!collider.isTrigger)
                    {
                        hasFoundOtherCollider = true;
                    }  
                }
            }

            // reverts position if it collides with something
            if (hasFoundOtherCollider || !inPlacingArea)
            {
                objectTransform.position = oldCoords;
            }
            
            objectTransform.GetComponent<BlockBase>().UpdateOriginalTransform();
        
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