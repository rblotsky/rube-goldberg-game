using System;
using System.Collections.Generic;
using UnityEngine;

namespace RubeGoldbergGame
{
    public class ManagerSelectingBase : MonoBehaviour
    {
        public List<GameObject> allPlacedObjects = new List<GameObject>();
        public List<GameObject> selectedObjects = new List<GameObject>();

        private bool nextClickIsMovingSelection = false;
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
        public Material propertiesMaterial;

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

        private void Update()
        {
            if (isClick)
            {
                durationOfClick += Time.unscaledDeltaTime;
                if (Input.GetMouseButtonUp(0))
                {
                    isClick = false;
                    if (durationOfClick > durationOfSelectingClick && !Input.GetKey(KeyCode.LeftControl))
                    {
                        selectedObjects[selectedObjects.Count - 1].GetComponent<IPropertiesComponent>().ActivateSelectionPanel(blockManager.selectionPanel);
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
            if (Input.GetKey(KeyCode.LeftControl) && !nextClickIsMovingSelection && selectedObjects.Contains(objectClickedOn))
            {
                RemoveSelectionFromList(objectClickedOn);
                return;
            }
            
            if (objectClickedOn == backgroundHitbox)
            {
                Debug.Log("activate drag box");
                dragScript.gameObject.SetActive(true);
            }

            if (allPlacedObjects.Contains(objectClickedOn))
            {
                AddSelectionToList(objectClickedOn);
                if (!Input.GetKey(KeyCode.LeftControl))
                {
                    moveScript.enabled = true;
                    nextClickIsMovingSelection = false;
                }
                else
                {
                    isClick = true;
                    durationOfClick = 0f;
                }
            }
        }

        private void OnDisable()
        {
            DeselectAllSelectedObjects();
        }

        // removes all objects from selected objects list
        public void DeselectAllSelectedObjects()
        {
            foreach (var obj in selectedObjects)
            {
                obj.GetComponent<BlockBase>().UpdateSelectionStatusForThisObject(false);
            }
            selectedObjects.Clear();
        }

        public void AddSelectionToList(GameObject selectedObject)
        {
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