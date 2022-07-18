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
        private GameObject backgroundHitbox;

        public float durationOfSelectingClick = 0.2f;

        private static ManagerSelectingBase _selectingManagerInstance;

        public static ManagerSelectingBase SelectingManagerInstance
        {
            get => _selectingManagerInstance;
            set => _selectingManagerInstance = value;
        }

        private EditorBlockPlacingManager blockManager;
        private ManagerMoveSelection moveScript;
        private ManagerDragSelect dragScript;

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
        }

        /**
         * When an object is clicked on this function will be called
         */
        public void ObjectClickedOn(GameObject objectClickedOn)
        {
            if (!Input.GetKey(KeyCode.LeftControl) && !nextClickIsMovingSelection)
            {
                DeselectAllSelectedObjects();
            }

            if (objectClickedOn == backgroundHitbox)
            {
                dragScript.enabled = true;
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
        }

        public void AddNewObjectToGlobalList(GameObject newObject)
        {
            allPlacedObjects.Add(newObject);
        }
        
        public void RemoveSelectionFromList(GameObject selectedObject)
        {
            selectedObjects.Remove(selectedObject);
            selectedObject.GetComponent<BlockBase>().UpdateSelectionStatusForThisObject(false);
        }
    }
}