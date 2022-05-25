using System;
using UnityEngine;

namespace RubeGoldbergGame
{
    public class ObjectSpringPlate : MonoBehaviour
    {
        // DATA //
        public float pushForce = 1000;
        public float pushTime = 0.3f;
        private float activeTime = 0f;
        private bool isPushing = false;
        private Rigidbody2D attachedRigidbody;


        // FUNCTIONS //
        // Unity Defaults
        private void Awake()
        {
            // Gets references
            attachedRigidbody = gameObject.GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (isPushing)
            {
                activeTime -= Time.unscaledDeltaTime;
                attachedRigidbody.AddForce(transform.right * pushForce, ForceMode2D.Impulse);
                if (activeTime < 0)
                {
                    isPushing = false;
                }
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            Debug.Log("Collision: " + collision.collider.name);
            // If the collided object has a rigidbody, pushes it
            if (collision.collider.attachedRigidbody != null)
            {
                isPushing = true;
                activeTime = pushTime;
                Debug.Log("Adding force to: " + collision.collider.name);
                
                //attachedRigidbody.AddForce(transform.right * pushForce, ForceMode2D.Impulse);
            }
        }
    }
}