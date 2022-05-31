using UnityEngine;
using System.Collections.Generic;

namespace RubeGoldbergGame
{
    public class PropellerObjectBase : MonoBehaviour, ISelectableObject
    {
        // DATA //
        // References
        public BlockBase objectBaseData;

        // Modifiable Properties
        public float pushStrMin = 0;
        public float pushStrMax = 20;

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
            selectionPanel.AddProperty("Strength", pushStrMin, pushStrMax, attachedRegion.pushStrength, UpdateStrengthProperty);
        }

        
        // Property Modification Functions
        public void UpdateStrengthProperty(float newVal)
        {
            attachedRegion.pushStrength = newVal;
        }
    }
}