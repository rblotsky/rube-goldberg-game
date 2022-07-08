using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using System.Linq;

namespace RubeGoldbergGame
{
    public static class UtilityFuncs
    {
        // FUNCTIONS //
        public static Vector3 ParseVector3(string stringData, char sepChar)
        {
            // Gets components
            string[] components = stringData.Trim().Split(sepChar);

            // Ensures that there are 3 components
            if (components.Length != 3)
            {
                Debug.LogError(string.Format("Could not convert {0} to a Vector3: Invalid number of components!", stringData));
            }

            else
            {
                // Tries to convert to a Vector3, if fails logs error
                try
                {
                    return new Vector3(float.Parse(components[0].Trim()), float.Parse(components[1].Trim()), float.Parse(components[2].Trim()));
                }
                catch (FormatException error)
                {
                    Debug.LogError(string.Format("Could not convert {0} to a Vector3: Bad format! ({1})", stringData, error.Message));
                }
            }

            // Returns a zero vector if fails to convert
            return Vector3.zero;

        }

        public static string SaveVector3ToString(Vector3 data)
        {
            return data.x + "," + data.y + "," + data.z;
        }

        public static Vector3 ClampElementToCanvas(RectTransform element, Canvas canvas, Vector3 newPos)
        {
            // Calculates min and max values for rect position
            float minX = (element.rect.size.x * canvas.scaleFactor * element.pivot.x);
            float maxX = (canvas.pixelRect.size.x - (element.rect.size.x * canvas.scaleFactor * element.pivot.x));
            float minY = (element.rect.size.y * canvas.scaleFactor * element.pivot.y);
            float maxY = (canvas.pixelRect.size.y - (element.rect.size.y * canvas.scaleFactor * Mathf.Abs(element.pivot.y - 1)));

            // Clamps the rect position to the min/max values
            newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
            newPos.y = Mathf.Clamp(newPos.y, minY, maxY);

            // Returns position
            return newPos;
        }

        public static bool IsScreenPosOverUIObject(Vector2 position)
        {
            // Raycasts to find all gameObjects the mouse is pointing at
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

            // Returns whether there exist any on the UI layer
            return results.FindAll(x => x.gameObject.layer == LayerMask.NameToLayer("UI")).Count > 0;
        }
    }
}