using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;

namespace RubeGoldbergGame
{
    public class PlacingHologram : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        // DATA //
        // References
        public SpriteRenderer holoRenderer;
        public BoxCollider2D objCollider;
        public GameObject placingArea;
        public EditorBlockPlacingManager blockManager;
        public GameObject debugPoint;

        // Colours
        public Color cannotPlaceColour = Color.red;

        // Cached Data
        private Color defaultSpriteColour;
        private bool canPlace = true;
        private Quaternion defaultRotation;
        private Vector3 defaultLocalScale;
        private Vector3 defaultColliderScale;
        private Vector3 defaultColliderOffset;

        public GameObject placementArea;

        // Properties
        public bool CanPlaceObject { get { return canPlace; } }


        // FUNCTIONS //
        // Unity Defaults
        private void Awake()
        {
            // Gets components
            holoRenderer = GetComponent<SpriteRenderer>();
            objCollider = GetComponent<BoxCollider2D>();
            blockManager = FindObjectOfType<EditorBlockPlacingManager>();
            placementArea = GameObject.FindGameObjectWithTag("PlacingArea");

            // Caches data
            defaultSpriteColour = holoRenderer.color;
            defaultRotation = transform.rotation;
            defaultLocalScale = transform.localScale;
            defaultColliderOffset = objCollider.offset;
            defaultColliderScale = objCollider.size;
        }

        // External Management
        public void ToggleHologram(bool status)
        {
            gameObject.SetActive(status);
            placementArea.layer = (status) ? 0 : 2; //enabling or disabling the placement area raycast layer
        }
    
        public void ResetRotation()
        {
            transform.rotation = defaultRotation;
        }

        public void UpdateSprite(Sprite newSprite, BoxCollider2D colliderInfo)
        {
            // Gets scales
            Vector3 newLocalScale = defaultLocalScale;
            Vector3 newColliderScale = defaultColliderScale;
            Vector3 newColliderOffset = defaultColliderOffset;

            // If collider info is provided, updates the scales and offsets
            if(colliderInfo != null)
            {
                newLocalScale = colliderInfo.transform.localScale;
                newColliderScale = colliderInfo.size;
                newColliderOffset = colliderInfo.offset;
            }

            // Updates the sprite, scale, and collider data
            holoRenderer.sprite = newSprite;
            transform.localScale = newLocalScale;
            objCollider.offset = newColliderOffset;
            objCollider.size = newColliderScale;
        }

        public void UpdatePosition(Vector3 position)
        {
            
            transform.position = position;
            //objCollider.attachedRigidbody.MovePosition(position);
        }

        public void RotateClockwise(float incrementAmount)
        {
            transform.Rotate(Vector3.forward, incrementAmount);
            //objCollider.transform.Rotate(Vector3.forward, incrementAmount);
            
        }

        public void UpdateColour()
        {
            // Updates colour to red if can't place, default if can
            if (canPlace)
            {
                holoRenderer.color = defaultSpriteColour;
            }

            else
            {
                holoRenderer.color = cannotPlaceColour;
            }

        }

        public void UpdateCanPlace()
        {
            // Gets nearby colliders
            Collider2D[] nearbyColliders = Physics2D.OverlapBoxAll(transform.position, Vector2.Scale(transform.lossyScale, objCollider.size), transform.rotation.eulerAngles.z);
            
            // Stores checks for different conditions
            bool inPlacingArea = false;
            bool hasFoundOtherCollider = false;
            foreach (Collider2D collider in nearbyColliders)
            {
                // Checks if colliding w/ placing area
                if (collider.gameObject == placingArea)
                {
                    inPlacingArea = true;
                }

                // Otherwise, ensures it's not colliding with other objects than itself
                else if (collider != objCollider)
                {
                    // Ensures the collider isn't a trigger
                    if (!collider.isTrigger)
                    {
                        hasFoundOtherCollider = true;
                        canPlace = false;
                    }  
                }
            }

            // Sets to can place if there are no colliders other than itself and in valid zone
            if (!hasFoundOtherCollider && inPlacingArea)
            {
                canPlace = true;
            }
        }

        public void OnPointerClick(PointerEventData pointerData)
        {
            // Places the hologram if it is clicked
            Debug.Log("hologram clicked!");
            blockManager.AttemptPlaceBlock(transform.position);

            // Also tries deleting the block if the selection type is deletion
            blockManager.AttemptDeleteObject();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            //Instantiate(debugPoint, gameObject.transform.position, gameObject.transform.rotation);
            Debug.Log("We detected the mouse enter the hologram area");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //var debugPoint2 = Instantiate(debugPoint, gameObject.transform.position, gameObject.transform.rotation);
            //debugPoint2.GetComponent<SpriteRenderer>().color = Color.red;
            Debug.Log("We detected the mouse exit the hologram area");
        }
    }
}