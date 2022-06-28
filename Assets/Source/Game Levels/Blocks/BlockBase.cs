using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.UI;

namespace RubeGoldbergGame 
{
    [DisallowMultipleComponent]
    public class BlockBase : TooltipComponent
    {
        // DATA //
        // Description Data
        public string displayName;
        public string displayDescription;
        public Sprite displaySprite;



        // Cached Data
        public EditorBlockPlacingManager blockPlacingManager;
        public IPropertiesComponent propertiesComponent;
        public ObjectSelectionBase objectSelectionManager;

        public bool isClickedOn = false;
        public bool isBeingMoved = false;
        private Vector3 originalPosition;
        private Quaternion originalRotation;
        private LevelManager levelManager;

        // Constants
        public static readonly char VECTOR3_SEP_CHAR = ':';


        // FUNCTIONS //
        // Non-Override Unity Defaults
        private void OnEnable()
        {
            levelManager.onSimulationStart += OnSimulationStart;
        }

        private void OnDisable()
        {
            levelManager.onSimulationStart -= OnSimulationStart;
        }


        // Override Functions
        public override string GetTooltipText()
        {
            return LanguageManager.TranslateFromEnglish(displayName + "\n" + displayDescription);
        }

        protected override void Awake()
        {
            // Runs base awake
            base.Awake();

            // Gets scene references
            levelManager = FindObjectOfType<LevelManager>(true);
            blockPlacingManager = FindObjectOfType<EditorBlockPlacingManager>(true);
            propertiesComponent = GetComponent<IPropertiesComponent>();
            objectSelectionManager = GetComponent<ObjectSelectionBase>();
            originalPosition = gameObject.transform.position;
            originalRotation = gameObject.transform.rotation;
        }

        protected override void Update()
        {
            // Runs base Update
            base.Update();
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


        // Events
        public void OnSimulationStart(LevelManager levelManagerUsed)
        {
            // Saves its current position and rotation to reset when the sim finishes
            originalPosition = transform.position;
            originalRotation = transform.rotation;
        }

        
        
        
        //confirmation function
        public void ClickedOn()
        {
            try
            {
                objectSelectionManager.durationSelected = 0f;
            }
            catch (Exception e)
            {
                Console.WriteLine(e + ": the object shouldn't be clicked!");
                
                throw;
            }
            
            isClickedOn = true;
        }


        // Simulation Management
        public void SimulationResetPos()
        {
            Debug.Log("reset pos");
            TransformHelper.SetTransform(transform, originalPosition, originalRotation);
            Rigidbody2D attachedRigidbody = gameObject.GetComponent<Rigidbody2D>();
            if (attachedRigidbody != null)
            {
                attachedRigidbody.velocity = Vector2.zero;
                attachedRigidbody.angularVelocity = 0f;
            }
            
        }

        public void updateTransform()
        {
            originalRotation = gameObject.transform.rotation;
            originalPosition = transform.position;
        }

        
    }
}
