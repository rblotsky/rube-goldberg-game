using System;
using System.Collections.Generic;
using UnityEngine;

namespace RubeGoldbergGame
{
    public class SelectionMove : MonoBehaviour
    {
        public ManagerSelectingBase baseSel;
        private Camera mainCam;
        public List<Vector3> mouseOffsets = new List<Vector3>();
        private void Start()
        {
            mainCam = Camera.main;
            this.enabled = false;
        }

        public void OnEnable()
        {
            baseSel = ManagerSelectingBase.SelectingManagerInstance;
            foreach (var obj in baseSel.selectedObjects)
            {
                
                mouseOffsets.Add(obj.transform.position - Vector3.Scale(mainCam.ScreenToWorldPoint(Input.mousePosition),
                    (new Vector3(1, 1, 0))));
            }
        }

        private void Update()
        {
            for (int i = 0; i < baseSel.selectedObjects.Count; i++)
            {
                Vector3 mouseCoords =
                    Vector3.Scale(mainCam.ScreenToWorldPoint(Input.mousePosition), (new Vector3(1, 1, 0)));
                baseSel.selectedObjects[i].transform.position = mouseCoords + mouseOffsets[i];
            }

            if (Input.GetMouseButtonUp(0))
            {
                mouseOffsets.Clear();
                this.enabled = false;
            }
        }
    }
}