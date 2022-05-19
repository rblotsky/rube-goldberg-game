using System;
using UnityEngine;

namespace RubeGoldbergGame
{
    public class ObjectSpringPlate : MonoBehaviour
    {
        // DATA //
        public float pushForce = 1000;
        private Rigidbody2D attachedRigidbody;


        // FUNCTIONS //
        // Unity Defaults
        private void Awake()
        {
            // Gets references
            attachedRigidbody = gameObject.GetComponent<Rigidbody2D>();
        }

        private void OnTriggerStay2D(Collider2D col)
        {
            // If the collided object has a rigidbody, pushes it
            if (col.attachedRigidbody != null)
            {
                attachedRigidbody.AddForce(transform.right * pushForce, ForceMode2D.Impulse);
            }
        }
    }
}