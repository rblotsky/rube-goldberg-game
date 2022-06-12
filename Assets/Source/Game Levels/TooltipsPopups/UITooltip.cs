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

        // Data
        public float ROOT_POSITION_OFFSET = 10f;


        // FUNCTIONS //
        // Basic Functions
        private void OnEnable()
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
            rectTransform.position = UtilityFuncs.ClampElementToCanvas(rectTransform, mainCanvas, rootPosition);

            // Dirties layout so it's immediately updated
            textBox.SetLayoutDirty();
        }      
    }
}