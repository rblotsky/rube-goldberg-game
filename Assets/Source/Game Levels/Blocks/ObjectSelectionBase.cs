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

    public class ObjectSelectionBase : MonoBehaviour
    {
        //Data
        private BlockBase objectBase;
        public float durationSelected = 0; //currently is tracked separately


        // Other Block Data
        private float durationSelectClick = 0.2f;

        //Data accessors from block base
        private bool isClickedOn
        {
            get { return objectBase.isClickedOn; }
            set { objectBase.isClickedOn = value; }
        }

        private bool isBeingMoved
        {
            get { return objectBase.isBeingMoved; }
            set { objectBase.isBeingMoved = value; }
        }

        private int PointerHoverCount
        {
            get { return objectBase.PointerHoverCount; }
            set { objectBase.PointerHoverCount = value; }
        }

        private bool isUserHovering
        {
            get { return objectBase.IsUserHovering; }
        }

        private EditorBlockPlacingManager blockPlacingManager
        {
            get { return objectBase.blockPlacingManager; }
        }

        private IPropertiesComponent propertiesComponent
        {
            get { return objectBase.propertiesComponent; }
        }
        
        private void Awake()
        {
            try
            {
                // If the gameObject has a BlockBase, gets it. Otherwise, gets it from the parent.
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


        public void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (isUserHovering)
                {
                    Debug.Log("send function");
                    ManagerSelectingBase.SelectingManagerInstance.ObjectClickedOn(gameObject);
                }
            }
        }
    }
}