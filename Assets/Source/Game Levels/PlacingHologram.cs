using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace RubeGoldbergGame
{
    public class PlacingHologram : MonoBehaviour
    {
        // DATA //
        // Basic
        public SpriteRenderer holoRenderer;
        public Color cannotPlaceColour = Color.red;
        public BoxCollider2D objCollider;

        // Cached Data
        private Color defaultSpriteColour;
        private bool canPlace = true;
        private Quaternion defaultRotation;
        private Vector3 defaultLocalScale;
        private Vector3 defaultColliderScale;
        private Vector3 defaultColliderOffset;

        // Properties
        public bool CanPlaceObject { get { return canPlace; } }


        // FUNCTIONS //
        // Unity Defaults
        private void Awake()
        {
            // Gets components
            holoRenderer = GetComponent<SpriteRenderer>();
            objCollider = GetComponent<BoxCollider2D>();

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
        }

        public void RotateClockwise(float incrementAmount)
        {
            transform.Rotate(Vector3.forward, incrementAmount);
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
            Collider2D[] nearbyColliders = Physics2D.OverlapBoxAll(transform.position, objCollider.bounds.size, transform.rotation.eulerAngles.x);

            // Stores checks for different conditions
            bool inValidLevelZone = false;
            bool hasFoundOtherCollider = false;
            foreach (Collider2D collider in nearbyColliders)
            {
                // Checks if colliding w/ valid level zone
                if (collider.gameObject.layer == LayerMask.NameToLayer("ValidLevelZone"))
                {
                    inValidLevelZone = true;
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
            if (!hasFoundOtherCollider && inValidLevelZone)
            {
                canPlace = true;
            }
        }
    }
}