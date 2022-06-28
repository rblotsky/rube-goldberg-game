using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RubeGoldbergGame
{
    /*add this to any object that you want to be selectable
     
     REQUIREMENTS:
     BLOCKBASE
     IPROPERTIESCOMPONENT
    */
    public class ObjectSelectionBase : MonoBehaviour, IPointerDownHandler
    {
        //Data
        private BlockBase objectBase;
        public float durationSelected = 0; //currently is tracked separately
        
        // Other Block Data
        private float durationSelectClick = 0.2f;
        
        //Data accessors from block base
        private bool isClickedOn
        {
            get { return objectBase.isClickedOn;}
            set { objectBase.isClickedOn = value; }
        }
        
        private bool isBeingMoved
        {
            get { return objectBase.isBeingMoved;}
            set { objectBase.isBeingMoved = value; }
        }
        
        private bool isUserHovering
        {
            get { return objectBase.isUserHovering;}
            set { objectBase.isUserHovering = value; }
        }
        
        private EditorBlockPlacingManager blockPlacingManager
        {
            get { return objectBase.blockPlacingManager;}
        }

        private IPropertiesComponent propertiesComponent
        {
            get { return objectBase.propertiesComponent;}
        }
        
        

        private void Awake()
        {
            try
            {
                objectBase = (GetComponent<BlockBase>() == null)
                    ? GetComponentInParent<BlockBase>()
                    : GetComponent<BlockBase>();
                objectBase.hasMultipleSections = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            //objectBase = GetComponent<BlockBase>();
        }

        //detecting mouse down and sending event function
        public void OnPointerDown(PointerEventData eventData)
        {
            objectBase.blockPlacingManager.AttemptSelectObject(objectBase, propertiesComponent); 
            Debug.Log("MBD");
        }
        
        
        public void Update()
        {
            // If the object is currently clicked, increments the duration it's selected for
            if (isClickedOn)
            {
                durationSelected += Time.unscaledDeltaTime;
                if (!isBeingMoved && durationSelected > durationSelectClick)
                {
                    blockPlacingManager.SelectionDragObject(objectBase, propertiesComponent);
                    isBeingMoved = true;
                }
            }

            // If stops clicking, cancels is clicked and resets duration selected
            if (Input.GetMouseButtonUp(0))
            {
                if (isBeingMoved)
                {
                    isBeingMoved = false;
                    blockPlacingManager.AttemptSelectObject(objectBase, propertiesComponent); 
                }
                else if (isUserHovering && isClickedOn) //isBeingMoved => isClickedOn
                {
                    Debug.Log(durationSelected);
                    if (durationSelected < durationSelectClick)
                    {
                        blockPlacingManager.SelectionOpenMenu(objectBase, propertiesComponent);
                    }
                }

                isClickedOn = false;
            }
        }
    }
}