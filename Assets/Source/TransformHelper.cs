using UnityEngine;

namespace RubeGoldbergGame
{
    public class TransformHelper
    {
        //helper to set a transform to a new transform
        public static void SetTransform(Transform orig, Vector3 newPos, Quaternion newRot)
        {
            orig.position = newPos;
            orig.rotation = newRot;
        }
    }
}