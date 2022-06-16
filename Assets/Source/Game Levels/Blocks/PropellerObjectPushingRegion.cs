using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RubeGoldbergGame
{
    public class PropellerObjectPushingRegion : MonoBehaviour
    {
        // DATA //
        public float pushStrength = 10;
        public float pushRange = 8f;


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
                float dist = Vector2.Distance(gameObject.transform.position, col.transform.position);
                col.attachedRigidbody.AddForce(gameObject.transform.up * (float)(pushStrength/50 * Math.Pow((pushRange / dist), 1.5)), ForceMode2D.Impulse);
            }
        }
    }
}