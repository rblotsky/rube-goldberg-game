using UnityEngine;

namespace RubeGoldbergGame
{
    public class DebugMouseClick : MonoBehaviour
    {

        void Start()
        {
            
        }
        void Update()
        {
            // Check for mouse input
            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                // Casts the ray and get the first game object hit
                Physics.Raycast(ray, out hit);
            }
        }
    }
}