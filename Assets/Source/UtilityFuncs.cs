using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
                    return new Vector3(float.Parse(components[0]), float.Parse(components[1]), float.Parse(components[2]));
                }
                catch (FormatException error)
                {
                    Debug.LogError(string.Format("Could not convert {0} to a Vector3: Bad format! ({1})", stringData, error.Message));
                }
            }

            // Returns a zero vector if fails to convert
            return Vector3.zero;

        }
    }
}