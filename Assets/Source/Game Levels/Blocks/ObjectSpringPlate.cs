using System;
using UnityEngine;

namespace RubeGoldbergGame
{
    public class ObjectSpringPlate : MonoBehaviour
    {
        // DATA //
        public float pushForce = 20; //min/max are 0.1,40?
        public float cooldownTime = 1.0f;
        private float activeTime = 0f;
        private bool onCooldown = false;
        private Rigidbody2D attachedRigidbody;
        
        public Transform restingPosition;

        // FUNCTIONS //
        // Unity Defaults
        private void Awake()
        {
            // Gets references
            attachedRigidbody = gameObject.GetComponent<Rigidbody2D>();
            restingPosition = this.transform;
        }

        private void Update()
        {
            if (onCooldown)
            {
                //active time above 0 is pushing, active time below 0 is cooldown phase
                activeTime -= Time.unscaledDeltaTime;
                if (activeTime < 0)
                {
                    onCooldown = false;
                }
            }
        }

        public void ResetPlatePosition()
        {
            TransformHelper.SetTransform(this.transform, restingPosition);
        }
        
        
        //trigger function
        //starts the spring pushing process
        private void OnCollisionStay2D(Collision2D collision)
        {
            Debug.Log("Spring Collision: " + collision.collider.name);
            // If the collided object has a rigidbody and the process is not started yet
            if (collision.collider.attachedRigidbody != null && !onCooldown)
            {
                onCooldown = true;
                attachedRigidbody.AddForce(transform.up * pushForce, ForceMode2D.Impulse);
                activeTime = cooldownTime;
            }
        }
    }
}