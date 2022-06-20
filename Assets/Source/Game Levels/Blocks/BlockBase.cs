using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using Unity.VisualScripting;

namespace RubeGoldbergGame 
{
    [DisallowMultipleComponent]
    public class BlockBase : TooltipComponent, IPointerClickHandler, IPointerDownHandler
    {
        // DATA //
        // Description Data
        public string displayName;
        public string displayDescription;
        public Sprite displaySprite;

        // Cached Data
        private EditorBlockPlacingManager blockPlacingManager;
        private IPropertiesComponent propertiesComponent;

        public Vector3 originalPosition;
        public Quaternion originalRotation;

        // Constants
        public static readonly char VECTOR3_SEP_CHAR = ':';


        private float durationSelected = 0;
        public bool isClickedOn = false;
        public float durationSelectClick = 0.2f;

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
            propertiesComponent = GetComponent<IPropertiesComponent>();
            originalPosition = gameObject.transform.position;
            originalRotation = gameObject.transform.rotation;
        }
        public void Update()
        {
            base.Update();
            if (isClickedOn)
            {
                durationSelected += Time.unscaledDeltaTime;
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (isUserHovering && isClickedOn)
                {
                    Debug.Log(durationSelected);
                    if (durationSelected < durationSelectClick)
                    {
                        blockPlacingManager.SelectionOpenMenu(this, propertiesComponent);
                    }
                    else
                    {
                        blockPlacingManager.SelectionDragObject(this, propertiesComponent);
                    }
                }
                isClickedOn = false;
            }
        }
        
        
        // Data Saving Functions
        public string SaveBlockData()
        {
            // Saves all the data as a csv file: 
            // BlockName,Position,Rotation,...Properties
            string csvString = "";
            csvString += displayName;
            csvString += ",";
            csvString += UtilityFuncs.SaveVector3ToString(transform.position).Replace(',', VECTOR3_SEP_CHAR);
            csvString += ",";
            csvString += UtilityFuncs.SaveVector3ToString(transform.rotation.eulerAngles).Replace(',', VECTOR3_SEP_CHAR);
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
                // Copies the data after Rotation to a new list and loads the propertiesComponent from that data
                List<string> propertiesComponentData = new List<string>();
                for(int i = 3; i < dataArray.Length; i++)
                {
                    propertiesComponentData.Add(dataArray[i]);
                }
                propertiesComponent.LoadProperties(propertiesComponentData.ToArray());
            }
        }


        // Interface Functions
        public void OnPointerClick(PointerEventData pointerData)
        {
            // Tries selecting this object
            
            Debug.Log("I was clicked on");
        }
        
        //detecting mouse down and sending event function
        public void OnPointerDown(PointerEventData eventData)
        {
            IPropertiesComponent childSelectable = GetComponentInChildren<IPropertiesComponent>();
            blockPlacingManager.AttemptSelectObject(this, childSelectable); 
            Debug.Log("MBD");
        }
        
        //confirmation function
        public void ClickedOn()
        {
            durationSelected = 0f;
            isClickedOn = true;
        }

        //TODO reset positions
        public void SimulationResetPos()
        {
            Debug.Log("reset pos");
            TransformHelper.SetTransform(gameObject.transform, originalPosition, originalRotation);
        }

        public void updateTransform()
        {
            originalRotation = gameObject.transform.rotation;
            originalPosition = transform.position;
        }

        
    }
}
