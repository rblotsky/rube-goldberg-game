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
        public Collider2D objCollider;

        // Cached Data
        private Color defaultSpriteColour;
        private bool canPlace = true;
        private Quaternion defaultRotation;
        private Vector3 defaultLocalScale;

        // Properties
        public bool CanPlaceObject { get { return canPlace; } }


        // FUNCTIONS //
        // Unity Defaults
        private void Awake()
        {
            // Gets components
            holoRenderer = GetComponent<SpriteRenderer>();
            objCollider = GetComponent<Collider2D>();

            // Caches data
            defaultSpriteColour = holoRenderer.color;
            defaultRotation = transform.rotation;
            defaultLocalScale = transform.localScale;
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

        public void UpdateSprite(Sprite newSprite, Vector3 newLocalScale)
        {
            // Updates the sprite and scale (scale is set to default if given 0)
            holoRenderer.sprite = newSprite;

            if (newLocalScale == Vector3.zero)
            {
                transform.localScale = defaultLocalScale;
            }
            else
            {
                transform.localScale = newLocalScale;
            }
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
            //TODO: Rework how this works.
            // New Method:
            //  - Scan all colliders *within the collision bounds of this block*
            //  - Ensure its within the bounding boxes
            //  - For nearby colliders, only care about certain ones (eg. Don't care about the effect range of the object pusher)

            // Gets nearby colliders
            Collider2D[] nearbyColliders = Physics2D.OverlapBoxAll(transform.position, objCollider.bounds.size, transform.rotation.eulerAngles.x);

            // Checks if there are other colliders within the bounds
            bool hasFoundOtherCollider = false;
            foreach (Collider2D collider in nearbyColliders)
            {
                // Sets to can't place if there are colliders other than itself
                if (collider != objCollider)
                {
                    hasFoundOtherCollider = true;
                    canPlace = false;
                }
            }

            // Sets to can place if there are no colliders other than itself
            if (!hasFoundOtherCollider)
            {
                canPlace = true;
            }
        }
    }
}