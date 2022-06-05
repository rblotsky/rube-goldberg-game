using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RubeGoldbergGame 
{
    [DisallowMultipleComponent]
    public class BlockBase : TooltipComponent, IPointerClickHandler
    {
        // DATA //
        // Description Data
        public string displayName;
        public string displayDescription;
        public Sprite displaySprite;

        // Cached Data
        private EditorBlockPlacingManager blockPlacingManager;
        private IPropertiesComponent propertiesComponent;


        // FUNCTIONS //
        // Override Functions
        protected override string GetTooltipText()
        {
            return displayName + "\n" + displayDescription;
        }

        protected override void Awake()
        {
            // Runs base awake
            base.Awake();

            // Gets scene references
            blockPlacingManager = FindObjectOfType<EditorBlockPlacingManager>(true);
            propertiesComponent = GetComponentInChildren<IPropertiesComponent>(true);
        }


        // Data Saving Functions
        public string SaveBlockData()
        {
            // Saves all the data as a csv file: 
            // BlockName,Position,Rotation,...Properties
            string csvString = "";
            csvString += name;
            csvString += ",";
            csvString += transform.position.ToString();
            csvString += ",";
            csvString += transform.rotation.eulerAngles.ToString();
            csvString += ",";

            // Saves the IPropertiesComponent component of this block too
            if(propertiesComponent != null)
            {
                csvString += propertiesComponent.SaveProperties();
            }

            // Returns the csv string
            return csvString;
        }

        public void LoadBlockData(string[] dataArray)
        {
           //TODO: Load data, then load the property objects
        }


        // Interface Functions
        public void OnPointerClick(PointerEventData pointerData)
        {
            // Tries selecting this object
            IPropertiesComponent childSelectable = GetComponentInChildren<IPropertiesComponent>();
            blockPlacingManager.AttemptSelectObject(this, childSelectable);  
        }

    }
}
