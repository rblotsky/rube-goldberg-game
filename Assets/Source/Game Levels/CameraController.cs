using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RubeGoldbergGame
{
    public class CameraController : MonoBehaviour
    {
        // CAMERA DATA //
        // Base camera values
        public float zoomSpeed = 25;
        public float moveSpeed = 10;
        public float minZoom = 1;
        public float maxZoom = 300;
        public float maxDistanceFromInitial = 50;

        // References
        public Camera mainCam;

        // Cached data
        private Vector3 initialCameraPos;


        // FUNCTIONS //
        // Unity Defaults
        private void Awake()
        {
            // Gets references
            mainCam = Camera.main;

            // Caches initial data
            initialCameraPos = transform.position;
        }

        private void LateUpdate()
        {
            // Uses horizontal/vertical axes to translate camera
            float moveHorizontal = Input.GetAxisRaw("Horizontal");
            float moveVertical = Input.GetAxisRaw("Vertical");
            
            // Gets the new position
            Vector3 newCameraPos = mainCam.transform.position + (new Vector3(moveHorizontal, moveVertical, 0) * moveSpeed * Time.unscaledDeltaTime);

            // Only moves if within allowed radius of initial position
            if((newCameraPos - initialCameraPos).sqrMagnitude < maxDistanceFromInitial*maxDistanceFromInitial)
            {
                mainCam.transform.position = newCameraPos;
            }
            
            // Zooms in/out depending on whether player uses zoom key
            float zoomAxis = Input.GetAxisRaw("Mouse ScrollWheel");
            mainCam.orthographicSize += zoomAxis * -zoomSpeed;
            mainCam.orthographicSize = Mathf.Clamp(mainCam.orthographicSize, minZoom, maxZoom);
        }
    }
}