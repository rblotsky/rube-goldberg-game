using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;

namespace RubeGoldbergGame
{
    public class PlacingHologram : MonoBehaviour, IPointerClickHandler
    {
        // DATA //
        // References
        public SpriteRenderer holoRenderer;
        public BoxCollider2D objCollider;
        public EditorBlockPlacingManager blockManager;
        public GameObject debugPoint;

        // Colours
        public Color cannotPlaceColour = Color.red;

        // Cached Data
        private Color defaultSpriteColour;
        private Quaternion defaultRotation;
        private Vector3 defaultLocalScale;
        private Vector3 defaultColliderScale;
        private Vector3 defaultColliderOffset;
        public GameObject placementArea;

        // Events
        public event EmptyDelegate onHologramClick;


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

        public void UpdateColour(bool placeable)
        {
            // Updates colour to red if can't place, default if can
            if (placeable)
            {
                holoRenderer.color = defaultSpriteColour;
            }

            else
            {
                holoRenderer.color = cannotPlaceColour;
            }

        }

        public bool GetCanPlace()
        {
            return UtilityFuncs.GetCanPlaceBlock(gameObject, objCollider);
        }

        public void OnPointerClick(PointerEventData pointerData)
        {
            // Runs click events
            blockManager.HandleHologramClick(transform.position);

            if(onHologramClick != null)
            {
                onHologramClick();
            }    
        }
    }
}