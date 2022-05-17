using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RubeGoldbergGame
{
    public class LevelManager : MonoBehaviour
    {
        // DATA //
        // Scene References
        public LevelUIManager interfaceManager;
        public MovableObject objectiveObject;
        public PlacingHologram placementHologram;

        // Simulation Management
        private static int[] simSpeedPercentages = { 0, 25, 50, 100, 200, 300, 400 };

        // Level Management
        public int levelID;
        public int blockPlaceRotationAmount = 15;
        public BlockBase[] availableBlocks;
        public LevelData levelData;
        public List<BlockBase> placedBlocks = new List<BlockBase>();

        // Cached Data

        private int numBlocksUsed;
        private int currentSimSpeedIndex = 3;
        public bool inSimulation;
        private Camera mainCam;
        private BlockBase currentPlacementBlock = null;


        // FUNCTIONS //
        // Unity Defaults
        private void Awake()
        {
            // Gets level references
            interfaceManager = FindObjectOfType<LevelUIManager>(true);
            mainCam = Camera.main;

            // Gets level data
            levelData = GlobalData.GetLevel(levelID);

            // Sets base UI according to level data
            interfaceManager.SetBasicInterface(levelData);

            // Updates number of blocks used
            numBlocksUsed = 0;
        }

        private void Start()
        {
            // Updates UI to have the required block slots
            interfaceManager.placeablesButtons.GenerateBlockSlots(availableBlocks);

            // Enters editor mode
            ToggleSimulationMode(false);
        }

        private void Update()
        {
            
            //only runs while in editor mode to prevent placing invisible things during runtime
            //and not run function while in runtime
            if (!inSimulation)
            {
                
                //UpdatePlacingHologram();
            }
        }


        // Level Events
        public void CompleteLevel()
        {
            interfaceManager.ToggleCompletionUI(true);
        }


        // Level Management
        public void UpdatePlacingHologram()
        {
            // If in simulation, makes transparent
            placementHologram.ToggleHologram(!inSimulation);

            // Gets mouse and placement positions
            Vector3 mousePos = Input.mousePosition;
            Vector3 placementPos = Vector3.Scale(mainCam.ScreenToWorldPoint(mousePos), (new Vector3(1, 1, 0)));

            // Moves hologram
            placementHologram.UpdatePosition(placementPos);

            // If current block is null, deletes the block pointed at
            if (currentPlacementBlock == null && Input.GetKeyDown(KeyCode.Mouse0))
            {
                // Resets rotation to default
                placementHologram.ResetRotation();

                // Uses a 2D raycast to get the block the player is pointing at
                Ray selectionRay = mainCam.ScreenPointToRay(mousePos);
                RaycastHit2D hitInfo = Physics2D.Raycast(selectionRay.origin, selectionRay.direction);

                if (hitInfo.collider != null)
                {
                    // Gets the block
                    BlockBase hitBlock = hitInfo.collider.GetComponent<BlockBase>();

                    // If it is placed by the player, deletes it and removes it from placed blocks list
                    if (placedBlocks.Contains(hitBlock))
                    {
                        Destroy(hitBlock.gameObject);
                        placedBlocks.Remove(hitBlock);
                        Debug.Log("Deleted a block!");
                    }
                }
            }

            // If block is not null, tries placing it.
            else
            {
                // Updates hologram position and colour
                placementHologram.UpdateCanPlace();
                placementHologram.UpdateColour();

                // Rotates 15 degrees clockwise if needed
                if (Input.GetKeyDown(KeyCode.R))
                {
                    placementHologram.RotateClockwise(blockPlaceRotationAmount);
                }

                // If player clicks, either places or deletes.
                if (Input.GetKeyDown(KeyCode.Mouse0) && placementHologram.CanPlaceObject)
                {
                    BlockBase placedBlock = Instantiate(currentPlacementBlock, placementPos, placementHologram.transform.rotation).GetComponent<BlockBase>();
                    placedBlocks.Add(placedBlock);
                }
            }

        }

        public void RefreshTimescale()
        {
            // Updates timescale depending on whether the game is in simulation or editor mode
            if (inSimulation)
            {
                Time.timeScale = 1f * (simSpeedPercentages[currentSimSpeedIndex] / 100f);
            }

            else
            {
                Time.timeScale = 0;
            }
        }


        // UI Events
        public void ToggleSimulationMode(bool inSimMode)
        {
            // Resets all the placed objects to correct positions
            objectiveObject.ResetToInitialValues();
            TogglePlaceableObjects(inSimMode);

            // Toggles UI
            interfaceManager.ToggleSimulationUI(inSimMode);
            interfaceManager.UpdateSimSpeedText(simSpeedPercentages[currentSimSpeedIndex]);

            // Updates cached status
            inSimulation = inSimMode;

            // Refreshes timescale
            RefreshTimescale();
        }

        private void TogglePlaceableObjects(bool inSimMode)
        {
            foreach (BlockBase block in placedBlocks)
            {
                block.ToggleTriggerArea(inSimMode);
            }
        }

        public void UpdateSimSpeedIndex(int changeAmount)
        {
            // Updates which simulation speed is being used
            currentSimSpeedIndex = Mathf.Clamp(currentSimSpeedIndex + changeAmount, 0, simSpeedPercentages.Length - 1);

            // Updates UI text
            interfaceManager.UpdateSimSpeedText(simSpeedPercentages[currentSimSpeedIndex]);

            // Refreshes timescale
            RefreshTimescale();
        }

        public void SetHologramToBlock(BlockBase selectedBlock)
        {
            // Gets the sprite on the selected block
            Sprite displaySprite = selectedBlock.GetComponent<SpriteRenderer>().sprite;

            // Updates display sprite
            BoxCollider2D blockCollider = selectedBlock.GetComponent<BoxCollider2D>();
            placementHologram.UpdateSprite(displaySprite, blockCollider);

            // Updates which block is used
            currentPlacementBlock = selectedBlock;
        }

        public void SetHologramToDeletion(Sprite displaySprite)
        {
            // Updates the current block to null and the hologram to the given sprite
            placementHologram.UpdateSprite(displaySprite, null);
            currentPlacementBlock = null;
        }

    }
}