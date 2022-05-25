using System;
using UnityEngine;

namespace RubeGoldbergGame
{
    public class ObjectSpringPlate : MonoBehaviour
    {
        // DATA //
        public float pushForce = 1000;
        public float pushTime = 0.3f;
        public float cooldownTime = 1.0f;
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
                if (activeTime > 0)
                {
                    attachedRigidbody.AddForce(transform.right * pushForce, ForceMode2D.Impulse);
                }
                else if (activeTime < -cooldownTime)
                {
                    isPushing = false;
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("Collision: " + collision.collider.name);
            // If the collided object has a rigidbody, pushes it
            if (collision.collider.attachedRigidbody != null && !isPushing)
            {
                isPushing = true;
                activeTime = pushTime;
                Debug.Log("Adding force to: " + collision.collider.name);
                
            }
        }
    }
}