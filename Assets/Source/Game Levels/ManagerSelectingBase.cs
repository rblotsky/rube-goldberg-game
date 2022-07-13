using System.Collections.Generic;
using UnityEngine;

namespace RubeGoldbergGame
{
    public class ManagerSelectingBase : MonoBehaviour
    {
        public List<GameObject> allPlacedObjects = new List<GameObject>();
        public List<GameObject> selectedObjects = new List<GameObject>();

        private bool nextClickIsMovingSelection = false;

        private EditorBlockPlacingManager blockManager;

        private void Awake()
        {
            blockManager = FindObjectOfType<EditorBlockPlacingManager>();
        }

        public void DeselectAllSelectedObjects()
        {
            foreach (var obj in selectedObjects)
            {
                obj.GetComponent<BlockBase>().UpdateSelectionStatusForThisObject(false);
            }
            selectedObjects.Clear();
        }
        
    }
}