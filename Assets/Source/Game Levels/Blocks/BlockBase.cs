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
    /**
     * usage: maximum of 1 BlockBase per object
     * each object can have multiple ObjectSelectionBase if required - the parent must have the block base
     */
    [DisallowMultipleComponent]
    public class BlockBase : TooltipComponent
    {
        // DATA //
        // Description Data
        public string displayName;
        public string displayDescription;
        public Sprite displaySprite;
        public bool connectToOthers;
        public bool hasMultipleSections = false;

        // Management Data
        public bool hasCustomPlacement = false;
        
        // Cached Data
        public EditorBlockPlacingManager blockPlacingManager;
        public IPropertiesComponent propertiesComponent;
        private ObjectSelectionBase objectSelectionManager;
        private BoxCollider2D objectCollider;
        public bool isClickedOn = false;
        public bool isBeingMoved = false;
        private Vector3 originalPosition;
        private Quaternion originalRotation;
        public LevelManager levelManager;
        private List<FixedJoint2D> attachedJoints = new List<FixedJoint2D>();
        private SpriteRenderer thisSpriteRenderer;
        private Vector2 dragMouseOffset;

        // Properties
        public Material currentMaterial
        {
            get { return thisSpriteRenderer.material; }
            set { thisSpriteRenderer.material = value; }
        }

        // Constants
        public static readonly char VECTOR3_SEP_CHAR = ':';

        // Hovering Data
        private int numPiecesHovered = 0;
        
        
        // FUNCTIONS //
        // Non-Override Unity Defaults
        private void OnEnable()
        {
            levelManager.onSimulationStart += OnSimulationStart;
            levelManager.onSimulationFinish += OnSimulationFinish;
        }

        private void OnDisable()
        {
            levelManager.onSimulationStart -= OnSimulationStart;
            levelManager.onSimulationFinish -= OnSimulationFinish;
        }
        
        // Refactor selection functions

        public void UpdateSelectionStatusForThisObject(bool status)
        {
            isClickedOn = status;
            if (status)
            {
                //TODO: change material
            }
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
            objectCollider = GetComponent<BoxCollider2D>();
            thisSpriteRenderer = GetComponent<SpriteRenderer>();
            ManagerSelectingBase.SelectingManagerInstance.AddNewObjectToGlobalList(gameObject);

            // Updates its default positions on awake
            UpdateOriginalTransform();
        }


        // Editor Functions
        public bool RunBlockMove(Vector2 snappedWorldMousePos)
        {
            // Moves to a new position
            Vector2 oldPos = transform.position;
            transform.position = snappedWorldMousePos + dragMouseOffset;

            // Resets to original if it can't be placed here
            if (!UtilityFuncs.GetCanPlaceBlock(gameObject, objectCollider))
            {
                transform.position = oldPos;
                return false;
            }

            // Returns true if it was able to be moved
            return true;
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

        public override void OnPointerEnter(PointerEventData pointerData)
        {
            if (hasMultipleSections)
            {
                if (numPiecesHovered == 0)
                {
                    base.OnPointerEnter(pointerData);
                }
                numPiecesHovered += 1;
            }
            else
            {
                base.OnPointerEnter(pointerData);
            }

        }

        public override void OnPointerExit(PointerEventData pointerData)
        {
            if (hasMultipleSections)
            {
                numPiecesHovered -= 1;
                if (numPiecesHovered == 0)
                {
                    base.OnPointerExit(pointerData);
                }

            }
            else
            {
                base.OnPointerExit(pointerData);
            }

        }


        // Events
        public void OnSimulationStart(LevelManager levelManagerUsed)
        {
            // Saves its current position and rotation to reset when the sim finishes
            UpdateOriginalTransform();

            if (connectToOthers)
            {
                // Generates fixed joints to all nearby blocks
                Collider2D[] adjacentColliders = Physics2D.OverlapBoxAll(transform.position, Vector2.Scale(transform.lossyScale, objectCollider.size), transform.rotation.eulerAngles.z);

                // For all the colliders that aren't itself and are BlockBase, adds a FixedJoint that connects to them
                foreach (Collider2D collider in adjacentColliders)
                {
                    Debug.DrawLine(transform.position, collider.transform.position, Color.red, 1);

                    if (collider.gameObject != gameObject && collider.GetComponent<BlockBase>() != null)
                    {
                        FixedJoint2D newJoint = gameObject.AddComponent<FixedJoint2D>();
                        newJoint.connectedBody = collider.attachedRigidbody;
                        attachedJoints.Add(newJoint);
                    }
                }
            }
        }

        public void OnSimulationFinish()
        {
            // Removes all FixedJoint components
            foreach(FixedJoint2D joint in attachedJoints)
            {
                Destroy(joint);
            }

            // Clears the list of joints
            attachedJoints.Clear();
        }

        

        // Simulation Management
        public void SimulationResetPos()
        {
            TransformHelper.SetTransform(transform, originalPosition, originalRotation);
            Rigidbody2D attachedRigidbody = gameObject.GetComponent<Rigidbody2D>();
            if (attachedRigidbody != null)
            {
                attachedRigidbody.velocity = Vector2.zero;
                attachedRigidbody.angularVelocity = 0f;
            }
            
        }

        public void UpdateOriginalTransform()
        {
            originalRotation = transform.rotation;
            originalPosition = transform.position;
        }
    }
}
