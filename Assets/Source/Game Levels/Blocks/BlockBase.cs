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

            // Gets all the ISaveableProperty components in this object, saves them too.


            // Returns the csv string
            return csvString;

        }

        public void LoadBlockData(string[] dataArray)
        {
           
        }


        // Interface Functions
        public void OnPointerClick(PointerEventData pointerData)
        {
            // Tries selecting this object
            ISelectableObject childSelectable = GetComponentInChildren<ISelectableObject>();
            blockPlacingManager.AttemptSelectObject(this, childSelectable);  
        }

    }
}
