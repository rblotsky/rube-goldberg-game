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

        public void UpdateSprite(Sprite newSprite)
        {
            // Updates the sprite
            holoRenderer.sprite = newSprite;
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