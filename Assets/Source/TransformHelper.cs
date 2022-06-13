using UnityEngine;

namespace RubeGoldbergGame
{
    public class TransformHelper
    {
        //helper to set a transform to a new transform
        public static void SetTransform(Transform orig, Transform newTrans)
        {
            orig.position = newTrans.position;
            orig.rotation = newTrans.rotation;
            orig.localScale = newTrans.localScale;
        }
    }
}