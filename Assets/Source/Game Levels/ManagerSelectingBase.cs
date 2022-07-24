using System;
using System.Collections.Generic;
using UnityEngine;

namespace RubeGoldbergGame
{
    public class ManagerSelectingBase : MonoBehaviour
    {
        public List<GameObject> allPlacedObjects = new List<GameObject>();
        public List<GameObject> selectedObjects = new List<GameObject>();

        public bool nextClickIsMovingSelection = false;
        private bool isClick = false;
        private float durationOfClick = 0f;
        public GameObject backgroundHitbox;

        public float durationOfSelectingClick = 0.2f;

        private static ManagerSelectingBase _selectingManagerInstance;

        public static ManagerSelectingBase SelectingManagerInstance
        {
            get => _selectingManagerInstance;
            set => _selectingManagerInstance = value;
        }

        private EditorBlockPlacingManager blockManager;
        public ManagerMoveSelection moveScript;
        public ManagerDragSelect dragScript;

        public Material defaultMaterial;
        public Material selectedMaterial;
        public Material invalidMaterial;

        private void Awake()
        {
            blockManager = FindObjectOfType<EditorBlockPlacingManager>();
            moveScript = FindObjectOfType<ManagerMoveSelection>();
            
            if (_selectingManagerInstance != null && _selectingManagerInstance != this)
            {
                Destroy(this);
            }

            SelectingManagerInstance = this;
        }
        
        private void OnDisable()
        {
            DeselectAllSelectedObjects();
        }

        private void Update()
        {
            if (isClick)
            {
                durationOfClick += Time.unscaledDeltaTime;
                if (Input.GetMouseButtonUp(0))
                {
                    Debug.Log("click released");
                    isClick = false;
                    if (durationOfClick < durationOfSelectingClick && !Input.GetKey(KeyCode.LeftControl))
                    {
                        try
                        {
                            selectedObjects[selectedObjects.Count - 1].GetComponent<IPropertiesComponent>()
                                .ActivateSelectionPanel(blockManager.selectionPanel);
                        }
                        catch (Exception e)
                        {
                            Debug.Log(e);
                        }
                    }
                }
            }
            
            if(Input.GetKeyDown(KeyCode.Backspace))
            {
                // Caches the blocks to delete list, since if we use the selected blocks list it will be modified during the loop and will crash the program.
                List<GameObject> blocksToDelete = new List<GameObject>(selectedObjects);
                foreach(GameObject block in blocksToDelete)
                {
                    selectedObjects.Remove(block);
                    Destroy(block);
                }
            }
        }

        /**
         * When an object is clicked on this function will be called
         */
        public void ObjectClickedOn(GameObject objectClickedOn)
        {
            Debug.Log(objectClickedOn);

            if (objectClickedOn == backgroundHitbox)
            {
                Debug.Log("activate drag box");
                if (!Input.GetKey(KeyCode.LeftControl))
                {
                    DeselectAllSelectedObjects();
                }
                
                
                //move all blocks if true, else activate drag script
                if (nextClickIsMovingSelection)
                {
                    nextClickIsMovingSelection = false;
                }
                else
                {
                    nextClickIsMovingSelection = true;
                    dragScript.gameObject.SetActive(true);
                }
               
            }
            
            if (allPlacedObjects.Contains(objectClickedOn))
            {
                isClick = true;
                durationOfClick = 0f;
                
                //user holds down control
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    if (selectedObjects.Contains(objectClickedOn))
                    {
                        RemoveSelectionFromList(objectClickedOn);
                    }
                    else
                    {
                        AddSelectionToList(objectClickedOn);
                        nextClickIsMovingSelection = (selectedObjects.Count > 1) ? true : false;
                    }

                    return;
                }
                
                //if the enxt click in not moving selection
                if (!nextClickIsMovingSelection)
                {
                    DeselectAllSelectedObjects();
                    AddSelectionToList(objectClickedOn);
                }
                
                moveScript.enabled = true;
                nextClickIsMovingSelection = false;
                
            }
        }

        // removes all objects from selected objects list
        public void DeselectAllSelectedObjects()
        {
            foreach (var obj in selectedObjects)
            {
                obj.GetComponent<BlockBase>().UpdateSelectionStatusForThisObject(false);
                obj.GetComponent<BlockBase>().currentMaterial = defaultMaterial;
            }
            selectedObjects.Clear();
        }

        /**
         * Adds an object to the list
         */
        public void AddSelectionToList(GameObject selectedObject)
        {
            if (selectedObjects.Contains(selectedObject))
            {
                return;
            }
            selectedObjects.Add(selectedObject);
            selectedObject.GetComponent<BlockBase>().UpdateSelectionStatusForThisObject(true);
            selectedObject.GetComponent<BlockBase>().currentMaterial = selectedMaterial;
        }

        public void AddNewObjectToGlobalList(GameObject newObject)
        {
            allPlacedObjects.Add(newObject);
        }
        
        public void RemoveSelectionFromList(GameObject selectedObject)
        {
            selectedObjects.Remove(selectedObject);
            selectedObject.GetComponent<BlockBase>().currentMaterial = defaultMaterial;
        }
    }
}