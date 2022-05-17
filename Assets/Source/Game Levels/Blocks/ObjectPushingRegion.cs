using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RubeGoldbergGame
{
    public class ObjectPushingRegion : MonoBehaviour
    {
        // DATA //
        public float pushStrength = 10;


        // FUNCTIONS //
        // Unity Defaults
        private void OnTriggerStay2D(Collider2D col)
        {
            PushCollider(col);
        }


        // Block Functionality Functions
        public void PushCollider(Collider2D col)
        {
            if (col.attachedRigidbody != null)
            {
                col.attachedRigidbody.AddForce(gameObject.transform.right * pushStrength, ForceMode2D.Impulse);
            }
        }
    }
}