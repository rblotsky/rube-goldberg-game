using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace RubeGoldbergGame
{
    public class CameraController : MonoBehaviour
    {
        // CAMERA DATA //
        // Base camera values
        public float zoomSpeed = 25;
        public float moveSpeed = 10;
        public float minZoom = 0.2f;
        public float maxZoom = 300;

        // References
        public Camera mainCam;
        public BoxCollider2D placingArea;

        // Cached data
        private Vector3 initialCameraPos;
        private Vector3 mouseCameraOffset;


        // FUNCTIONS //
        // Unity Defaults
        private void Awake()
        {
            // Gets references
            mainCam = Camera.main;
            placingArea = FindObjectsOfType<BoxCollider2D>().First(x => x.CompareTag("PlacingArea"));

            // Positions itself in the center of the placing area
            Vector3 placingAreaCenter = placingArea.bounds.center;
            transform.position = (Vector3)placingArea.offset + placingArea.transform.position + new Vector3(0,0,-1);
            
            // Caches initial data
            initialCameraPos = transform.position;
        }

        private void LateUpdate()
        {
            // Uses horizontal/vertical axes to translate camera
            float moveHorizontal = Input.GetAxisRaw("Horizontal");
            float moveVertical = Input.GetAxisRaw("Vertical");
            MoveFromBasicInput(moveHorizontal, moveVertical);
            
            // Zooms in/out depending on whether player uses zoom key
            float zoomAxis = Input.GetAxisRaw("Mouse ScrollWheel");
            mainCam.orthographicSize += zoomAxis * -zoomSpeed;
            mainCam.orthographicSize = Mathf.Clamp(mainCam.orthographicSize, minZoom, maxZoom);
        }


        // Camera Movement Functions
        public void MoveFromBasicInput(float horizontal, float vertical)
        {
            // Gets the new position
            Vector3 newCameraPos = mainCam.transform.position + (new Vector3(horizontal, vertical, 0) * moveSpeed * Time.unscaledDeltaTime);

            // Only moves if within placing area
            if (placingArea.bounds.Contains(newCameraPos+new Vector3(0,0,1)))
            {
                mainCam.transform.position = newCameraPos;
            }
        }
    }
}