using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RubeGoldbergGame
{
    public class UITooltip : MonoBehaviour
    {
        // DATA //
        // References
        public TextMeshProUGUI textBox;
        public LayoutGroup layout;
        public RectTransform rectTransform;
        public Canvas mainCanvas;
        public RectTransform canvasRectTransform;

        // Data
        public float ROOT_POSITION_OFFSET = 10f;


        // FUNCTIONS //
        // Basic Functions
        private void Awake()
        {
            Setup();
        }


        // External Management
        public void Setup()
        {
            layout = GetComponent<LayoutGroup>();
            rectTransform = GetComponent<RectTransform>();
            mainCanvas = GetComponentInParent<Canvas>();
        }

        public void UpdateText(string newText)
        {
            // Updates the text box
            textBox.SetText(newText);
            textBox.SetLayoutDirty();
        }

        public void UpdatePosition(Vector3 rootPosition)
        {
            // Moves to a slight offset
            rootPosition.x -= (2f * (rectTransform.pivot.x - 0.5f)) * ROOT_POSITION_OFFSET;
            rootPosition.y -= (2f * (rectTransform.pivot.y - 0.5f)) * ROOT_POSITION_OFFSET;

            // Clamps to stay on canvas
            rectTransform.position = ClampElementToCanvas(rectTransform, mainCanvas, rootPosition);
            textBox.SetLayoutDirty();
        }


        // Static Functions
        public static Vector3 ClampElementToCanvas(RectTransform element, Canvas canvas, Vector3 newPos)
        {
            float minX = (element.rect.size.x * canvas.scaleFactor * element.pivot.x);
            float maxX = (canvas.pixelRect.size.x - (element.rect.size.x * canvas.scaleFactor * element.pivot.x));
            float minY = (element.rect.size.y * canvas.scaleFactor * element.pivot.y);
            float maxY = (canvas.pixelRect.size.y - (element.rect.size.y * canvas.scaleFactor * Mathf.Abs(element.pivot.y - 1)));

            newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
            newPos.y = Mathf.Clamp(newPos.y, minY, maxY);

            // Sets position and dirties layout
            return newPos;
        }
    }
}