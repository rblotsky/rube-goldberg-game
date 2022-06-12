using System;
using UnityEngine;

namespace RubeGoldbergGame
{
    public class SpringObjectBase : MonoBehaviour, IPropertiesComponent
    {
        // DATA //
        // References
        public BlockBase objectBaseData;

        // Modifiable Properties
        public float launchStrMin = 0;
        public float launchStrMax = 20;
        public float rotationMin = 0;
        public float rotationMax = 360;

        // References
        public ObjectSpringPlate attachedRegion;
        
        public void ActivateSelectionPanel(UISelectionBox selectionPanel)
        {
            selectionPanel.SetupSelectionBox(objectBaseData.displayName, Input.mousePosition);

            // Adds all its properties
            selectionPanel.AddProperty("Strength", launchStrMin, launchStrMax, attachedRegion.pushForce, true, UpdateStrengthProperty);
            selectionPanel.AddProperty("Rotation", rotationMin, rotationMax, objectBaseData.transform.eulerAngles.z, true, UpdateRotationProperty);
        }

        public string SaveProperties()
        {
            return attachedRegion.pushForce.ToString();
        }

        public void LoadProperties(string[] propertyStrings)
        {
            try
            {
                attachedRegion.pushForce = float.Parse(propertyStrings[0]);
            }
            catch(FormatException error)
            {
                Debug.Log(string.Format("Failed to load the push strength of object \"{0}\"! ({1})", name, error.Message));
                attachedRegion.pushForce = launchStrMax;
            }
        }
        
        
        public void UpdateStrengthProperty(float newVal)
        {
            attachedRegion.pushForce = newVal;
        }

        public void UpdateRotationProperty(float newRotation)
        {
            Vector3 rotationEulers = objectBaseData.transform.eulerAngles;
            objectBaseData.transform.eulerAngles = new Vector3(rotationEulers.x, rotationEulers.y, newRotation);
        }
    }
}