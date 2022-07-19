using System;
using UnityEngine;

namespace RubeGoldbergGame
{
    public class ManagerDragSelect : MonoBehaviour
    {
        public Vector2 startPos;
        public Vector2 endPos;
        private Camera mainCam;

        public RectTransform boxVisual;

        private void Start()
        {
            mainCam = Camera.main;
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            startPos = Vector3.Scale(mainCam.ScreenToWorldPoint(Input.mousePosition), (new Vector3(1, 1, 0)));
            endPos = startPos;
            drawBox();
        }

        private void Update()
        {
            endPos = Vector3.Scale(mainCam.ScreenToWorldPoint(Input.mousePosition), (new Vector3(1, 1, 0)));
            drawBox();
            if (Input.GetMouseButtonUp(0))
            {
                int count = 0;
                foreach (var coll in Physics2D.OverlapBoxAll(transform.position,
                             Vector2.Scale(transform.lossyScale, GetComponent<BoxCollider2D>().size),
                             transform.rotation.eulerAngles.z))
                {
                    if (coll.tag == "PlaceableBlock")
                    {
                        count++;
                        ManagerSelectingBase.SelectingManagerInstance.AddSelectionToList(coll.gameObject);
                    }
                    
                }

                if (count == 0)
                {
                    ManagerSelectingBase.SelectingManagerInstance.nextClickIsMovingSelection = false;
                }
                gameObject.SetActive(false);
            }
        }

        private void drawBox()
        {
            Vector3 boxCenter = (startPos + endPos) / 2;
            transform.position = boxCenter;

            Vector2 boxSize = new Vector2(Mathf.Abs(startPos.x - startPos.y), Mathf.Abs(endPos.x - endPos.y));

            transform.localScale = new Vector3(Mathf.Abs(startPos.x - endPos.x), Mathf.Abs(startPos.y - endPos.y), 1);

        }
    }
}