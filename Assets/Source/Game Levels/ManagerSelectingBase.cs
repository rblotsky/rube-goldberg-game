using System;
using System.Collections.Generic;
using UnityEngine;

namespace RubeGoldbergGame
{
    public class ManagerSelectingBase : MonoBehaviour
    {
        public List<GameObject> allPlacedObjects = new List<GameObject>();
        public List<GameObject> selectedObjects = new List<GameObject>();

        private ManagerSelectingBase moveScript;

        private bool nextClickIsMovingSelection = false;
        private bool backgroundHitbox;

        private static ManagerSelectingBase _selectingManagerInstance;

        public static ManagerSelectingBase SelectingManagerInstance
        {
            get => _selectingManagerInstance;
            set => _selectingManagerInstance = value;
        }

        private EditorBlockPlacingManager blockManager;

        private void Awake()
        {
            blockManager = FindObjectOfType<EditorBlockPlacingManager>();
            moveScript = FindObjectOfType<ManagerSelectingBase>();
            
            if (_selectingManagerInstance != null && _selectingManagerInstance != this)
            {
                Destroy(this);
            }

            SelectingManagerInstance = this;
        }
        
        public void ObjectClickedOn(GameObject objectClickedOn)
        {
            if (!Input.GetKey(KeyCode.LeftControl) && !nextClickIsMovingSelection)
            {
                DeselectAllSelectedObjects();
            }

            if (objectClickedOn == backgroundHitbox)
            {
                //drag box script
            }

            if (allPlacedObjects.Contains(objectClickedOn))
            {
                AddSelectionToList(objectClickedOn);
                if (!Input.GetKey(KeyCode.LeftControl))
                {
                    moveScript.enabled = true;
                    nextClickIsMovingSelection = false;
                }
            }
        }

        private void OnDisable()
        {
            DeselectAllSelectedObjects();
        }

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