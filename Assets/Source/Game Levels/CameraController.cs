using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RubeGoldbergGame
{
    public class CameraController : MonoBehaviour
    {
        // CAMERA DATA //
        public float zoomSpeed = 50;
        public float minZoom = 1;
        public float maxZoom = 300;
        public Camera mainCam;


        // FUNCTIONS //
        // Unity Defaults
        private void Start()
        {
            mainCam = Camera.main;
        }

        private void LateUpdate()
        {
            // moves the camera based on if movement keys are pressed
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            mainCam.transform.position += new Vector3(moveHorizontal * 0.01f, moveVertical * 0.01f, 0);
            //TODO: clamp the movement to only view within the level
            
            // Zooms in/out depending on whether player uses zoom key
            float zoomAxis = Input.GetAxis("Mouse ScrollWheel");
            mainCam.orthographicSize += zoomAxis * -zoomSpeed;
            mainCam.orthographicSize = Mathf.Clamp(mainCam.orthographicSize, minZoom, maxZoom);
        }
    }
}