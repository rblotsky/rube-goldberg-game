using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace RubeGoldbergGame
{
    public class UISelectionBox : MonoBehaviour
    {
        // DATA //
        // References
        public GameObject propertyPrefab;
        public TextMeshProUGUI objectNameText;
        public List<UISliderProperty> properties = new List<UISliderProperty>();

        // Cached Data
        private Canvas objCanvas;
        private RectTransform objRectTransform;


        // FUNCTIONS //
        // Unity Defaults
        private void Awake()
        {
            objRectTransform = GetComponent<RectTransform>();
            objCanvas = GetComponentInParent<Canvas>();
        }


        // Management Functions
        public void SetupSelectionBox(string objectName, Vector3 displayPosition)
        {
            // Opens UI
            gameObject.SetActive(true);

            // Updates title and position
            objectNameText.SetText(objectName);
            objRectTransform.position = UtilityFuncs.ClampElementToCanvas(objRectTransform, objCanvas, displayPosition);
        }

        public void AddProperty(string propName, float min, float max, float currentVal, bool onlyInts, FloatValueDelegate onValChangeFunc)
        {
            UISliderProperty newProperty = Instantiate(propertyPrefab, transform).GetComponent<UISliderProperty>();
            newProperty.SetupProperty(propName, currentVal, min, max, onlyInts, onValChangeFunc);
            properties.Add(newProperty);
        }

        public void CloseSelectionBox()
        {
            // Disables and destroys all the property objects
            foreach(UISliderProperty property in properties)
            {
                property.DisconnectProperty();
                Destroy(property.gameObject);
            }

            properties.Clear();

            // Closes UI
            gameObject.SetActive(false);
        }
    }
}