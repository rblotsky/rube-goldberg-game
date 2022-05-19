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

        private void OnCollisionStay2D(Collision2D collision)
        {
            Debug.Log("Collision: " + collision.collider.name);
            // If the collided object has a rigidbody, pushes it
            if (collision.collider.attachedRigidbody != null)
            {
                Debug.Log("Adding force to: " + collision.collider.name);
                attachedRigidbody.AddForce(transform.right * pushForce, ForceMode2D.Impulse);
            }
        }
    }
}