using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

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

        // Constants
        public static readonly char VECTOR3_SEP_CHAR = ':';


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
            csvString += transform.position.ToString().Replace(',', VECTOR3_SEP_CHAR);
            csvString += ",";
            csvString += transform.rotation.eulerAngles.ToString().Replace(',', VECTOR3_SEP_CHAR);
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
            // Data format: BlockName,Position,Rotation,...Properties
            transform.position = UtilityFuncs.ParseVector3(dataArray[1], VECTOR3_SEP_CHAR);
            transform.rotation = Quaternion.Euler(UtilityFuncs.ParseVector3(dataArray[2], VECTOR3_SEP_CHAR));

            // Loads IPropertiesComponent data
            if(propertiesComponent != null && dataArray.Length > 3)
            {
                // Copies the data after Rotation to a new array and loads the propertiesComponent from those items
                string[] propertiesComponentData = new string[dataArray.Length - 3];
                dataArray.CopyTo(propertiesComponentData, 3);
                propertiesComponent.LoadProperties(propertiesComponentData);
            }
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
