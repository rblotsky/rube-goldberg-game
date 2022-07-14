using System;
using System.Collections.Generic;
using UnityEngine;

namespace RubeGoldbergGame
{
    public class ManagerMoveSelection : MonoBehaviour
    {
        public ManagerSelectingBase baseSel;
        private Camera mainCam;

        private Grid placementGrid;
        public List<Vector3> mouseOffsets = new List<Vector3>();
        private List<Vector3> oldPositions = new List<Vector3>();
        private void Start()
        {
            mainCam = Camera.main;
            this.enabled = false;
            placementGrid = FindObjectOfType<Grid>(true);
        }

        public void OnEnable()
        {
            baseSel = ManagerSelectingBase.SelectingManagerInstance;
            foreach (var obj in baseSel.selectedObjects)
            {
                Vector3 position = obj.transform.position;
                mouseOffsets.Add(position - Vector3.Scale(mainCam.ScreenToWorldPoint(Input.mousePosition),
                    (new Vector3(1, 1, 0))));
                oldPositions.Add(position);
            }
        }

        private void Update()
        {
            Vector3 mouseCoords =
                Vector3.Scale(mainCam.ScreenToWorldPoint(Input.mousePosition), (new Vector3(1, 1, 0)));

            bool canMoveBlocks = true;
            
            //moving each block
            for (int i = 0; i < baseSel.selectedObjects.Count; i++)
            {
                //record old positions
                oldPositions[i] = baseSel.selectedObjects[i].transform.position;
                
                //move each block to new pos, record when we cannot move the block
                if(!baseSel.selectedObjects[i].GetComponent<BlockBase>().RunBlockMove(
                       UtilityFuncs.SnapToGrid(mouseCoords + mouseOffsets[i], placementGrid)))
                {
                    canMoveBlocks = false;
                }
                
                //baseSel.selectedObjects[i].transform.position = mouseCoords + mouseOffsets[i];
            }
            
            //move all blocks back to old position if we cannot move
            if (!canMoveBlocks)
            {
                for (int i = 0; i < baseSel.selectedObjects.Count; i++)
                {
                    baseSel.selectedObjects[i].transform.position = oldPositions[i];
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                mouseOffsets.Clear();
                this.enabled = false;
            }
        }
    }
}