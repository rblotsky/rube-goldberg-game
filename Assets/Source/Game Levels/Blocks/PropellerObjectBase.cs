using UnityEngine;
using System.Collections.Generic;

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
            //TODO
            return null;
        }

        public void LoadProperties(string[] propertyStrings)
        {
            //TODO
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
        }
    }
}