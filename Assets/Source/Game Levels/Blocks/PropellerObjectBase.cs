using UnityEngine;
using System.Collections.Generic;
using System;

namespace RubeGoldbergGame
{
    public class PropellerObjectBase : MonoBehaviour, IPropertiesComponent
    {
        // DATA //
        // References
        public BlockBase objectBaseData;

        // Modifiable Properties
        public float pushStrMin = 0;
        public float pushStrMax = 20;
        public float rotationMin = 0;
        public float rotationMax = 360;

        // References
        public PropellerObjectPushingRegion attachedRegion;

        // FUNCTIONS //
        // Unity Defaults
        private void Awake()
        {
            objectBaseData = GetComponent<BlockBase>();
        }


        // Interface Functions
        public void ActivateSelectionPanel(UISelectionBox selectionPanel)
        {
            // Opens the panel
            selectionPanel.SetupSelectionBox(objectBaseData.displayName, Input.mousePosition);

            // Adds all its properties
            selectionPanel.AddProperty("Strength", pushStrMin, pushStrMax, attachedRegion.pushStrength, true, UpdateStrengthProperty);
            selectionPanel.AddProperty("Rotation", rotationMin, rotationMax, objectBaseData.transform.eulerAngles.z, true, UpdateRotationProperty);
        }

        public string SaveProperties()
        {
            // Saves the properties
            return attachedRegion.pushStrength.ToString();
        }

        public void LoadProperties(string[] propertyStrings)
        {
            // Loads the push strength
            try
            {
                attachedRegion.pushStrength = float.Parse(propertyStrings[0]);
            }
            catch(FormatException error)
            {
                Debug.Log(string.Format("Failed to load the push strength of object \"{0}\"! ({1})", name, error.Message));
                attachedRegion.pushStrength = pushStrMax;
            }
        }

        
        // Property Modification Functions
        public void UpdateStrengthProperty(float newVal)
        {
            attachedRegion.pushStrength = newVal;
        }

        public void UpdateRotationProperty(float newRotation)
        {
            Vector3 rotationEulers = objectBaseData.transform.eulerAngles;
            objectBaseData.transform.eulerAngles = new Vector3(rotationEulers.x, rotationEulers.y, newRotation);
            objectBaseData.updateTransform();
        }
    }
}